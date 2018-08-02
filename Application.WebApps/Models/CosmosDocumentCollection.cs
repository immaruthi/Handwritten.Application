using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApps.Models
{
    public class CosmosDocumentCollection
    {
        public ExtractHandwrittenText extracthandwrittenText { get; set; }

        public Text text { get; set; }

        public TextLocation textLocation { get; set; }

        public TextDocumentType textDocumentType { get; set; }

        public DocUserDetails docUserDetails { get; set; }
    }
}