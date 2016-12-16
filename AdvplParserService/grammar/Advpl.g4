/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

grammar Advpl;
options 
{
language = Java;
}
program
	:	preprocessorDeclaration*  
	(sources
	)* EOF?
	
	;

preprocessorDeclaration
	:	includeDeclaration|crlf|staticVariable;

includeDeclaration
	: INCLUDE (STRINGSIMPLE | STRING) 
	;	
/*ifdef:
        (IFDEF | IFNDEF) expression crlf
        block
        (ELSEDEF crlf block) *
        ENDIFDEF   
;
        
 */       
ifdef:
        ((IFDEF | IFNDEF) expression)
        | ELSEDEF | ENDIFDEF   ;
defineDeclaration
	:	DEFINE identifier expression crlf ;


sources 
	:
	 classDeclaration| methodBody|funcDeclaration|wsServiceDeclaration|restmethodBody| wsmethodBody|staticVariable |wsServiceClientDeclaration|wsmethodClientBody |(crlf)|defineDeclaration|restServiceDeclaration|(ifdef crlf);


modifiersFunction  
    :
	USER
    |   STATIC
    |   MAIN
    |   PROJECT
    ;
//-----------------------------------------------------------
// variaveis statics
//-----------------------------------------------------------
staticVariable
	:	STATIC localVariableDeclarationStatement 
	;
//-----------------------------------------------------------
// Definão da classe
//-----------------------------------------------------------
classDeclaration:
                  CLASS identifier CAMELCASE? fromClass?  crlf
                  (dataDefinition|serializabledataDefinition)*
                  methodDefinition*
                  (ENDCLASS|(END CLASS) )  (crlf|EOF)
                ;
fromClass:
             FROM identifier;
dataDefinition:
                 DATA identifier (AS wsDataType)? crlf;
serializabledataDefinition:
        CAMELCASE DATA identifier (AS wsDataType )? crlf;
methodDefinition:
                 METHOD identifier arguments 'CONSTRUCTOR'? crlf;
methodBody:
                 METHOD identifier (LPAREN formalParameters? RPAREN)? CLASS identifier crlf
                 	initFuncOrMethod?   block 	
;
//-----------------------------------------------------------
// Definão de REST
//-----------------------------------------------------------

restServiceDeclaration:
                  WSRESTFUL identifier DESCRIPTION expression (FORMAT literal)? crlf
                  wsdataDefinition*
                  restmethodDefinition*
                  END WSRESTFUL crlf
                ;

restmethodDefinition :                        
                 WSMETHOD ('GET'|'PUT'|'POST'|'DELETE') identifier?  (DESCRIPTION expression )?
                 (WSSYNTAX literal) ?
                 (('PATH') expression)?
                 (PRODUCES identifier)?
                 (REQUEST literal)?
                 (RESPONSE identifier)?
                 crlf;
restmethodBody:
                 WSMETHOD ('GET'|'PUT'|'POST'|'DELETE') identifier 
                 (PATHPARAM expressionList )?
                 (QUERYPARAM expressionList)?
                 (WSREST|WSRESTFUL) identifier crlf
                 
                 	initFuncOrMethod?   block 	
            ;

//-----------------------------------------------------------
// Definão de WebService
//-----------------------------------------------------------

wsServiceDeclaration:
                  WSSERVICE identifier DESCRIPTION literal NAMESPACE literal crlf
                  wsdataDefinition*
                  wsmethodDefinition*
                  ENDWSSERVICE crlf
                ;

wsdataDefinition:
                 WSDATA identifier AS wsDataType OPTIONAL? crlf;
wsmethodDefinition:
                 WSMETHOD identifier arguments? (DESCRIPTION literal)? crlf;
wsmethodBody:
                 WSMETHOD identifier wsReceive? wsSend ? WSSERVICE identifier crlf
                 
                 	initFuncOrMethod?   block 	
            ;
wsReceive: WSRECEIVE formalParameters;
wsSend: WSSEND formalParameters ;
wsDataType: identifier (OF identifier)?;
//-----------------------------------------------------------
// Definão de WebServiceCliente
//-----------------------------------------------------------

wsServiceClientDeclaration:
                  WSCLIENT identifier crlf
                  (wsdataDefinition|wsmethodClientDefinition)*
                  ENDWSCLIENT crlf
                ;
