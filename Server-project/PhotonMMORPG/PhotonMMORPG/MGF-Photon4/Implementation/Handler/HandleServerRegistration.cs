using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using log4net.Core;
using MGF_Photon.Implementation.Data;
using MGF_Photon.Implementation.Operation;
using MGF_Photon.Implementation.Operation.Data;
using MGF_Photon.Implementation.Server;
using MultiplayerGameFramework.Implementation.Config;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using Photon.SocketServer;
using ErrorCode = MGF_Photon.Implementation.Code.ErrorCode;
using ILogger = ExitGames.Logging.ILogger;

namespace MGF_Photon.Implementation.Handler
{
    public class HandleServerRegistration : ServerHandler
    {
        private readonly IServerType _serverType;
        public ILogger Log { get; set; }
        private ServerConfiguration _serverConfiguration;
        public HandleServerRegistration(ILogger log, IServerType serverType, ServerConfiguration serverConfiguration)
        {
            Log = log;
            _serverType = serverType;
            _serverConfiguration = serverConfiguration;
        }

        public override MessageType Type
        {
            get { return MessageType.Request; }
        }

        public override byte Code
        {
            get { return 0; }
        }

        public override int? SubCode
        {
            get { return null; }
        }

        public override bool OnHandleMessage(IMessage message, IServerPeer serverPeer)
        {
            var peer = serverPeer as PhotonServerPeer;
            if (peer != null)
            {
                return OnHandleMessage(message, peer);
            }

            return false;
        }

        protected bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
        {
            OperationResponse operationResponse;
            //we are already registered tell subserver if tried to register more than once
            if (serverPeer.Registered)
            {
                operationResponse = new OperationResponse(message.Code) { ReturnCode = (short)ErrorCode.InternalServerError, DebugMessage = "Already registred" };
            }
            else
            {
                var registreRequest = new RegisterSubServer(serverPeer.Protocol, message);

                //Register Sub Server Operation is bad somehting is missing ,etc
                if (!registreRequest.IsValid)
                {
                    string msg = registreRequest.GetErrorMessage();
                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("Invalid register Request {0}", msg);
                    }

                    operationResponse = new OperationResponse(message.Code) { DebugMessage = msg, ReturnCode = (short)ErrorCode.OperationInvalid };
                }
                else
                {
                    //Valid message not registred, process registration

                    XmlSerializer mySerializer = new XmlSerializer(typeof(RegisterSubServerData));
                    StringReader inStream = new StringReader(registreRequest.RegisterSubServerOperation);
                    var registerData = (RegisterSubServerData)mySerializer.Deserialize(inStream);

                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("Received register request: Address {0}, UdpPort={1}, TcpPort={2}, Type={3}",
                            registerData.GameServerAddress, registerData.UdpPort, registerData.TcpPort, registerData.ServerType);
                    }

                    var serverData = serverPeer.ServerData<ServerData>();
                    if (serverData == null)
                    {
                        //Autofac doesnt have a reference to ServerData so it doesnt exist in server's IServerData list
                        Log.DebugFormat("ServerData is null..");
                    }

                    if (registerData.UdpPort.HasValue)
                    {
                        serverData.UdpAddress = registerData.GameServerAddress + ";" + registerData.UdpPort;
                    }

                    if (registerData.TcpPort.HasValue)
                    {
                        serverData.TcpAddress = registerData.GameServerAddress + ";" + registerData.TcpPort;
                    }

                    //setting server id
                    serverData.ServerId = registerData.ServerId;
                    //setting server type
                    serverData.ServerType = registerData.ServerType;
                    //looking up the server type for the server peer
                    serverPeer.ServerType = _serverType.GetServerType(registerData.ServerType);
                    //setting application name=to the server name
                    serverData.ApplicationName = registerData.ServerName;

                    operationResponse = new OperationResponse(message.Code, new Dictionary<byte, object>(){{_serverConfiguration.SubCodeParameterCode,0}});

                    serverPeer.Registered = true;
                }
            }

            serverPeer.SendOperationResponse(operationResponse, new SendParameters());
            return true;
        }
    }
}
