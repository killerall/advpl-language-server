using advpl_language_server.AdvplParserService;
using advpl_parser.symbolTable;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static advpl_language_server.AdvplParserService.AdvplParser;

namespace advpl_parser.listener
{
    class SymbolTableDefPhase : AdvplBaseListener
    {
        private ParseTreeProperty<Scope> scopes = new ParseTreeProperty<Scope>();
        public GlobalScope globals;

        public Dictionary<string, string> m_definesInSource = new Dictionary<string, string>();
        public Dictionary<string, ClassSymbol> m_classInSource = new Dictionary<string, ClassSymbol>();
        //TODO ver aqui se o dicionary resolve. Nao tem parsetree no c# IParseTree
        public Dictionary<Scope, IParseTree> treesByScopes = new Dictionary<Scope, IParseTree>();

        Scope currentScope;
        public ArrayList allScopes = new ArrayList();
        private ArrayList m_functionsAndMethodsInSource = new ArrayList();

        /**
	    * Metodos generico
	    */
        /**
	 * Define uma variavel na tabela de simbolo
	 * @param type
	 * @param nameToken
	 */
        Symbol defineVar(Symbol.Type type, IToken nameToken, ParserRuleContext ctx)
        {
            String varName = nameToken.Text.ToUpper();
            if (!currentScope.existLocalScope(varName))
            {
                VariableSymbol var = new VariableSymbol(nameToken.Text, type);
                currentScope.define(var); // Define symbol in current scope
                var.setContext(ctx);
                return var;
            }
            else
            {
              /*  String msg = String.format(Messages.DefPhase_2, varName);
                Token ec = nameToken;
                int st = this.m_converter.convert(ec).getColumn();
                String sm = ec.getText();
                int et = st + ((sm != null) ? sm.length() : 1);
                if (st == -1)
                    return null;

                DefaultProblem defaultProblem = new DefaultProblem("", msg, 0,
                new String[] { }, ProblemSeverities.Error, st, et,
                ec.getLine());
                m_reporter.reportProblem(defaultProblem);*/

            }
            return null;
        }
        void defineStaticCh(Symbol.Type type, String varName, String value)
        {
            if (!currentScope.existLocalScope(varName))
            {
                VariableSymbol var = new VariableSymbol(varName, type);
                currentScope.define(var); // Define symbol in current scope	    
            }

        }
        void saveScope(ParserRuleContext ctx, Scope s)
        {
            scopes.Put(ctx, s);
        }
        public void genericExit()
        {
            currentScope = currentScope.getEnclosingScope();
        }
        public void genericEnterClass(string name, ParserRuleContext ctx)
        {
            //String name = ctx.identifier().getText();
            ClassSymbol classS = new ClassSymbol(name, currentScope, null);
            currentScope.define(classS);
            saveScope(ctx, classS);
            currentScope = classS;
            allScopes.Add(currentScope);
            treesByScopes.Add(currentScope, ctx);
            m_classInSource.Add(name.ToUpper(), classS);
        }
        public void genericEnterClass(String name, ParserRuleContext ctx, String superClass)
        {

            ClassSymbol classS = new ClassSymbol(name, currentScope, new ClassSymbol(superClass, null, null));
            currentScope.define(classS);
            saveScope(ctx, classS);
            currentScope = classS;
            allScopes.Add(currentScope);
            treesByScopes.Add(currentScope, ctx);
            m_classInSource.Add(name.ToUpper(), classS);
        }
        public void genericEnterMethod(String name, String cClassName, ParserRuleContext ctx)
        {
            Scope scopo_method = currentScope;
            ClassSymbol clss;
            m_classInSource.TryGetValue(cClassName, out clss);
            if (clss != null)
            {
                scopo_method = clss;
            }
            MethodSymbol method = new MethodSymbol(name, Symbol.Type.tMETHOD, scopo_method);
            method.startFuncionPos = ctx.start.StartIndex;
            method.endFuncionPos = ctx.Stop.StopIndex;
            m_functionsAndMethodsInSource.Add(method);
            scopo_method.define(method);
            saveScope(ctx, method);
            currentScope = method;
            allScopes.Add(currentScope);
            treesByScopes.Add(currentScope, ctx);
        }

