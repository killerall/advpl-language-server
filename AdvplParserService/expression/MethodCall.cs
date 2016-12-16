using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static advpl_language_server.AdvplParserService.AdvplParser;

namespace advpl_parser.expression
{
    class MethodCall : FunctionCall
    {
        public string varName;
        public bool model;
        public bool view;
        public ArrayList loops;
        public bool constructor;
        public MethodCall(ObjectMethodAccessContext ctx):base()
        {
            Context = ctx;
            varName = ctx.identifier().GetText();
            name = ctx.methodAccessLoop(0).identifier().GetText();
        }
        public MethodCall(ClassConstructorContext ctx) :base()
        {
            
            Context = ctx;
            varName = ctx.identifier(0).GetText();
            name = ctx.identifier(1).GetText();
            constructor = true;
        }
    }
}
