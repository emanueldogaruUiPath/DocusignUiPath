﻿using BenMann.Docusign;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;

namespace Docusign.Authentication
{
    [DisplayName("Get Authorization Url")]
    public class GetAuthorizationUrl : NativeActivity
    {
        [Category("Output")]
        [DisplayName("Authentication Url")]
        public OutArgument<string> AuthenticationUrl { get; set; }

        public GetAuthorizationUrl()
        {
            base.Constraints.Add(BenMann.Docusign.Activities.Constraints.CheckParent<DocusignApplicationScope>());
        }
        protected override void Execute(NativeActivityContext context)
        {
            var property = context.DataContext.GetProperties()["authAgent"];
            if (property.GetValue(context.DataContext)== null)
            {
                throw new Exception("DocuSign activities must be within DocuSign Context activity");
            }
            AuthenticationAgent authAgent = (AuthenticationAgent)property.GetValue(context.DataContext);
            AuthenticationUrl.Set(context, authAgent.authUrl);
        }
    }
}