wsmethodClientDefinition:
                WSMETHOD identifier crlf;

wsmethodClientBody:
                 WSMETHOD identifier  wsSend ? wsReceive? WSCLIENT identifier crlf
                 
                 	initFuncOrMethod?  block 
            ;

endWSMethod: END WSMETHOD;
//-----------------------------------------------------------
// Definão da Funcao
//-----------------------------------------------------------

funcDeclaration 
	: modifiersFunction?  FUNCTION identifier (LPAREN formalParameters? RPAREN)? (crlf|EOF)
		initFuncOrMethod?   block 	
	;
formalParameters
    :   formalParameter (',' formalParameter)*
    ;
formalParameter:
                   identifier;
               
initFuncOrMethod 
	:	((LOCAL localVariableDeclarationStatement crlf) 
                  |(ifdef crlf ) 
                  |(staticVariableBeforeLocal crlf) )+
			;
staticVariableBeforeLocal:
				staticVariable;			
localVariableDeclarationStatement 
    :   expression
        (COMMA expression
        )*
    ;
publicVariableDeclarationStatement:
    PUBLIC expression
        (COMMA expression
        )* ;

privateVariableDeclarationStatement:
    PRIVATE expression
        (COMMA expression
        )* ;
defaultStatement:
    DEFAULT expression
                ;
/*variableDeclarator
	:	identifier (':=' variableInitializer)?
        
	;
variableInitializer
	:	expression;*/
arrayOrBlockInitializer
    :   '{' expressionList '}'   #arrayInitializer
    |   '{' '|' blockParams? '|' expressionList'}'  #blockInitializer
    ;
blockParams
	:	identifier (COMMA identifier )*
	;

block	:	
	(statement (crlf|EOF)
	|crlf)+	
	;
	

statement 
	: 	statementExpression
	|	ifStatement
	|	forStatement
        |       doStatement
        |       whileStatement
        |       docaseStatement
	|	returnStatement         
        |       privateVariableDeclarationStatement        
        |       exitOrLoopStatement
        |       defaultStatement   
        |       publicVariableDeclarationStatement        
        |       staticVariable
        |       ifdef
        |       chStatement         
	;
returnStatement
	:	RETURN returnvalues?
	;	
returnvalues
	:	expression ;
		
statementExpression
	:expression ;	
//Removido o Assigment pois, se deixamos o return ser um expression, e podemos começar com expression como commando direto 
//a gramatica na fica mais LL(*) e precisamos ativar o backtracer, que onera a performace.	
expression
	:  primary    #ExprPrimary
    |    expression (op=PLUSPLUS | op=MINUSMINUS) #ExprIncrPos
    |   (op=PLUS|op=MINUS|op=PLUSPLUS|op=MINUSMINUS) expression #ExprIncrPre
    |   '!' expression #ExprNot
    |   expression (op=MULT|op=DIV|op=PERC) expression #ExprMul
    |   expression (op=PLUS|op=MINUS) expression #ExprPlus    
    |   expression op=AND expression #ExprLogical
    |   expression op=OR expression #ExprLogical
    |   expression (op=MINOREQUALS | op=MAJOREQUALS | op=MINOR | op=MAJOR| op=EQUALS|op=DOUBLEEQUAL|op=DIF1|op=DIF2|op=DIF3|op=CONTIDO) expression #ExprComp
    |   expression ALIASACCESS expression #AliasAssignment
    |   expression    
         (PLUSEQUALS 
        |'-='
        |'*='
        |'/='
        |':='
        |'^'
         )    
        expression # Assignment
    
    /*
    | expression arrayAccess   #VarArrayAccess
    | expression arguments  #Call
    | expression ':' identifier #ObjectAttribAccess
    | expression ':' identifier arguments#ObjectMethodAccess*/
    
    ;
