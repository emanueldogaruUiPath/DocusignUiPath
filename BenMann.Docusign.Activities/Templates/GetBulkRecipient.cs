using System;
using System.Activities;
using System.Net.Http;
using System.ComponentModel;
using BenMann.Docusign;
using Docusign.DocusignTypes;
using System.Collections.Generic;
using Microsoft.VisualBasic.Activities;

namespace Docusign.Templates
{
    [DisplayName("Get Bulk Recipient")]
    [Description("Get the unique bulk recipient associated to a template")]
    public sealed class GetBulkRecipient : DocusignActivity
    {
        string mEnvelopeId;
        RecipientsListResponse resObj;
        Action GetRecipientsDelegate;

        [Category("Input")]
        [DisplayName("Envelope ID")]
        [Description("Envelope ID")]
        [RequiredArgument]
        public InArgument<string> EnvelopeID { get; set; }

        [Category("Output")]
        [DisplayName("Bulk Recipient ID")]
        [Description("ID of the bulk recipient (exception if does not exists)")]
        public OutArgument<string> BulkRecipientID { get; set; }


        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            LoadAuthentication(context);

            if (EnvelopeID.Get(context) != null)
                mEnvelopeId = EnvelopeID.Get(context);
            GetRecipientsDelegate = new Action(_ListDocuments);
            return GetRecipientsDelegate.BeginInvoke(callback, state);
        }

        void _ListDocuments()
        {
            DocusignResponse response = new DocusignResponse();
            SendRestRequest(response, HttpMethod.Get, "templates/" + mEnvelopeId +"/recipients", null).Wait();
            if (response.IsError)
            {
                response.Throw();
            }
            resObj = response.GetData<RecipientsListResponse>();
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            GetRecipientsDelegate.EndInvoke(result);
            bool existsBulkRecipient = false;
            foreach (var signer in resObj.signers)
            {
                if (signer.isBulkRecipient)
                {
                    BulkRecipientID.Set(context, signer.recipientId);
                    existsBulkRecipient = true;
                }
            }
            if (!existsBulkRecipient)
            {
                throw new Exception("Template does not have a bulk recipient!");
            }
        }
    }

    public class RecipientsListResponse
    {
        public List<SignerResponse> signers;
    }
    public class SignerResponse
    {
        public string name;
        public string email;
        public bool isBulkRecipient;
        public string recipientId;
    }
}
