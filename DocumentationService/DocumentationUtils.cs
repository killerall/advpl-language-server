using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.documentation
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
        private DocumentationUtils()
        {
            m_dbConnection = new SQLiteConnection("Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "/db/SYSTEM.db;Version=3;");
            m_dbConnection.Open();
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

        public ArrayList getFunctionInDb()
        {
            string sql = "select DOC_NAME,cast(DOC_CONTENT as TEXT) from DOC_FUNC";
            ArrayList functions = new ArrayList();
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
//            DataTable dt = new DataTable();
  //          dt.Load(reader);
            
          /*  functions = dt.AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
                                row => row.Field<string>(1));*/
             while (reader.Read())
             {
                FunctionDocumentation func = new FunctionDocumentation();
                    func.Name = reader.GetString(0);
                    func.Content= reader.GetString(1);
                functions.Add(func);
                //functions.Add(reader.GetString(0), reader.GetBlob(1,true));
             }
            reader.Close();
            //Console.WriteLine("Name: " + reader["doc_name"] + "\tScore: " + reader["doc_content"]);


            return functions;
        }
    }
}