primary
	: '(' expressionList ')'        #Parens
        | ARROBA? identifier arrayAccess        #VarArrayAccess
        | identifier arguments  (arrayAccess?)  #Call
        | identifier arguments   ':' identifier  #CallWithAtt        
        | ARROBA? identifier arrayAccess? ( ':' identifier arrayAccess?)+     #ObjectAttribAccess
        | identifier arrayAccess* methodAccessLoop+  #ObjectMethodAccess
        | identifier LPAREN RPAREN ':' identifier arguments  #ClassConstructor        
        | ARROBA? identifier        #Var                
        | ARROBA ASSUME identifier (':' identifier)* AS identifier     #Assume
	| literal                   #lit
	| arrayOrBlockInitializer   #ArrayOrBlock
	| ifFunctioncall            #IfCall
        | ECOMERCIAL expression   ('.' expression)? methodAccessLoop?   #MacroExecucao
        
    ;
//| ARROBA identifier         #VarByRef
//| identifier  arrayAccess ':' (identifier)?  #VarArrayAccess
/*
identifier:
          ':'|'ARRAY'|'SERVER'| 'ALIAS'|'MSDIALOG'|'DISABLE'|'FWMBROWSE'|'FWFORMBROWSE'|'CREATE'
          |'FWBROWSE' |'FWMBROWSE'|'FWFORMBROWSE'|'FWSEEK'|'BOLD'|'ADJUST'|'DATA'|'DEFAULT'|'SCROLLBOX'
          |'SAY'|'TITLE'|'FINISH'|'METER'|'COLOR'|'TOTAL'|'STATUS'|'TEXT'|'READONLY'|'RADIO'|'BITMAP'
          |'VALID'|'DATA'|'RIGHT'|'DOUBLECLICK'|'FILTER'|'PARAM'
          |'PASSWORD'|'OR'|'OBJECT'|'RESET'|'ORDER'|'INDEX'|'SET' |'CENTURY' |'ON'| 'OFF'| 'BACK'
          |'ERROR'|'MODULO'|'FIELDSIZES'|'PANEL'|'EXEC'|'VAR'|'ONSTOP'|'RESNAME'|'PROMPT'|'GROUP'
          |'CONNECT'| 'SMTP' |'ACCOUNT' |'RESULT' |'SSL'|'TLS'|'SEND'|'MAIL'| 'DISCONNECT'|'ITEMS'
    |'ADD'|'INIT'|'NEW'|'ENABLE'|'PICTURE'|'GET'|SELF| 'BEGIN'|'DATE'| 'END' |'SEEK'|'DEFAULT'
    |'TYPE'| 'TABLE'|'NAME'|'ACTIVATE'|'AT'|IDENTIFIER;
*/
chIdentifier:
          TO|SELF|END|DEFAULT|CLASS|IDENTIFIER|DATA|FROM|OPTIONAL;

identifier:
          DEFAULT|PROJECT|WSMETHOD|OF|ASSUME|DESCRIPTION|AS|TO|NEXT|END|DATA|'SELF'|':'|  IDENTIFIER
          ;
arrayAccess
    :  ( '[' expressionList ']' )+
    ;
methodAccessLoop:
                    (':' identifier arguments? arrayAccess? )
                    
                ;
/*functionCall
	:	arguments;	
*/
arguments
    :   LPAREN expressionList RPAREN 
    ;

expressionList
    : optionalExpression (COMMA optionalExpression)*
  ;

optionalExpression
  : expression?
  ;
expressionListComa:
                      COMMA;
literal	
	:  NUMBER #LiteralNumber
	|  STRING #LiteralStringDupla
	|  STRINGSIMPLE  #LiteralStringSimples
	|  TRUE  #LiteralLogical
	|  FALSE #LiteralLogical
        |  NIL #LiteralNil
    ;

ifFunctioncall
	:	IF LPAREN expression COMMA expression? COMMA expression? RPAREN //-> ^(IFSTATMENT expression+ )
	;        
//---------------------------------------------------------
// STATEMENTs    
//---------------------------------------------------------
ifStatement 
	: IF expression  crlf 
		block
	( ELSEIF expression crlf block   )*
	(ELSE crlf  
	 block)*
	(ENDIF|END| END IF)
	;    
    
forStatement
	:	
		FOR forInit TO expression (STEP expression)? crlf
		     block?
		(NEXT expression? ) 
		;

doStatement
	:	DO (whileStatement |docaseStatement) ;
exitOrLoopStatement:
    EXIT|LOOP;
whileStatement
	: WHILE expression  crlf
            block? (ENDDO|END |'ENDD') crlf? 
	
	;
