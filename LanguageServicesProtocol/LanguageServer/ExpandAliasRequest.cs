//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.LanguageServer.EditorServices.Protocol.MessageProtocol;

namespace Microsoft.LanguageServer.EditorServices.Protocol.LanguageServer
{
    public class ExpandAliasRequest
    {
        public static readonly
            RequestType<string, string> Type =
            RequestType<string, string>.Create("powerShell/expandAlias");
    }
}