        public override void EnterProgram([NotNull] AdvplParser.ProgramContext context)
        {
            globals = new GlobalScope(null, context);
            currentScope = globals;
            treesByScopes.Add(globals, context);
        }
        /**
         * Rest         
         */
        public override void EnterRestServiceDeclaration([NotNull] AdvplParser.RestServiceDeclarationContext context)
        {
            String name = context.identifier().GetText();
            genericEnterClass(name, context);
        }
        public override void ExitRestServiceDeclaration([NotNull] AdvplParser.RestServiceDeclarationContext context)
        {
            genericExit();
        }
        public override void EnterRestmethodBody([NotNull] AdvplParser.RestmethodBodyContext ctx)
        {
            string name = ctx.identifier(0).GetText();
            string cClassName = ctx.identifier(1).GetText().ToUpper();
            genericEnterMethod(name, cClassName, ctx);
        }
        public override void ExitRestmethodBody([NotNull] AdvplParser.RestmethodBodyContext ctx)
        {
            if (currentScope is MethodSymbol)
            {
                MethodSymbol method = (MethodSymbol)currentScope;
                if (ctx.identifier().Length == 2)
                {
                    String cClassName = ctx.identifier(1).GetText().ToUpper();
                    ClassSymbol clss;
                    m_classInSource.TryGetValue(cClassName, out clss);
                    if (clss != null)
                    {
                        method.setClassName(ctx.identifier(1).GetText());
                        //clss.addMember(ctx.identifier(0).getText(), method);
                    }
                }
            }

            genericExit();
        }
        /**
        * WebServices
        */
        public override void EnterWsServiceDeclaration([NotNull]AdvplParser.WsServiceDeclarationContext ctx)
        {
            String name = ctx.identifier().GetText();
            genericEnterClass(name, ctx);

        }

        public override void ExitWsServiceDeclaration([NotNull] AdvplParser.WsServiceDeclarationContext ctx)
        {
            genericExit();
        }
        public override void EnterWsmethodBody([NotNull] AdvplParser.WsmethodBodyContext ctx)
        {
            string name = ctx.identifier(0).GetText();
            string cClassName = ctx.identifier(1).GetText().ToUpper();
            genericEnterMethod(name, cClassName, ctx);
        }
        public override void ExitWsmethodBody([NotNull] AdvplParser.WsmethodBodyContext ctx)
        {
            if (currentScope is MethodSymbol)
            {
                MethodSymbol method = (MethodSymbol)currentScope;
                if (ctx.identifier().Length == 2)
                {
                    String cClassName = ctx.identifier(1).GetText().ToUpper();
                    ClassSymbol clss;
                    m_classInSource.TryGetValue(cClassName, out clss);
                    if (clss != null)
                    {
                        method.setClassName(ctx.identifier(1).GetText());
                        //clss.addMember(ctx.identifier(0).getText(), method);
                    }
                }
            }

            genericExit();
        }
        public override void EnterWsdataDefinition([NotNull] AdvplParser.WsdataDefinitionContext ctx)
        {
            Symbol syb = defineVar(Symbol.Type.tDATA, ctx.identifier().Start, ctx);
        }
        /**
	 * WebServices Cliente
	 */
        public override void EnterWsServiceClientDeclaration(AdvplParser.WsServiceClientDeclarationContext ctx)
        {
            String name = ctx.identifier().GetText();
            genericEnterClass(name, ctx);
        }

        public override void ExitWsServiceClientDeclaration(AdvplParser.WsServiceClientDeclarationContext ctx)
        {
            genericExit();
        }
        public override void EnterWsmethodClientBody(AdvplParser.WsmethodClientBodyContext ctx)
        {
            String name = ctx.identifier(0).GetText();
            String cClassName = ctx.identifier(1).GetText().ToUpper();
            genericEnterMethod(name, cClassName, ctx);
        }

        public override void ExitWsmethodClientBody(AdvplParser.WsmethodClientBodyContext ctx)
        {
            if (currentScope is MethodSymbol)
		    {
                    MethodSymbol method = (MethodSymbol)currentScope;
                    if (ctx.identifier().Length == 2)
                    {
                        String cClassName = ctx.identifier(1).GetText().ToUpper();
                        ClassSymbol clss;
                        m_classInSource.TryGetValue(cClassName,out clss);
                        if (clss != null)
                        {
                            method.setClassName(ctx.identifier(1).GetText());
                            //clss.addMember(ctx.identifier(0).getText(), method);
                        }
                    }
                }

                genericExit();
         }
     /**
	 * CLASSES
	 */

     
    public override void EnterClassDeclaration(AdvplParser.ClassDeclarationContext ctx)
        {
            String name = ctx.identifier().GetText();
            if (ctx.fromClass() != null)
                genericEnterClass(name, ctx, ctx.fromClass().identifier().GetText());
            else
                genericEnterClass(name, ctx);
        }

        public override void ExitClassDeclaration(AdvplParser.ClassDeclarationContext ctx)
        {

            genericExit();
        }

        public override void EnterDataDefinition(AdvplParser.DataDefinitionContext ctx)
        {
            Symbol syb = defineVar(Symbol.Type.tDATA, ctx.identifier().Start, ctx);           
        }
        public override void EnterMethodBody(AdvplParser.MethodBodyContext ctx)
        {
            String name = ctx.identifier(0).GetText();
            String cClassName = ctx.identifier(1).GetText().ToUpper();
            genericEnterMethod(name, cClassName, ctx);
        }

        public override void ExitMethodBody(AdvplParser.MethodBodyContext ctx)
        {
            if (currentScope is  MethodSymbol)
		    {
                MethodSymbol method = (MethodSymbol)currentScope;
                if (ctx.identifier().Length == 2)
                {
                    String cClassName = ctx.identifier(1).GetText().ToUpper();
                    ClassSymbol clss;
                    m_classInSource.TryGetValue(cClassName, out clss);
                    if (clss != null)
                    {
                        method.setClassName(ctx.identifier(1).GetText());
                        
                    }
                }
            }

            genericExit();
        }
        
