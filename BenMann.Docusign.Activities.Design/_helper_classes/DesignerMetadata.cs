﻿using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.Drawing;
using BenMann.Docusign.Activities.Design.Authentication;
using BenMann.Docusign.Activities.Design.Build.Envelopes;
using BenMann.Docusign.Activities.Design.Build.Recipients;
using BenMann.Docusign.Activities.Design.Build.Tabs;
using BenMann.Docusign.Activities.Design.Build.Tabs.Display;
using BenMann.Docusign.Activities.Design.Build.Tabs.GUI;
using BenMann.Docusign.Activities.Design.Build.Tabs.Input;
using BenMann.Docusign.Activities.Design.Build.Tabs.Signing;
using BenMann.Docusign.Activities.Design.Templates;
using Docusign;
using Docusign.Authentication;
using Docusign.Basic;
using Docusign.Documents;
using Docusign.Envelopes;
using Docusign.Recipients;
using Docusign.Tabs.Display;
using Docusign.Tabs.GUI;
using Docusign.Tabs.Input;
using Docusign.Tabs.Signing;
using Docusign.Templates;
using BenMann.Docusign.Activities.Design.Build.Documents;
using Docusign.Tabs;

namespace BenMann.Docusign.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            AttributeTableBuilder attributeTableBuilder = new AttributeTableBuilder();

            attributeTableBuilder.AddCustomAttributes(typeof(DocusignApplicationScope), new DesignerAttribute(typeof(DocuSignContextDesigner)));

            //Authentication
            attributeTableBuilder.AddCustomAttributes(typeof(Authenticate), new DesignerAttribute(typeof(AuthenticateActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(GetAuthorizationUrl), new DesignerAttribute(typeof(GetAuthorizationUrlActivityDesigner)));

            //Basic
            attributeTableBuilder.AddCustomAttributes(typeof(RequestSignature), new DesignerAttribute(typeof(RequestSignatureActivityDesigner)));

            //Build
            //Build - Documents
            attributeTableBuilder.AddCustomAttributes(typeof(AttachDocument), new DesignerAttribute(typeof(AttachDocumentActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(DownloadDocument), new DesignerAttribute(typeof(DownloadDocumentActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(ListDocuments), new DesignerAttribute(typeof(ListDocumentsActivityDesigner)));
            //Build - Envelopes
            attributeTableBuilder.AddCustomAttributes(typeof(CreateEnvelope), new DesignerAttribute(typeof(CreateEnvelopeActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(SendEnvelope), new DesignerAttribute(typeof(SendEnvelopeActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(ListEnvelopes), new DesignerAttribute(typeof(ListEnvelopesActivityDesigner)));

            //Build - Recipients
            attributeTableBuilder.AddCustomAttributes(typeof(AddAgent), new DesignerAttribute(typeof(AddAgentActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddCarbonCopy), new DesignerAttribute(typeof(AddCarbonCopyActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddCertifiedDelivery), new DesignerAttribute(typeof(AddCertifiedDeliveryActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddEditor), new DesignerAttribute(typeof(AddEditorActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddIntermediary), new DesignerAttribute(typeof(AddIntermediaryActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddSigner), new DesignerAttribute(typeof(AddSignerActivityDesigner)));

            //Build - Tabs
            //Build - Tabs - Display
            attributeTableBuilder.AddCustomAttributes(typeof(AddDateSignedTab), new DesignerAttribute(typeof(AddDateSignedActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddEmailAddressTab), new DesignerAttribute(typeof(AddEmailAddressActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddEnvelopeIDTab), new DesignerAttribute(typeof(AddEnvelopeIDActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddFirstNameTab), new DesignerAttribute(typeof(AddFirstNameActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddFullNameTab), new DesignerAttribute(typeof(AddFullNameActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddLastNameTab), new DesignerAttribute(typeof(AddLastNameActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddNoteTab), new DesignerAttribute(typeof(AddNoteActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddTextDisplayTab), new DesignerAttribute(typeof(AddTextActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddTitleTab), new DesignerAttribute(typeof(AddTitleActivityDesigner)));
            //Build - Tabs - GUI
            attributeTableBuilder.AddCustomAttributes(typeof(AddCheckboxTab), new DesignerAttribute(typeof(AddCheckboxActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddFormulaTab), new DesignerAttribute(typeof(AddFormulaActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddListTab), new DesignerAttribute(typeof(AddListActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddRadioGroupTab), new DesignerAttribute(typeof(AddRadioGroupActivityDesigner)));
            //Build - Tabs - input
            attributeTableBuilder.AddCustomAttributes(typeof(AddCompanyTab), new DesignerAttribute(typeof(AddCompanyActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddDateTab), new DesignerAttribute(typeof(AddDateActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddEmailTab), new DesignerAttribute(typeof(AddEmailActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddNumberTab), new DesignerAttribute(typeof(AddNumberActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddSignerAttachmentTab), new DesignerAttribute(typeof(AddSignerAttachmentActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddSSNTab), new DesignerAttribute(typeof(AddSSNActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddTextInputTab), new DesignerAttribute(typeof(AddTextActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddZipTab), new DesignerAttribute(typeof(AddZipActivityDesigner)));
            //Build - Tabs - Signing
            attributeTableBuilder.AddCustomAttributes(typeof(AddApproveTab), new DesignerAttribute(typeof(AddApproveActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddDeclineTab), new DesignerAttribute(typeof(AddDeclineActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddInitialHereTab), new DesignerAttribute(typeof(AddInitialHereActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddSignHereTab), new DesignerAttribute(typeof(AddSignHereTabActivityDesigner)));
            // Build - Tabs - Read Tabs
            attributeTableBuilder.AddCustomAttributes(typeof(ReadTabs), new DesignerAttribute(typeof(ReadTabsActivityDesigner)));
            //Templates
            attributeTableBuilder.AddCustomAttributes(typeof(LoadTemplate), new DesignerAttribute(typeof(LoadTemplateActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AddBulkRecipient), new DesignerAttribute(typeof(AddBulkRecipientsActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(AssignTemplateRole), new DesignerAttribute(typeof(AssignTemplateRoleActivityDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(SendTemplate), new DesignerAttribute(typeof(SendTemplateActivityDesigner)));

            MetadataStore.AddAttributeTable(attributeTableBuilder.CreateTable());
        }
    }
}