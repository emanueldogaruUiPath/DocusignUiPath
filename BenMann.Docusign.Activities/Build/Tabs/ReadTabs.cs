using System;
using System.Activities;
using System.Net.Http;
using System.ComponentModel;
using BenMann.Docusign;
using Docusign.DocusignTypes;
using System.Collections.Generic;
using Microsoft.VisualBasic.Activities;

namespace Docusign.Tabs
{
    public class TabsResponse
    {
        public List<SignHereTabsResponse> signHereTabs;
        public List<FullNameTabs> fullNameTabs;
        public List<DateSignedTabs> dateSignedTabs;
        public List<TitleTabs> titleTabs;
        public List<TextTabs> textTabs;
        public List<RadioGroupTabs> radioGroupTabs;
    }
    public class TextResponse
    {
        public string value;
        public string originalValue;
    }
    public class SignHereTabsResponse
    {
        public string name;
        public string status = "unsigned";
    }

    public class FullNameTabs
    {
        public string name;
        public string value;
    }

    public class DateSignedTabs
    {
        public string name;
        public string value;
    }

    public class TitleTabs
    {
        public string value;
    }

    public class TextTabs
    {
        public string value;
        public string tabLabel;
    }

    public class RadioGroupTabs
    {
        public string groupName;
        public List<Radio> radios;
        public bool IsSelected()
        {
            bool ret = false;
            foreach (var radio in radios)
            {
                if (radio.selected)
                    ret = true;
            }
            return ret;
        }

        public string GetSelected()
        {
            foreach (var radio in radios)
            {
                if (radio.selected)
                    return radio.value;
            }
            return "None Selected";
        }
    }

    public class Radio
    {
        public string value;
        public bool selected;
    }
    public sealed class ReadTabs : DocusignActivity
    {
        string mEnvelopeId;
        string mDocumentId;
        TabsResponse resObj;
        Action ReadTabsAction;

        [Category("Input")]
        [DisplayName("Envelope ID")]
        [Description("Envelope ID")]
        [RequiredArgument]
        public InArgument<string> EnvelopeId { get; set; }

        [Category("Input")]
        [DisplayName("Document ID")]
        [Description("Document ID")]
        [RequiredArgument]
        public InArgument<string> DocumentId { get; set; }

        [Category("Output")]
        [DisplayName("Tabs Filled in the Document")]
        [Description("Tabs Filled in the Document")]
        public OutArgument<TabsResponse> FilledTabs { get; set; }


        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            LoadAuthentication(context);

            if (EnvelopeId.Get(context) != null)
                mEnvelopeId = EnvelopeId.Get(context);
            else
                throw new System.ArgumentException("Envelope ID is required!");
            if (DocumentId.Get(context) != null)
                mDocumentId = DocumentId.Get(context);
            else
                throw new System.ArgumentException("Document ID is required!");
            ReadTabsAction = new Action(_GetTabs);
            return ReadTabsAction.BeginInvoke(callback, state);
        }

        void _GetTabs()
        {
            DocusignResponse response = new DocusignResponse();
            SendRestRequest(response, HttpMethod.Get, "envelopes/" + mEnvelopeId + "/documents/" + mDocumentId + "/tabs", null).Wait();
            if (response.IsError)
            {
                response.Throw();
            }
            resObj = response.GetData<TabsResponse>();
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            ReadTabsAction.EndInvoke(result);
            FilledTabs.Set(context, resObj);       
        }
    }

}
