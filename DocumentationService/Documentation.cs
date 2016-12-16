using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.documentation
{
    class Documentation
    {
        protected ArrayList namesAvaliable;
        protected Dictionary<string, FileStream> avaliable;
        protected String path;
        protected String key;

        public Documentation(string path)
        {
            this.path = path;
            fillAvaliable();
        }
        public Documentation(string path, string key)
        {
            this.path = path;
            this.key = key;
            fillAvaliable();
        }

        protected void fillAvaliable()
        {

            //this.avaliable = DocumentationUtils.getInstance().getFunctionInDb();
            //ArrayList avaliableFiles = getFileRoot();

           /* this.avaliable = new Dictionary<string, FileStream>();

            namesAvaliable =  new ArrayList();
            foreach (FileStream file in avaliableFiles)
            {
                string name = file.Name;
                /*name = name.replace(".db", "");
                if (name.toUpperCase().substring(0, 1).equals(this.key.toUpperCase()))
                {
                    namesAvaliable.add(name);
                    this.avaliable.put(name.toUpperCase(), file);
                }*/
            }
        

        }
      /*  protected ArrayList getFileRoot()
        {
            string path = System.Environment.CurrentDirectory;
            ArrayList avaliableFile = new ArrayList();

            if (path != null)
            {

                File fileDir = null;

                try
                {
                    URL url = FileLocator.resolve(fileURL);
                    fileDir = new File(url.getFile());
                    avaliableFile = DocumentationUtils.getFilesPath(fileDir);

                }
                catch (IOException e)
                {
                    e.printStackTrace();
                }
            }

            return avaliableFile;
        }*/
    }