docaseStatement 
	: CASE crlf (  CASE expression crlf block?)+ (OTHERWISE block? )? ('ENDC'|ENDCASE|(END CASE? ) ) crlf? 
		;
                    
//-----------------------------------------------    
//Instruções para ler o CH do protheus
//-----------------------------------------------    
chStatement:
               (chIdentifier | arrobaDefine               )
                    (chIdentifier|(expression (COMMA expression)*) )+
           ;
arrobaDefine
    :   ARROBA expressionList
    ;

/*
oldchStatement:
               addOptionCH
           | paramType
           | dialogDefine
           | formBrowseDefine
           | activateDefine
           | addColumnDefine
           | buttonDefine
           | sayDefine
           | beginTrans
           | endTrans
           | getDefine
           | sbuttonDefine
           | comboBoxDefine
           | toDefine
           | radioDefine 
           | checkBoxDefine
           | menuDefine
           | menuItemDefine
           | menuEndDefine
           | fontDefine
           | tcQuery
           | wizardDef
           | createPanelDef
           | prepareIn             
           | resetEnv
           | listBoxDef
           | meterDefine
           | classExceptionDefine
           | bitmapDefine
           | msgraphicsDefine
           | mspanelDefine
           | indexDefine
           | setsDefine
           | connectSTMPDefine
           | sendMailDefine
           | getmailDefine
           | disconnectDefine
           | scrollBoxDefine
           | setFilterDefine
           | paramException
                ;
addOptionCH:
               ADD OPTION primary TITLE primary ACTION primary OPERATION NUMBER ACCESS NUMBER ('DISABLE' 'MENU')?;
prepareIn:
    'PREPARE' 'ENVIRONMENT' ('EMPRESA' expression
                            |'FILIAL' expression
                            | 'MODULO' expression  )+;
resetEnv:
    'RESET' 'ENVIRONMENT';
paramType:
             'PARAMTYPE' (NUMBER'VAR')? 
              identifier 'AS'  listTypes (',' listTypes)*
              ('OR' 'OBJECT' 'CLASS' expressionList)?
             ('MESSAGE' expression)? 
             'OPTIONAL'?
             (DEFAULT expression )? ;
paramException :
                'PARAMEXCEPTION' ('PARAM' NUMBER'VAR')?  identifier 'TEXT' expression ('MESSAGE' expression)? 
                
;
classExceptionDefine:
                'CLASSEXCEPTION' expression 'MESSAGE' expression                
                        
                    ;
listTypes:
                  ('ARRAY'|'BLOCK' |'CHARACTER'|'DATE' |'NUMERIC'|'LOGICAL'|('OBJECT' ('CLASS' expressionList)?));

dialogDefine:
             'DEFINE' ('DIALOG'|'MSDIALOG'|'WINDOW') identifier (
                                                        ('TITLE' expression)
                                                        | ('FROM' expressionList)
                                                        | ('TO' expressionList )
                                                        | 'PIXEL'
                                                        |('STYLE' expression)
                                                        | ofDefine
                                                        | 'STATUS'
                                                       )+
            ;

listBoxDef: arrobaDefine 'LISTBOX' expression ('FIELDS'expression?                                               
                                   | 'HEADER' expressionList
                                   | sizeDefine
                                   | ofDefine
                                   | varDefine
                                   | onDblClickDefine
                                   | validDef
                                   | whenDefine
                                   | 'FIELDSIZES'expressionList
                                   | 'ITEMS' expression
                                   | 'PIXEL')+

          ;
formBrowseDefine:
                    'DEFINE' ('FWBROWSE' |'FWMBROWSE'|'FWFORMBROWSE') expression 
                    ('DATA' ('QUERY'|'ARRAY'|'TABLE') 
                    |'ALIAS' expression ('QUERY' expression)? 
                    |'ARRAY' expression                    
                    |'SEEK' 'ORDER'? expression
                    |'NO REPORT'
                    |'DOUBLECLICK' expression
                    |'NO LOCATE'
                    |'OF' expression)+;
activateDefine:
                'ACTIVATE' ('FWBROWSE'|'FWMBROWSE'|'FWFORMBROWSE'|'DIALOG'|'MSDIALOG'|'MENU'|'WIZARD'|'WINDOW') expression (
                                                                                'CENTERED'
                                                                              |'CENTER'
                                                                              | 'AT' expressionList
                                                                              | 'ON' 'INIT' expression
                                                                              | validDef
                                                                              )*
                                
              ;
                            
                            
addColumnDefine:
                   'ADD' 'COLUMN' expression 'DATA' expression 'TITLE' expression 'SIZE' expression
                   (pictureDefine)? ofDefine
;
pictureDefine:
    'PICTURE' expression;
onDblClickDefine:
                   'ON' 'DBLCLICK'  expression;

onChangeDefine:
                   'ON' 'CHANGE'  expression;
varDefine:
    ('VAR' expression)                   ;
sayDefine : 
    arrobaDefine 'SAY' expression (
                                    promptDefine
                                  |  sizeDefine
                                  |  'PIXEL'
                                  | ofDefine
                                  | colorDefine
                                  | 'FONT' expression
                                  | varDefine
                                  | 'CENTER'
                                  | 'RIGHT'
                                  | 'HTML'
                                  )+
          ;
colorDefine:
            ('COLOR'| 'COLORS')expressionList;

            
getDefine : 
    arrobaDefine ('GET'|'MSGET') expression ('VAR' expression)? 
                (pictureDefine
                |'MEMO' 
                |sizeDefine
                |validDef
                |('F3' expression)                
                |'PIXEL'
                |ofDefine
                | whenDefine
                | 'FONT' expression
                | 'READONLY'
                | 'PASSWORD'
                |'HASBUTTON')+
          ;
validDef:
    ('VALID' expression);
checkBoxDefine : 
    arrobaDefine ('CHECKBOX') expression 
                    ('VAR' expression
                    |promptDefine 
                    |'VALID' expression
                    | sizeDefine
                    | 'PIXEL'
                    | whenDefine
                    | onChangeDefine
                    | ofDefine)+

               ;
toDefine:
    arrobaDefine ('GROUP' expression)? 'TO' expressionList ('LABEL' expression 
                                     |'PIXEL'
                                     | 'PROMPT' expression
                                     |ofDefine)+;

setFilterDefine:
                'SET' 'FILTER' 'TO' expression?
               ;

scrollBoxDefine: arrobaDefine 'SCROLLBOX' expression (sizeDefine
                                                     | ofDefine
                                                     | 'VERTICAL'
                                                     | 'HORIZONTAL')+
               
               ;

comboBoxDefine : 
    arrobaDefine 'COMBOBOX' expression (
                                        ('ITEMS' expression)
                                        | sizeDefine
                                        | varDefine
                                        | 'PIXEL' 
                                        | ofDefine
                                        | onChangeDefine
                                        | whenDefine
                                       )+;
radioDefine : 
    arrobaDefine 'RADIO' expression (
                        'VAR' expression
                        | 'ITEMS'expressionList
                        | '3D'
                        | sizeDefine
                        |'VALID' expression
                        |'PIXEL'
                        | onChangeDefine                        
                        | whenDefine
                        | ofDefine)+
            ;
msgraphicsDefine: 
                    arrobaDefine 'MSGRAPHIC' expression sizeDefine ofDefine 'FONT' expression;

mspanelDefine:
                    arrobaDefine 'MSPANEL' expression (sizeDefine 
                                                      | colorDefine
                                                      | ofDefine)+;
                    
bitmapDefine: 
                arrobaDefine ('BITMAP'|'BTNBMP' ) ('NAME') ?expression ( 'RESOURCE' expression
                                               | 'NOBORDER'
                                               | 'PIXEL'
                                               | sizeDefine
                                               | ofDefine
                                               | 'ADJUST'                                               
                                               | 'RESNAME' expression
                                               | 'ACTION' expressionList
                           )+;

promptDefine :'PROMPT' expression ;

buttonDefine:  arrobaDefine ('BUTTON' | 'SBUTTON') expression?
               ( promptDefine
               | sizeDefine 
               | actionDefine
               |'PIXEL'
               |ofDefine
               | 'MESSAGE' expression                 
               | whenDefine)+
;
meterDefine:
               arrobaDefine 'METER' expression 
               ( varDefine
               | sizeDefine 
               | ofDefine
               | 'PIXEL'
               | 'NOPERCENTAGE'
               | 'TOTAL' expression)+
               

; 
sbuttonDefine: 'DEFINE' 'SBUTTON' expression? 'FROM' expressionList 'TYPE' expression 
               ('ENABLE'
               |ofDefine
               |'PIXEL'
               | whenDefine
               | 'ONSTOP' expression
               |actionDefine)+
               ;

indexDefine: 
        'INDEX' 'ON' expression 'TAG' expression 'TO' expression;
fontDefine: 'DEFINE' 'FONT' expression 'NAME' expression sizeDefine 'BOLD'? ;
whenDefine:
              'WHEN' expression;
ofDefine:
            'OF' expression
        ;
actionDefine:
    'ACTION' LPAREN? expressionList RPAREN?;

menuDefine: 'MENU' expression 'POPUP' ofDefine
          ;
menuItemDefine: 'MENUITEM' expression 'ACTION' expression ofDefine                
;
menuEndDefine : 'ENDMENU';

tcQuery:
    'TCQUERY' expression ( ('ALIAS' expression) 
                          | 'NEW'
                          | ('SERVER' expression)
                          | ('ENVIONMENT' expression)
                         ) +;
                          
sizeDefine :
               'SIZE' expressionList;
beginTrans:
              'BEGIN' ('TRANSACTION'|'SEQUENCE');
endTrans:
              'END' ('TRANSACTION'|'SEQUENCE');
setsDefine:
               'SET' 'CENTURY' ('ON'| 'OFF');
wizardDef:
               'DEFINE' 'WIZARD' expression (
                                                'HEADER' expression
                                            |   'TEXT' expression
                                            |   'NEXT' expression
                                            | 'TITLE' expression
                                            | 'MESSAGE' expression
                                            | 'FINISH' expression
                                            | 'PANEL'
                                            )+
;
connectSTMPDefine:
                      'CONNECT' 'SMTP' 
                                    ('SERVER' expression 
                                    |'ACCOUNT' expression
                                    |'PASSWORD'expression
                                    |'RESULT' expression
                                    |'SSL'
                                    |'TLS'
                                    )+                      
                 ;
sendMailDefine: 'SEND' 'MAIL' ( 'FROM' expression
                              |'TO' expression
                              | 'SUBJECT' expression
                              | 'BODY' expression
                              | 'RESULT' expression
                              | 'SSL'
                              | 'TLS'
                              )+
              
;
getmailDefine: 'GET' 'MAIL' 'ERROR' expression
                            
             ;
disconnectDefine :
            'DISCONNECT' 'SMTP' 'SERVER';
createPanelDef:
                   'CREATE' 'PANEL' expression
                                            (
                                                'HEADER' expression
                                            |   'MESSAGE' expression
                                            |   'NEXT' expression
                                            |   'PANEL'
                                            |   'FINISH' expression                                                
                                            |   'BACK' expression
                                            |   'EXEC' expression
                                            )+
;

ADD         :    'ADD';
OPTION      :    'OPTION';
TITLE       :    'TITLE';
ACTION      :    'ACTION';
OPERATION   :   'OPERATION';
ACCESS      :   'ACCESS';
*/
forInit	:identifier
        (ATTRIB_OPERATOR | EQUALS) expression 
	;
