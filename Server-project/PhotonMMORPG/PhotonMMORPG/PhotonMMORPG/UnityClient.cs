using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using System.Collections.Generic;
using PhotonMMORPG.Common;
using PhotonMMORPG.Operations;
using System.Linq;

namespace PhotonMMORPG
{
     public class UnityClient: ClientPeer
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();
        public string CharacterName { get; private set; }
        public Vector3Net Position { get; private set; }
        public Vector3Net Rotation { get; private set; }
        public float speed;
        public bool jump;
        public bool die;
        public bool respawn;
        public bool attack;


        public UnityClient(InitRequest initRequest) : base(initRequest)
        {
            Log.Info("Player connection ip: " + initRequest.RemoteIP);
            Position = new Vector3Net(325, 0, 250); // default
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            World.Instance.RemoveClient(this);
            var sendParameters = new SendParameters();
            sendParameters.Unreliable = true;
            WorldExitHandler(sendParameters);
            Log.Debug("Disconnected");
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case (byte)OperationCode.Login:
                    var loginRequest = new Login(Protocol, operationRequest);
                    if (!loginRequest.IsValid)
                    {
                        SendOperationResponse(loginRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                        return;
                    }

                    CharacterName = loginRequest.CharacterName;
                    if (World.Instance.IsContain(CharacterName))
                    {
                        SendOperationResponse(loginRequest.GetResponse(ErrorCode.NameExists), sendParameters);
                        return;
                    }

                    World.Instance.AddClient(this);
                    var response = new OperationResponse(operationRequest.OperationCode);
                    SendOperationResponse(response, sendParameters);

                    Log.Info("user with name: " + CharacterName);
                    Log.Info("user id: " + ConnectionId);
                    //World.Instance.ViewCurrentClients();

                    break;
                case (byte)OperationCode.SendChatMessage:
                    {
                        var chatRequest = new ChatMessage(Protocol, operationRequest);

                        if (!chatRequest.IsValid)
                        {
                            SendOperationResponse(chatRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                            return;
                        }

                        string message = chatRequest.Message;

                        string[] param = message.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (param.Length == 2)
                        {
                            string targetName = param[0];
                            message = param[1];
                            if (World.Instance.IsContain(targetName))
                            {
                                var targetClient = World.Instance.TryGetByName(targetName);
                                if (targetClient == null)
                                    return;

                                message = CharacterName + "[PM]:" + message;

                                var personalEventData = new EventData((byte)EventCode.ChatMessage);
                                personalEventData.Parameters = new Dictionary<byte, object> { { (byte)ParameterCode.ChatMessage, message } };
                                personalEventData.SendTo(new UnityClient[] { this, targetClient }, sendParameters);
                            }
                            return;
                        }

                        message = CharacterName + ": " + message;

                        Log.Debug(CharacterName + " said " + message);

                        Chat.Instance.AddMessage(message);

                        var eventData = new EventData((byte)EventCode.ChatMessage);
                        eventData.Parameters = new Dictionary<byte, object> { { (byte)ParameterCode.ChatMessage, message } };
                        eventData.SendTo(World.Instance.Clients, sendParameters);
                    }

                    break;

                case (byte)OperationCode.GetRecentChatMessages:
                    {
                        var messages = Chat.Instance.GetRecentMessages();
                        messages.Reverse();

                        if (messages.Count == 0)
                            break;

                        var message = messages.Aggregate((i, j) => i + "\r\n" + j);
                        var eventData = new EventData((byte)EventCode.ChatMessage);
                        eventData.Parameters = new Dictionary<byte, object> { { (byte)ParameterCode.ChatMessage, message } };
                        eventData.SendTo(new UnityClient[] { this }, sendParameters);
                    }

                    break;

                case (byte)OperationCode.Move:
                    {
                        var moveRequest = new Move(Protocol, operationRequest);
                        if (!moveRequest.IsValid)
                        {
                            SendOperationResponse(moveRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                            return;
                        }

                        Position = new Vector3Net(moveRequest.X, moveRequest.Y, moveRequest.Z);
                        Rotation = new Vector3Net(moveRequest._X, moveRequest._Y, moveRequest._Z);


                        var eventData = new EventData((byte)EventCode.Move);
                        eventData.Parameters = new Dictionary<byte, object> {
                            { (byte)ParameterCode.PosX, Position.X },
                            { (byte)ParameterCode.PosY, Position.Y },
                            { (byte)ParameterCode.PosZ, Position.Z },
                            { (byte)ParameterCode.RotX, Rotation.X },
                            { (byte)ParameterCode.RotY, Rotation.Y },
                            { (byte)ParameterCode.RotZ, Rotation.Z },
                            {(byte)ParameterCode.CharacterName, CharacterName }
                        };
                        
                        eventData.SendTo(World.Instance.Clients, sendParameters);

                    }
                    break;

                case (byte)OperationCode.UpdateAnimator:
                    {
                        var updateAnimatorRequest = new UpdateAnimator(Protocol, operationRequest);
                        if (!updateAnimatorRequest.IsValid)
                        {
                            SendOperationResponse(updateAnimatorRequest.GetResponse(ErrorCode.InvalidParameters), sendParameters);
                            return;
                        }

                        speed = updateAnimatorRequest.Speed;

                        var eventData = new EventData((byte)EventCode.UpdateAnimator);
                        eventData.Parameters = new Dictionary<byte, object> {
                            { (byte)ParameterCode.Speed, updateAnimatorRequest.Speed },
                            { (byte)ParameterCode.Jump, updateAnimatorRequest.Jump },
                            { (byte)ParameterCode.Die, updateAnimatorRequest.Die },
                            { (byte)ParameterCode.Respawn, updateAnimatorRequest.Respawn },
                            { (byte)ParameterCode.Attack, updateAnimatorRequest.Attack },
                            {(byte)ParameterCode.CharacterName, CharacterName }
                        };

                        eventData.SendTo(World.Instance.Clients, sendParameters);
        

                    }
                    break;
                case (byte)OperationCode.WorldEnter:
                    {
                        var eventData = new EventData((byte)EventCode.WorldEnter);
                        eventData.Parameters = new Dictionary<byte, object> {
                            { (byte)ParameterCode.PosX, Position.X },
                            { (byte)ParameterCode.PosY, Position.Y },
                            { (byte)ParameterCode.PosZ, Position.Z },
                            {(byte)ParameterCode.CharacterName, CharacterName }
                        };

                        eventData.SendTo(World.Instance.Clients, sendParameters);
                        Log.Info("Operation WorldEnter: " + CharacterName);

                    }
                    break;
                case (byte)OperationCode.WorldExit:
                    WorldExitHandler(sendParameters);
                    Log.Info("Operation WorldExit: " + CharacterName);
                    break;
                case (byte)OperationCode.ListPlayers:
                    ListPlayersHandler(sendParameters);
                    Log.Info("Operation ListPlayers: " + CharacterName);
                    break;
                default:
                    Log.Debug("Unknown OperationRequest received: " + operationRequest.OperationCode);
                    break;
            }
        }

        private void ListPlayersHandler(SendParameters sendParameters)
        {
            OperationResponse response = new OperationResponse((byte)OperationCode.ListPlayers);

            var players = World.Instance.Clients;

            var dicPlayers = new Dictionary<string, object[]>();

            foreach (var p in players)
            {
                if (!p.CharacterName.Equals(CharacterName))
                    dicPlayers.Add(p.CharacterName, new object[] { p.Position.X, p.Position.Y, p.Position.Z });
            }


            response.Parameters = new Dictionary<byte, object> { { (byte)ParameterCode.ListPlayers, dicPlayers } };
            SendOperationResponse(response, sendParameters);

            Log.Debug("ListPlayersHandler:" + CharacterName);
        }

        private void WorldExitHandler(SendParameters sendParameters)
        {

            var eventData = new EventData((byte)EventCode.WorldExit);
            eventData.Parameters = new Dictionary<byte, object> {
                            { (byte)ParameterCode.CharacterName, CharacterName }
                        };

            eventData.SendTo(World.Instance.Clients, sendParameters);
        }


    }
}
