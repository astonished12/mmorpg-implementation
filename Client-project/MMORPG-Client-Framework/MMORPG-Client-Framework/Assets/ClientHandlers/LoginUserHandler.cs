using Assets.MGFClient.Implementation;
using Assets.MGFClient.Interfaces;
using Assets.MGFClient.Message.Implementation;
using GameCommon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.ClientHandlers
{
    public class LoginUserHandler : IMessageHandler
    {
        public MessageType Type
        {
            get { return MessageType.Response; }
        }

        public byte Code => (byte)MessageOperationCode.Login;
        public int? SubCode => (int?)MessageSubCode.LoginUserPass;

        public bool HandleMessage(IMessage message)
        {
            var response = message as Response;
            if (response.ReturnCode == (short)ReturnCode.Ok)
            {
                // successful login
                SceneManager.LoadScene("CharacterSelect");

            }
            else
            {
                //ShowError(response.DebugMessage);
            }

            return true;
        }
    }
}
