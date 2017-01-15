using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_language_server.EditorServices.Workspace
{
    public interface IScriptPosition
    {
        int ColumnNumber { get; set; }
        string File { get; set; }
        string Line { get; set; }
        int LineNumber { get; set; }
        int Offset { get; set; }

        string GetFullScript();
    }
}
