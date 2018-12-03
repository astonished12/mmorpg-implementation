using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.MGFClient.Implementation;
using Assets.MGFClient.Message.Implementation;
using Assets.MGFClient.Interfaces;
using ExitGames.Client.Photon;
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
    public List<IMessageHandler> eventHandlerList { get; protected set; }
    public List<IMessageHandler> responseHandlerList { get; protected set; }


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
        var handlers =
            from t in Assembly.GetAssembly(GetType()).GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IMessageHandler)))
            select Activator.CreateInstance(t) as IMessageHandler;

        Debug.Log(string.Format("Found {0} handlers", handlers.Count()));
        eventHandlerList = handlers.Where(h => h.Type == MessageType.Async).ToList();
        responseHandlerList = handlers.Where(h => h.Type == MessageType.Response).ToList();

    }

    #region IPhotonPeerListner
    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(string.Format("Debug Return: {0} - {1}", level, message));
    }

    public void OnEvent(EventData eventData)
    {
        var message = new Assets.MGFClient.Message.Implementation.Event(eventData.Code, (int?)eventData.Parameters[SubCodeParameterCode], eventData.Parameters);
        var handlers = eventHandlerList.Where(h => h.Code == message.Code && h.SubCode == message.SubCode);
        if (handlers == null || handlers.Count() == 0)
        {
            //Default handler
            Debug.Log(string.Format("Attempted to handle event code: {0} - subcode : {1}", message.Code, message.SubCode));
        }

        foreach (var handler in handlers)
        {
            handler.HandleMessage(message);
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {

        var message = new Assets.MGFClient.Message.Implementation.Response(operationResponse.OperationCode, (int?)operationResponse.Parameters[SubCodeParameterCode], operationResponse.Parameters);
        var handlers = responseHandlerList.Where(h => h.Code == message.Code && h.SubCode == message.SubCode);
        if (handlers == null || handlers.Count() == 0)
        {
            //Default handler
            Debug.Log(string.Format("Attempted to handle response code: {0} - subcode : {1}", message.Code, message.SubCode));
        }

        foreach (var handler in handlers)
        {
            handler.HandleMessage(message);
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
}