//-----------------------------------------------    
// tokens
//-----------------------------------------------
MINOR 	: '<';
MAJOR	: '>';
EQUALS	: '=';
MINOREQUALS 	: '<=';
MAJOREQUALS		: '>=';
DOUBLEEQUAL : '==';
DIF1	: '!=';
DIF2	: '<>';
DIF3	: '#';
CONTIDO : '$';
PLUSPLUS            : '++';
MINUSMINUS           : '--';
PLUSEQUALS 			: '+=';    
PLUS            : '+';
MINUS           : '-';    
MULT            : '*';
DIV             : '/';
PERC            : '%';
ARROBA          : '@';
ECOMERCIAL      : '&';
    
BEGIN_SQL       : 'BEGINSQL' .*? 'ENDSQL'  -> channel(HIDDEN);
ALIASACCESS     :       '->';
STEP		:	'STEP';    
TO		:	'TO';    
TRUE		:	'.T.';
FALSE		:	'.F.';	
NIL             :       'NIL';
DEFINE		:	'#DEFINE';
IFDEF           :       '#IFDEF';
IFNDEF          :       '#IFNDEF';

ELSEDEF         :       '#ELSE';
ENDIFDEF        :       '#ENDIF';

FOR		:	'FOR';
NEXT		:	'NEXT';
WHILE		:	'WHILE';
DO              :       'DO';
ELSEIF		:	'ELSEIF';
IF		:	'IF';
ELSE		:	'ELSE';
ENDIF		:	'ENDIF';
//END_IF          :       'END IF';
ENDDO           :       'ENDDO';
END		:	'END';
CASE		:	'CASE';
ENDCASE		:	'ENDCASE';
OTHERWISE	:	'OTHERWISE';
EXIT		:	'EXIT';
LOOP		:	'LOOP';
LOCAL		:	'LOCAL';
PRIVATE		:	'PRIVATE';
PUBLIC		:	'PUBLIC';
STATIC		:	'STATIC';
USER		:	'USER';
MAIN		:	'MAIN';
FUNCTION	:	'FUNCTION';
SELF		:	'SELF';
PROJECT		:   'PROJECT';

