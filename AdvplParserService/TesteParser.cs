using advpl_language_server.AdvplParserService;

using advpl_parser.listener;
using advpl_parser.util;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_language_server.AdvplParserServiceService
{
    class TesteParser
    {
        public void Parse()//(Response response, dynamic args)
        {
            //string source = getString(args, "source");
            string source = "";
            NoCaseAntlrStringStream input = new NoCaseAntlrStringStream(source);
            AdvplLexer lexer = new AdvplLexer(input);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            AdvplParser advplParser = new AdvplParser(commonTokenStream);
            advplParser.RemoveErrorListeners();
            AdvplErrorListener errorListener = new AdvplErrorListener();
            advplParser.AddErrorListener(errorListener);
            ParserRuleContext tree = advplParser.program();

            //Cria a tabela de symbolo
            SymbolTableDefPhase tableSymbolList = new SymbolTableDefPhase();
            ParseTreeWalker walkerGeneral = new ParseTreeWalker();
            walkerGeneral.Walk(tableSymbolList, tree);


           // AdvplCompileInfo info = new AdvplCompileInfo();
            //info.Errors = errorListener.Errors;
            //string json = JsonConvert.SerializeObject(info);
            //SendResponse(response, info);
            //System.Console.WriteLine(json);
        }
    }
}
