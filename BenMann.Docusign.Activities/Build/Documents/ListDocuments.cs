using System;
using System.Activities;
using System.Net.Http;
using System.ComponentModel;
using BenMann.Docusign;
using Docusign.DocusignTypes;
using System.Collections.Generic;
using Microsoft.VisualBasic.Activities;

namespace Docusign.Documents
{
    public sealed class ListDocuments : DocusignActivity
    {
        string mEnvelopeId;
        DocumentResponse resObj;
        Action GetDocumentsDelegate;

        [Category("Input")]
        [DisplayName("Envelope ID")]
        [Description("Envelope ID")]
        [RequiredArgument]
        public InArgument<string> EnvelopeID { get; set; }

        [Category("Output")]
        [DisplayName("Documents List")]
        [Description("List of Documents returned")]
        public OutArgument<DocumentInfoList> DocumentList { get; set; }


        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            LoadAuthentication(context);

            if (EnvelopeID.Get(context) != null)
            {
                mEnvelopeId = EnvelopeID.Get(context);
            }
            else
            {
                throw new System.ArgumentException("Envelope ID is required!");
            }
            GetDocumentsDelegate = new Action(_ListDocuments);
            return GetDocumentsDelegate.BeginInvoke(callback, state);
        }

        void _ListDocuments()
        {
            DocusignResponse response = new DocusignResponse();
            SendRestRequest(response, HttpMethod.Get, "envelopes/" + mEnvelopeId +"/documents", null).Wait();
            if (response.IsError)
            {
                response.Throw();
            }
            resObj = response.GetData<DocumentResponse>();
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            GetDocumentsDelegate.EndInvoke(result);
            DocumentInfoList envelopeInfoList = new DocumentInfoList();
            foreach (DocumentInfo envInf in resObj.envelopeDocuments)
            {
                envelopeInfoList.Add(envInf);
            }
            DocumentList.Set(context, envelopeInfoList);
        }
    }
}
