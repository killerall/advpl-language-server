﻿using Microsoft.LanguageServer.EditorServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using advpl_language_server.EditorServices;
namespace advpl_language_server.LanguageServicesProtocol.Session
{
    class EditorSession
    {

        /// <summary>
        /// Gets the Workspace instance for this session.
        /// </summary>
        public Workspace Workspace { get; private set; }



        /// <summary>
        /// Gets the LanguageService instance for this session.
        /// </summary>
        public LanguageService LanguageService { get; private set; }

        /// <summary>
        /// Disposes of any Runspaces that were created for the
        /// services used in this session.
        /// </summary>
        public void Dispose()
        {
        }
        public void StartSession()
        {
            this.Workspace = new Workspace();
            this.LanguageService = new LanguageService();
        }
    }
}