        public override void EnterFuncDeclaration(AdvplParser.FuncDeclarationContext ctx)
        {
            String name = ctx.identifier().GetText();
            int typeTokenType = -1;
            if (ctx.modifiersFunction() != null)
                typeTokenType = ctx.modifiersFunction().start.Type;
            Symbol.Type type = CheckSymbols.getType(typeTokenType);

            FunctionSymbol function = new FunctionSymbol(name, type, currentScope);
            function.startFuncionPos = ctx.start.StartIndex;
            function.endFuncionPos = ctx.stop.StopIndex;
            m_functionsAndMethodsInSource.Add(function);
            currentScope.define(function);
            saveScope(ctx, function);
            currentScope = function;
            allScopes.Add(currentScope);
            treesByScopes.Add(currentScope, ctx);
        }
        public override void ExitFuncDeclaration(AdvplParser.FuncDeclarationContext ctx)
        {
            currentScope = currentScope.getEnclosingScope(); // pop scope

        }
        public override void EnterBlockInitializer(AdvplParser.BlockInitializerContext ctx)
        {
            currentScope = new LocalScope(currentScope);
            saveScope(ctx, currentScope);
            allScopes.Add(currentScope);
            treesByScopes.Add(currentScope, ctx);
        }
        public override void ExitBlockInitializer(AdvplParser.BlockInitializerContext ctx)
        {
            currentScope = currentScope.getEnclosingScope(); // pop scope
        }
        public override void ExitBlockParams(AdvplParser.BlockParamsContext ctx)
        {
            foreach(IdentifierContext var in ctx.identifier())
            {
                defineVar(Symbol.Type.tLOCAL, var.start, ctx);
            }
        }
        public override void EnterBlock(AdvplParser.BlockContext ctx)
        {
            // Cria um novo escopo
            currentScope = new LocalScope(currentScope);
            saveScope(ctx, currentScope);
            allScopes.Add(currentScope);
            treesByScopes.Add(currentScope, ctx);
        }
        public override void ExitBlock(AdvplParser.BlockContext ctx)
        {
            
            currentScope = currentScope.getEnclosingScope(); // pop scope
        }
        public override void ExitFormalParameter(AdvplParser.FormalParameterContext ctx)
        {
            defineVar(Symbol.Type.tARGUMENT, ctx.identifier().Start, ctx);
            if (currentScope is FunctionSymbol)
			((FunctionSymbol)currentScope).addParam(ctx.identifier().GetText());

        }

        public override void ExitLocalVariableDeclarationStatement(AdvplParser.LocalVariableDeclarationStatementContext ctx)
        {
            
            foreach (ExpressionContext var in ctx.expression())
            {   
                defineVar(Symbol.Type.tLOCAL, var.start, ctx);
            }

        }
        /**
	     * Defines
	     */
        public override void ExitDefineDeclaration(AdvplParser.DefineDeclarationContext ctx)
        {


            defineVar(Symbol.Type.tDEFINE, ctx.identifier().Start, ctx);
            m_definesInSource.Add(ctx.identifier().Start.Text.ToUpper(), ctx.expression().GetText());

        }
        public override void ExitPrivateVariableDeclarationStatement(PrivateVariableDeclarationStatementContext ctx)
        {
            foreach (ExpressionContext var in ctx.expression())
            {  
                defineVar(Symbol.Type.tPRIVATE, var.start, ctx);
            }
        }
        public override void ExitAliasAssignment(AdvplParser.AliasAssignmentContext ctx)
        {
            if (ctx.expression().Length > 0)
            {
                if (ctx.expression(0).children.Count > 0)
                {
                    IParseTree it = ctx.expression(0).children[0];

                    IdentifierContext lastCxt = null;
                    for (int nx = 0; nx < it.ChildCount; nx++)
                    {
                        IParseTree tree = it.GetChild(nx);
                        if (tree is IdentifierContext)
						lastCxt = (IdentifierContext)tree;
                }
                if (lastCxt != null)
                {
                    String varName = lastCxt.GetText().ToUpper();
                    if (!currentScope.existLocalScope(varName))
                        defineVar(Symbol.Type.tALIAS, lastCxt.Start, ctx);
                }
            }
        }
        
    }

        public override void ExitAssume(AdvplParser.AssumeContext ctx)
        {

            int qtdid = ctx.identifier().Length;

            if (qtdid == 3)
            {
                if (ctx.identifier(0).GetText().ToUpper()==("SELF"))
                {
                    String variavel = ctx.identifier(1).GetText();
                    Symbol simb = currentScope.resolve(variavel.ToUpper());
                    if (simb != null && (simb is VariableSymbol))
			{
                        String tipo = ctx.identifier(2).GetText().ToUpper();
                        VariableSymbol var = (VariableSymbol)simb;
                        var.AssumeType = (tipo);
                    }
                }
            }

        }




    }
}
