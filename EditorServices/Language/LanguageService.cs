using DocumentationService.documentation;
using Microsoft.LanguageServer.EditorServices;
using Microsoft.LanguageServer.EditorServices.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_language_server.EditorServices
{

    /// <summary>
    /// Provides a high-level service for performing code completion and
    /// navigation operations on Advpl program.
    /// </summary>
    class LanguageService
    {



        #region Public Methods

        /// <summary>
        /// Gets completions for a statement contained in the given
        /// script file at the specified line and column position.
        /// </summary>
        /// <param name="scriptFile">
        /// The script file in which completions will be gathered.
        /// </param>
        /// <param name="lineNumber">
        /// The 1-based line number at which completions will be gathered.
        /// </param>
        /// <param name="columnNumber">
        /// The 1-based column number at which completions will be gathered.
        /// </param>
        /// <returns>
        /// A CommandCompletion instance completions for the identified statement.
        /// </returns>
        public async Task<CompletionResults> GetCompletionsInFile(
            ScriptFile scriptFile,
            int lineNumber,
            int columnNumber)
        {
            Validate.IsNotNull("scriptFile", scriptFile);

            // Get the offset at the specified position.  This method
            // will also validate the given position.
            int fileOffset =
                scriptFile.GetOffsetAtPosition(
                    lineNumber,
                    columnNumber);
            
            CompletionResults completionResults =
                    CompletionResults.Create(
                        scriptFile, lineNumber, columnNumber);

            return completionResults;
        }
    }
    #endregion
}
