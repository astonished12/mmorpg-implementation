using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Implementation.Messaging;
using GameCommon;
using ExitGames.Logging;
using Servers.Interfaces;
using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Support;
using MultiplayerGameFramework.Implementation.Config;
using Servers.Data.Client;
using MGF.Domain;

namespace Servers.Handlers.Login
{
    public class LoginAccountCreationHandler : IHandler<IServerPeer>
    {
        private ILogger Log;
        private IAuthorizationService AuthService;
      
        public LoginAccountCreationHandler(ILogger log, IAuthorizationService authService)
        {
            Log = log;
            AuthService = authService;
        }

        public MessageType Type => MessageType.Request;

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => (int)MessageSubCode.LoginNewAccount;

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            Response response;
            // If not enough arguments, ok to return Invalid - Normally we return InvalidUserPass
            if (!message.Parameters.ContainsKey((byte)MessageParameterCode.LoginName) || !message.Parameters.ContainsKey((byte)MessageParameterCode.Password))
            {
                Log.DebugFormat("Sending Invalid Operation Response");
                response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "Not enough arguments", (short)ReturnCode.OperationInvalid);
                peer.SendMessage(response);
            }
            else
            {
                // Use our preferred Authorization Service to check if authorized
                // Start by seeing if an account already existts with this name
                var returnCode = AuthService.CreateAccount((string)message.Parameters[(byte)MessageParameterCode.LoginName], (string)message.Parameters[(byte)MessageParameterCode.Password]);
                if (returnCode != ReturnCode.Ok)
                {
                    response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "Cannot create account", (short)ReturnCode.InvalidUserPass);
                }
                else
                {
                    // UserPass is not good.
                    response = new Response(Code, SubCode, new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, SubCode }, { (byte)MessageParameterCode.PeerId, message.Parameters[(byte)MessageParameterCode.PeerId] } }, "Account created", (short)ReturnCode.Ok);
                }
            }
            return true;
        }
    }
}
