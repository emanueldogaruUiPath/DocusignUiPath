using System;
using System.Activities;
using System.Net.Http;
using System.ComponentModel;
using BenMann.Docusign;
using Docusign.DocusignTypes;
using System.Collections.Generic;
using Microsoft.VisualBasic.Activities;
using System.Threading.Tasks;
using System.IO;

namespace Docusign.Documents
{
    public sealed class DownloadDocument : DocusignActivity
    {
        Dictionary<string, string> Query = new Dictionary<string, string>();
        string mEnvelopeID;
        string mDocumentID;
        string mOutFile;
        HttpContent resObj;
        Action GetDocumentDelegate;

        [Category("Input")]
        [DisplayName("EnvelopeID")]
        [Description("ID of the envelope")]
        [RequiredArgument]
        public InArgument<string> EnvelopeID { get; set; }

        [Category("Input")]
        [DisplayName("DocumentID")]
        [Description("ID of the Document")]
        [RequiredArgument]
        public InArgument<string> DocumentID { get; set; }

        [Category("Input")]
        [DisplayName("Output File Path")]
        [Description("FilePath Of The Downloaded File")]
        [RequiredArgument]
        public InArgument<string> OutFile { get; set; }



        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            LoadAuthentication(context);

            if (EnvelopeID.Get(context) != null)
                mEnvelopeID = EnvelopeID.Get(context);
            else
                throw new System.ArgumentException("Envelope ID is required!");

            if (DocumentID.Get(context) != null)
                mDocumentID = DocumentID.Get(context);
            else
                throw new System.ArgumentException("Document id is required!");

            if (OutFile.Get(context) != null)
                mOutFile = OutFile.Get(context);
            else
                throw new System.ArgumentException("Out filepath is required!");

            GetDocumentDelegate = new Action(_DownloadDocument);
            return GetDocumentDelegate.BeginInvoke(callback, state);
        }

        void _DownloadDocument()
        {
            DocusignResponse response = new DocusignResponse();
            SendRestRequest(response, HttpMethod.Get, "envelopes/"+mEnvelopeID+"/documents/"+mDocumentID, null, Query).Wait();
            if (response.IsError)
            {
                response.Throw();
            }
            resObj = response.getStreamContent();
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            GetDocumentDelegate.EndInvoke(result);
            Utils.ReadAsFileAsync(resObj, mOutFile, true).ContinueWith((readTask) => {});
        }

        
    }
}
