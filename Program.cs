using AdvplLSPServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_language_server
{
    class Program
    {
        static void Main(string[] args)
        {
            init();
            
        }
        static void init()
        {
            LanguageServer server = new LanguageServer();
            server.Start().Wait(); 
        }
    }
}
