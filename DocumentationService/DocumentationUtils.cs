using advpl_language_server.EditorServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationService.documentation
{
    class DocumentationUtils
    {
        public static int TYPE_FUNCTION = 1;
        public static int TYPE_CLASS = 2;

        private static DocumentationUtils instance;
        private ArrayList allFunctions;
        static Dictionary<string, FunctionDocumentation> cache = new Dictionary<string, FunctionDocumentation>();
        private ClassDocumentation classDocumentation;
        private int typeComplete = 0;
        private String nameClassActive = "";
        SQLiteConnection m_dbConnection;
        SQLiteCommand m_argCommand;
        private DocumentationUtils()
        {
            m_dbConnection = new SQLiteConnection("Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "/db/SYSTEM.db;Version=3;");
            m_dbConnection.Open();
            m_argCommand = new SQLiteCommand(null, m_dbConnection);
            m_argCommand.CommandText = "SELECT ARG_NAME, ARG_OPTIONAL FROM ARGUMENTS WHERE ARG_FUNC = @id ORDER BY ARG_SEQ";
          
        }

        public  static DocumentationUtils getInstance()
        {
            if (instance == null)
            {
                instance = new DocumentationUtils();
                cache = new Dictionary<string, FunctionDocumentation>();
            }
            return instance;
        }
        public string resolveOwner(string owner)
        {
            if(owner.Equals("T"))
             return "function Advpl Basic";
            if (owner.Equals("F"))
                return "function FrameWork";
            return "unknow";
        }

            
        public Collection<CompletionResult> getFunctionInDb()
        {
            //string sql = "select DOC_NAME,cast(DOC_CONTENT as TEXT) from DOC_FUNC";
            string sql = "SELECT DOC_ID,DOC_NAME, DOC_DESCRI, DOC_TYPERET, DOC_DESCRET, cast(DOC_SAMPLE AS TEXT), cast(DOC_OBS AS TEXT), DOC_OWNER,cast(DOC_DETAIL AS TEXT) FROM DOCS";
            Collection<CompletionResult> functions = new Collection<CompletionResult>();
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
                        //DataTable dt = new DataTable();
                       //  dt.Load(reader);

             /* functions = dt.AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
                                  row => row.Field<string>(1));*/
            //string doc = "Adiciona um elemento a um vetor.\n\naAdd() é usado para aumentar um vetor dinamicamente.Note: Se o elemento inserido for um vetor,o novo elemento no vetor de destino ira conter uma referencia ao outro vetor.";
            while (reader.Read())
             {

                string name = reader.GetString(1).TrimEnd();
                string doc = reader.GetString(2);
                doc += "\n\n" +reader.GetString(8);
                doc = doc.Replace("<br>", System.Environment.NewLine);
                doc.TrimEnd();
                CompletionResult func = new CompletionResult(name, name+ getArgInfo(reader.GetString(0)), CompletionResultType.Command, doc,resolveOwner(reader.GetString(7).TrimEnd()));
                func._id = reader.GetString(0);
                //func.ToolTip = reader.GetString(2);
                functions.Add(func);
               // break;
                /* FunctionDocumentation func = new FunctionDocumentation();
                    func.Name = reader.GetString(0);
                    func.Content= reader.GetString(1);
                functions.Add(func);*/
                //functions.Add(reader.GetString(0), reader.GetBlob(1,true));
             }
            reader.Close();
            //Console.WriteLine("Name: " + reader["doc_name"] + "\tScore: " + reader["doc_content"]);


            return functions;
        }
        

        
            
        public string getArgInfo(string functionId)
        {
             //( "@id", DbType.String, "");
            string cArgs = "(";
            SQLiteParameter idParam = new SQLiteParameter("@id", functionId);
            m_argCommand.Parameters.Add(idParam);
            m_argCommand.Prepare();            
            SQLiteDataReader reader = m_argCommand.ExecuteReader();
            bool optional;
            while (reader.Read())
            {
                optional = reader.GetString(1).Equals("T");
                if (optional)
                    cArgs += "[";
               cArgs += reader.GetString(0).Trim() ;
                if (optional)
                    cArgs += "]";
                cArgs += ",";
            }
            if(cArgs.Substring(cArgs.Length - 1).Equals(","))
                cArgs = cArgs.Remove(cArgs.Length - 1);
            reader.Close();
            cArgs += ")";
            return cArgs;
        }
    }
}
