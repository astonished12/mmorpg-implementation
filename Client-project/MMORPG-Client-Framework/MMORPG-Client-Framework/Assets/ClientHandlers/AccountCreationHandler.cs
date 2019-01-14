using System;
using System.Diagnostics;
using Assets.MGFClient.Implementation;
using Assets.MGFClient.Interfaces;
using Assets.MGFClient.Message.Implementation;
using GameCommon;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.ClientHandlers
{
    public class AccountCreationHandler: IMessageHandler
    {
        public MessageType Type
        {
            get { return MessageType.Response; }
        }

        public byte Code => (byte) MessageOperationCode.Login;

        public int? SubCode => (int?)MessageSubCode.LoginNewAccount;

        public bool HandleMessage(IMessage message)
        {
            var response = message as Response;
            if (response.ReturnCode == (short) ReturnCode.Ok)
            {
                //Show the login server
                Debug.LogFormat("Account Created Successfully");
            }
            else
            {
                //Show the error
                //ShowError(response.DebugMessage);
                Debug.LogFormat("{0}",response.DebugMessage);
            }

            return true;
        }
    }
}
