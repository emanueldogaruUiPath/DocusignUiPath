using System;
using System.Activities;
using System.Net.Http;
using System.ComponentModel;
using BenMann.Docusign;
using Docusign.DocusignTypes;

namespace Docusign.Templates
{
    public class TemplateInfo
    {
        public EnvelopeTemplateDefinition envelopeTemplateDefinition;
        public Notification notification = new Notification();
    }

    public class EnvelopeTemplateDefinition
    {
        public String name;
    }

    public class Notification
    {
        public Expirations expirations = new Expirations();
    }

    public class Expirations
    {
        public String expireAfter;
        public bool expireEnabled;
        public String expireWarn;
    }

    public class TemplateSummaryResponse
    {

    }

    public sealed class UpdateTemplate : DocusignActivity
    {
        

        [Category("Input")]
        [DisplayName("Template ID")]
        [Description("Template ID")]
        public InArgument<string> TemplateID { get; set; }

        String templateID;
        TemplateSummaryResponse resObj;

        Action UpdateTemplateDelegate;


        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            LoadAuthentication(context);

            templateID = TemplateID.Get(context);

            UpdateTemplateDelegate = new Action(_SendEnvelope);
            return UpdateTemplateDelegate.BeginInvoke(callback, state);
        }

        void _SendEnvelope()
        {
            TemplateInfo send = new TemplateInfo();
            send.notification.expirations.expireAfter = "40";
            send.notification.expirations.expireEnabled = true;
            send.notification.expirations.expireWarn = "5";
            DocusignResponse response = new DocusignResponse();
            SendRestRequest(response, HttpMethod.Put, "templates/"+templateID, send).Wait();
            if (response.IsError)
            {
                response.Throw();
            }
            resObj = response.GetData<TemplateSummaryResponse>();
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            UpdateTemplateDelegate.EndInvoke(result);
        }
    }
}