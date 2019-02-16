using System;
using System.Collections.Generic;
using System.Linq;
using Assets.MGFClient.Implementation;
using Assets.MGFClient.Message.Implementation;
using Assets.Scripts;
using ExitGames.Client.Photon;
using GameCommon;
using JetBrains.Annotations;
using UnityEngine;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    public string ServerAddres;
    public string ApplicationName;
    public byte SubCodeParameterCode; // 0 => check server.config <subCodeServer>
    public bool UseEncryption;

    public static PhotonEngine Instance = null;
    public EngineState State { get; protected set; }
    public PhotonPeer Peer { get; protected set; }
    public int Ping { get; protected set; }
    public List<GameMessage> eventMessageList = new List<GameMessage>();
    public List<GameMessage> responseMessageList = new List<GameMessage>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            //Destroy object duplicate
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    protected void Initialize()
    {
        State = EngineState.DisconnectedState;
        Application.runInBackground = true;
        GatherMessageHandlers();
        //peer with udp
        Peer = new PhotonPeer(this, ConnectionProtocol.Udp);
    }


    void Start()
    {
        // remove this code => LOGIN BUTTON => no direct connect :)
        Debug.Log(string.Format("Connecting to {0}", ServerAddres));
        ConnectToServer(ServerAddres, ApplicationName);
    }

    void FixedUpdate()
    {
        Ping = Peer.RoundTripTime;
        //Use the state to update 
        State.OnUpdate();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
        Instance = null;
    }

    public void Disconnect()
    {
        if (Peer != null && Peer.PeerState == PeerStateValue.Connected)
        {
            Peer.Disconnect();
        }

        State = EngineState.DisconnectedState;
    }

    public void ConnectToServer(string serverAddres, string applicationName)
    {
        if (State == EngineState.DisconnectedState)
        {
            Peer.Connect(serverAddres, applicationName);
            State = EngineState.WaitingToConnectState;
        }
    }

    public void GatherMessageHandlers()
    {
        foreach (var message in Resources.LoadAll<GameMessage>(""))
        {
            if (message.messageType == MessageType.Async)
            {
                eventMessageList.Add(message);
            }
            else if (message.messageType == MessageType.Response)
            {
                responseMessageList.Add(message);
            }
        }
    }

    #region IPhotonPeerListner
    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(string.Format("Debug Return: {0} - {1}", level, message));
    }

    public void OnEvent(EventData eventData)
    {
        var message = new Assets.MGFClient.Message.Implementation.Event(eventData.Code, (int?)eventData.Parameters[SubCodeParameterCode], eventData.Parameters);
        var handlers = eventMessageList.Where(h => (byte)h.code == message.Code && (int)h.subCode == message.SubCode);
        if (handlers == null || handlers.Count() == 0)
        {
            //Default handler
            Debug.Log(string.Format("Attempted to handle event code: {0} - subcode : {1}", message.Code, message.SubCode));
        }

        foreach (var handler in handlers)
        {
            handler.Notify(message.Parameters);
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        var message = new Response(operationResponse.OperationCode, (int?)operationResponse.Parameters[SubCodeParameterCode], operationResponse.Parameters, operationResponse.DebugMessage, operationResponse.ReturnCode);
        var handlers = responseMessageList.Where(h => (byte)h.code == message.Code && (byte)h.subCode == message.SubCode);
        if (handlers == null || handlers.Count() == 0)
        {
            //Default handler
            Debug.Log(string.Format("Attempted to handle response code: {0} - subcode : {1}", message.Code, message.SubCode));
        }

        foreach (var handler in handlers)
        {
            handler.Notify(message.Parameters, message.DebugMessage, message.ReturnCode);
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(string.Format("Switching status to {0}", statusCode.ToString()));
        switch (statusCode)
        {
            case StatusCode.Connect:
                if (UseEncryption)
                {
                    Peer.EstablishEncryption();
                    State = EngineState.ConnectingState;
                }
                else
                {
                    State = EngineState.ConnectedState;
                }

                break;

            case StatusCode.EncryptionEstablished:
                State = EngineState.ConnectedState;
                break;

            case StatusCode.Disconnect:
            case StatusCode.DisconnectByServer:
            case StatusCode.DisconnectByServerUserLimit:
            case StatusCode.DisconnectByServerLogic:
            case StatusCode.EncryptionFailedToEstablish:
            case StatusCode.Exception:
            case StatusCode.ExceptionOnConnect:
            case StatusCode.ExceptionOnReceive:
            case StatusCode.SecurityExceptionOnConnect:
            case StatusCode.TimeoutDisconnect:
                State = EngineState.DisconnectedState;
                break;
            default:
                State = EngineState.DisconnectedState;
                break;
        }
    }
    #endregion

    public void SendRequest(OperationRequest request)
    {
        State.SendRequest(request, true, 0, UseEncryption);
    }

    public void SendRequest(MessageOperationCode code, MessageSubCode subCode, [CanBeNull] params object[] parameters)
    {
        var request = new OperationRequest()
        {
            OperationCode = (byte)code,
            Parameters = new Dictionary<byte, object>() { { (byte)MessageParameterCode.SubCodeParameterCode, subCode } }
        };

        //all paramters as pairs of 2
        if (parameters != null)
            for (int i = 0; i < parameters.Length; i += 2)
            {
                // 
                if (!(parameters[i] is MessageParameterCode))
                {
                    throw new ArgumentException(string.Format("Paramter {0} is not a MessageParamterCode", i));
                }

                //add the pairs
                request.Parameters.Add((byte) parameters[i], parameters[i + 1]);
            }

        //original send request
        SendRequest(request);
    }
}