AND		:	'.AND.';
OR		:	'.OR.';

DEFAULT		:	'DEFAULT';

RETURN		:	'RETURN';
ASSUME          :       'ASSUME';
CLASS           :       'CLASS';
ENDCLASS        :       'ENDCLASS';
METHOD          :       'METHOD';
DATA            :       'DATA';
FROM            :       'FROM';

WSCLIENT        :       'WSCLIENT';
WSSERVICE       :       'WSSERVICE';
NAMESPACE       :       'NAMESPACE';
ENDWSCLIENT     :       'ENDWSCLIENT';
ENDWSSERVICE    :       'ENDWSSERVICE';
WSRESTFUL       :       'WSRESTFUL';
FORMAT          :       'FORMAT';
WSMETHOD        :       'WSMETHOD';
WSDATA          :       'WSDATA';
WSRECEIVE       :       'WSRECEIVE';
WSSEND          :       'WSSEND';
DESCRIPTION     :       'DESCRIPTION';
AS              :       'AS';
OF              :       'OF';
PRODUCES        :       'PRODUCES';
OPTIONAL        :       'OPTIONAL';
WSSYNTAX        :       'WSSYNTAX';
RESPONSE        :       'RESPONSE';
REQUEST         :       'REQUEST';
QUERYPARAM      :       'QUERYPARAM';
WSREST          :       'WSREST';
CAMELCASE       :       'CAMELCASE';
PATHPARAM       :       'PATHPARAM';
//BEGIN           :       'BEGIN';
LPAREN	: '(' ;

