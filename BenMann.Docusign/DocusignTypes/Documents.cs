using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docusign.DocusignTypes
{
    public class Document
    {
        public int documentId;
        public string name;
        public string documentBase64;

        [JsonIgnore]
        private static int documentIndex = 1;

        [JsonIgnore]
        public string filename;

        public Document(string name, string filename, int index = -1)
        {
            if (index > 0)
            {
                documentIndex = index + 1;
                this.documentId = index;
            }
            else
            {
                this.documentId = documentIndex;
                documentIndex++;
            }
            this.name = name;
            this.filename = filename;
            Byte[] bytes = File.ReadAllBytes(filename);
            this.documentBase64 = Convert.ToBase64String(bytes);
        }
    }

    public class DocumentResponse
    {
        public List<DocumentInfo> envelopeDocuments;
    }

    public class DocumentInfo
    {
        public DocumentInfo()
        {

        }
        public string documentId;
        public string name;
        public string uri;
    }

    public class DocumentInfoList : IEnumerable<DocumentInfo>
    {
        List<DocumentInfo> documents = new List<DocumentInfo>();
        public DocumentInfo this[int index]
        {
            get { return documents[index]; }
            set { documents.Insert(index, value); }
        }

        public void Add(DocumentInfo envelopeInfo)
        {
            documents.Add(envelopeInfo);
        }

        public IEnumerator<DocumentInfo> GetEnumerator()
        {
            return documents.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
