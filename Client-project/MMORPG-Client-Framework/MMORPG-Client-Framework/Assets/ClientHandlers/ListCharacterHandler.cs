using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.MGFClient.Implementation;
using Assets.MGFClient.Interfaces;
using Assets.MGFClient.Message.Implementation;
using GameCommon;
using UnityEngine;

namespace Assets.ClientHandlers
{
    public class ListCharacterHandler : IMessageHandler
    {
        public MessageType Type
        {
            get { return MessageType.Response; }
        }

        public byte Code => (byte)MessageOperationCode.Login;

        public int? SubCode => (int?)MessageSubCode.CharacterList;

        public bool HandleMessage(IMessage message)
        {
            var response = message as Response;
            if (response.ReturnCode == (short)ReturnCode.Ok)
            {
                Debug.LogFormat("Character list - {0}", message.Parameters[(byte)MessageParameterCode.Object]);
            }
            else
            {
                //Show the error
                //ShowError(response.DebugMessage);
                Debug.LogFormat("{0}", response.DebugMessage);
            }

            return true;
        }
    }

}