RPAREN	: ')'  ;

LBRACK	: '['  ;

RBRACK	: ']'  ;
INCLUDE	:	'#''INCLUDE';

COMMA		:	',';
ATTRIB_OPERATOR
	:	':=';

NUMBER
    :   ('0'..'9')+ ('.'  ('0'..'9')+ )?
    |   '.' ('0'..'9')+ 
    ;
   
fragment
DIGITS : ( '0' .. '9' )+ ;

    
IDENTIFIER	:	( 'a' .. 'z' | 'A' .. 'Z' | '_')
        ( 'a' .. 'z' | 'A' .. 'Z' | '_' | '0' .. '9' )* ;
    

COMMENT
    :   '/*' .*? '*/'  -> channel(5)
    ;
LINE_COMMENT:   '//' ~('\n'|'\r')*  -> channel(HIDDEN);
   
STRING
    :  '"' ( ~('"'|'\n') )* '"'?
    ;
STRINGSIMPLE
    :  '\'' (  ~('\''|'\n') )* '\''
    ;
//------------------------------------------------------------
//Em davpl o CRLF ajuda a determina o fim dos expression

crlf:
        (CRLF+|';');
CRLF
  : ((('\r')? '\n' ))  
  ;

WS  :   ( ' ' | '\t')+  -> skip;

CRLF_ESCAPED
  : (';' ( ' ' | '\t')*(  ('//') ~('\n'|'\r')*   )? ('\r')?'\n' )-> channel(HIDDEN);
/*
CRLF_ESCAPED
  : (DOT_COMA  ('\r')?'\n')-> skip;
*/