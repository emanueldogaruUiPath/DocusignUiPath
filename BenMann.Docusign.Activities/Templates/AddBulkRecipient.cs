using System;
using System.Collections.Generic;
using System.Activities;
using System.ComponentModel;
using System.Net.Http;
using BenMann.Docusign;
using Docusign.DocusignTypes;


    namespace Docusign.Templates
{
    [DisplayName("Add Template Bulk Recipients")]
    [Description("Add new or update the bulk recipient file associated to a template")]
    public sealed class AddBulkRecipient : DocusignActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Template ID")]
        [Description("Template ID")]
        public InArgument<string> TemplateId { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Bulk Recipient Id")]
        [Description("Bulk Recipient Id (obtain it with 'Get Bulk Recipient' activity)")]
        public InArgument<string> RecipientId { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Recipients File")]
        [Description("Location of the CSV Recipients File")]
        public InArgument<string> FilePath { get; set; }

        [Category("Output")]
        [DisplayName("Bulk Recipients Response")]
        [Description("Summary of the Bulk Recipients Response")]
        public OutArgument<BulkRecipientsSummaryResponse> BulkResponse { get; set; }

        private string templateId;
        private string recipientId;
        private string filePath;

        BulkRecipientsSummaryResponse resObj;
        Action CreateBulkEnvelopeDelegate;


        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            LoadAuthentication(context);

            templateId = TemplateId.Get(context);
            recipientId = RecipientId.Get(context);
            filePath = FilePath.Get(context);
            
            CreateBulkEnvelopeDelegate = new Action(_AddBulkRecipientAction);
            return CreateBulkEnvelopeDelegate.BeginInvoke(callback, state);
        }

        void _AddBulkRecipientAction()
        {
            DocusignResponse response = new DocusignResponse();
            SendRestRequest(response, HttpMethod.Put, "envelopes/"+templateId+"/recipients/" + recipientId + "/bulk_recipients", filePath).Wait();
            if (response.IsError)
            {
                // more compex error handling is needed
                string messageError = "Error trying to Add bulk recipient: ";
                try
                {
                    resObj = response.GetData<BulkRecipientsSummaryResponse>();
                    foreach (var bulkRecipient in resObj.bulkRecipients)
                    {
                        messageError += "\nEmail: " + bulkRecipient.email + "\n Name: " + bulkRecipient.name
                            + "Errors: ";
                        foreach (var error in bulkRecipient.errorDetails)
                        {
                            messageError += "\n " + error.message;
                        }
                    }
                    foreach (var errorDetail in resObj.errorDetails)
                    {
                        messageError += "Error code: " + errorDetail.errorCode + " - " + errorDetail.message + "\n";
                    }
                }
                catch (System.Exception)
                {
                    response.Throw();
                }
                throw new System.Exception(messageError);
            }
            resObj = response.GetData<BulkRecipientsSummaryResponse>();
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            CreateBulkEnvelopeDelegate.EndInvoke(result);
            BulkResponse.Set(context, resObj);
        }
    }

    public class BulkRecipientsSummaryResponse
    {
        public List<BulkRecipients> bulkRecipients = new List<BulkRecipients>();
        public string bulkRecipientsCount;
        public List<ErrorDetails> errorDetails = new List<ErrorDetails>();
    }

    public class BulkRecipients
    {
        public string rowNumber;
        public string email;
        public string name;
        public List<ErrorDetails> errorDetails;
    }

    public class ErrorDetails
    {
        public string errorCode;
        public string message;
    }
}
