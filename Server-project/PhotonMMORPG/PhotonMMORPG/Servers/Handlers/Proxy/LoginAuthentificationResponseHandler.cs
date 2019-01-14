using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Photon.SocketServer;
using Servers.Data.Client;

namespace Servers.Handlers.Proxy
{
    public class LoginAuthentificationResponseHandler : IHandler<IServerPeer>
    {
        private readonly ServerConfiguration _serverConfiguration;
        private readonly IConnectionCollection<IClientPeer> _connectionCollection;
        private readonly IClientCodeRemover _clientCodeRemover;

        public MessageType Type => MessageType.Response;
        public byte Code => (byte) MessageOperationCode.Login;

        public int? SubCode => (int?) MessageSubCode.LoginUserPass;

        public ILogger Log { get; private set; }

        public LoginAuthentificationResponseHandler(ILogger log,
            IConnectionCollection<IClientPeer> connectionCollection, ServerConfiguration serverConfiguration,
            IClientCodeRemover clientCodeRemover)
        {
            Log = log;
            _connectionCollection = connectionCollection;
            _serverConfiguration = serverConfiguration;
            _clientCodeRemover = clientCodeRemover;
        }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            //Got a response back from the LoginUserPass - might be succesfull, might not
            if (message.Parameters.ContainsKey(_serverConfiguration.PeerIdCode))
            {
                Log.DebugFormat("Looking for Peer Id {0}",
                    new Guid((Byte[]) message.Parameters[_serverConfiguration.PeerIdCode]));
                IClientPeer clientPeer = _connectionCollection.GetPeers<IClientPeer>().FirstOrDefault(p =>
                    p.PeerId == new Guid((Byte[]) message.Parameters[_serverConfiguration.PeerIdCode]));

                if (clientPeer != null)
                {
                    Log.DebugFormat("Found Peer");
                    var reponse = message as Response;

                    //copy our response to a return response
                    Response returnResponse = new Response(Code,SubCode, message.Parameters);
                    //remove any unecessary code from the returning packet
                    _clientCodeRemover.RemoveCodes(returnResponse);
                    if (reponse.ReturnCode != (short) ReturnCode.Ok)
                    {
                        clientPeer.ClientData<CharacterData>().UserId = (int)
                            reponse.Parameters[(byte) MessageParameterCode.UserId];
                    }
                    clientPeer.SendMessage(reponse);

                }
            }

            return true;
        }
    }

}