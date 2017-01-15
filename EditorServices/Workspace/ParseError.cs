using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_language_server.EditorServices.Workspace
{
    class ParseError
    {
        public string ErrorId { get; set; }
        public IScriptExtent Extent { get; set; }
        public bool IncompleteInput { get; set; }
        public string Message { get; set; }
    }
}
