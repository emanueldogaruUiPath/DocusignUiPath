using System;
using System.Activities;
using System.Net.Http;
using System.ComponentModel;
using BenMann.Docusign;
using Docusign.DocusignTypes;
using System.Collections.Generic;
using Microsoft.VisualBasic.Activities;

namespace Docusign.Recipients
{
    public sealed class GetRecipients : DocusignActivity
    {
        Dictionary<string, string> Query = new Dictionary<string, string>();
        RecipientsResponse resObj;
        Action GetRecipientsDelegate;

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Envelope ID")]
        [Description("Envelope ID")]
        public InArgument<string> EnvelopeID { get; set; }

        [Category("Output")]
        [DisplayName("Recipients List")]
        [Description("Recipients List")]
        public OutArgument<RecipientsResponse> RecipientsResponseOut { get; set; }

        string envelopeID;

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            LoadAuthentication(context);

            Query["include_extended"] = "true";
            Query["include_tabs"] = "true";
            envelopeID = EnvelopeID.Get(context);

            GetRecipientsDelegate = new Action(_GetRecipients);
            return GetRecipientsDelegate.BeginInvoke(callback, state);
        }

        void _GetRecipients()
        {
            DocusignResponse response = new DocusignResponse();
            SendRestRequest(response, HttpMethod.Get, "envelopes/"+envelopeID+"/recipients", null, Query).Wait();
            if (response.IsError)
            {
                response.Throw();
            }
            resObj = response.GetData<RecipientsResponse>();
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            GetRecipientsDelegate.EndInvoke(result);
            RecipientsResponseOut.Set(context, resObj);
        }
    }

    public class RecipientsResponse
    {
       public List<RecipientInfo> signers;
    }

    public class RecipientInfo
    {
        public string name;
        public string email;
        public string status;
        public string declinedReason;
        public DateTime deliveredDateTime = DateTime.MinValue;
        public Tabs.TabsResponse tabs;
    }
}

