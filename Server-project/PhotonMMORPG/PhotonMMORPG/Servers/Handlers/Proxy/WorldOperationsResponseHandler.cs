﻿using ExitGames.Logging;
using GameCommon;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using MultiplayerGameFramework.Interfaces.Support;
using Servers.Data.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon.SerializedObjects;
using Servers.Models.Interfaces;
using Servers.PubSubModels;

namespace Servers.Handlers.Proxy
{
    public class WorldOperationsResponseHandler : IHandler<IServerPeer>
    {

        public MessageType Type => MessageType.Response;

        public byte Code => (byte)MessageOperationCode.World;

        public int? SubCode => null;

        public ILogger Log { get; private set; }

        private readonly IConnectionCollection<IClientPeer> _connectionCollection;
        private readonly ServerConfiguration _serverConfiguration;
        private readonly IClientCodeRemover _clientCodeRemover;

        public WorldOperationsResponseHandler(ILogger log, IConnectionCollection<IClientPeer> connectionCollection, ServerConfiguration serverConfiguration, IClientCodeRemover clientCodeRemover)
        {
            Log = log;
            _connectionCollection = connectionCollection;
            _serverConfiguration = serverConfiguration;
            _clientCodeRemover = clientCodeRemover;
        }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            // Got a response back from the World - might be successful, might not.
            if (message.Parameters.ContainsKey(_serverConfiguration.PeerIdCode))
            {
                Log.DebugFormat("Looking for Peer Id {0}", new Guid((Byte[])message.Parameters[_serverConfiguration.PeerIdCode]));
                IClientPeer clientPeer = _connectionCollection.GetPeers<IClientPeer>().FirstOrDefault(p => p.PeerId == new Guid((Byte[])message.Parameters[_serverConfiguration.PeerIdCode]));
                if (clientPeer != null)
                {
                    Log.DebugFormat("ON WorldOperationsResponseHandler:  Found Peer");

                    var response = message as Response;

                    if (response.ReturnCode == (short)ReturnCode.Ok)
                    {
                       if (message.SubCode == (int?)MessageSubCode.EnterRegion)
                        {

                        }
                        else if (message.SubCode == (int?) MessageSubCode.RequestRegion)
                        {
                            clientPeer.ClientData<CharacterData>().Region =
                                MessageSerializerService.DeserializeObjectOfType<Region>(response.Parameters[(byte)MessageParameterCode.Object]);
                        }
                        else if (message.SubCode == (int?) MessageSubCode.Move)
                        {

                        }
                    }

                    else if (response.ReturnCode == (short) ReturnCode.WorldAddedNewPlayer)
                    {
                        // Good response, get the client data and look for the userId to set it for the future.
                        if (message.SubCode == (int?)MessageSubCode.EnterWorld)
                        {
                            message.Parameters.Add((byte)MessageParameterCode.PlayerChannel, clientPeer.ClientData<CharacterData>().SelectedCharacter.Name);
                        }
                    }
                    // copy our response to a return response
                    Response returnResponse = new Response(Code, SubCode, message.Parameters);
                    // remove any unnecessary codes from the returning packet
                    _clientCodeRemover.RemoveCodes(returnResponse);

                    // make one call to send the message back - One "exit point" for the message.
                    clientPeer.SendMessage(response);
                }
            }
            return true;
        }
    }
}