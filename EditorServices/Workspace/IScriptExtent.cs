using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_language_server.EditorServices.Workspace
{
    public interface IScriptExtent
    {
        int EndColumnNumber { get; set; }
        int EndLineNumber { get; set; }
        int EndOffset { get; set; }
        IScriptPosition EndScriptPosition { get; set; }
        string File { get; }
        int StartColumnNumber { get; set; }
        int StartLineNumber { get; set; }
        int StartOffset { get; set; }
        IScriptPosition StartScriptPosition { get; set; }
        string Text { get; }
    }
}
