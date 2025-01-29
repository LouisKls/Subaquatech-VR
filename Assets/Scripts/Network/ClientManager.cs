using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Mirror;

public struct Message : NetworkMessage
{
    public string type;
    public string content;
}

public struct ClientInfoMessage : NetworkMessage
{
    public int connectionId;
}

public struct ClientNameMessage : NetworkMessage
{
    public string name;
}


public class ClientManager : NetworkManager
{
    public static ClientManager Instance { get; private set; }

    public delegate void MessageReceivedHandler(Message message);
    public event MessageReceivedHandler OnMessageReceived;

    public int clientId;
    public string clientName = "VR";

    [Header("UI References")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button toggleUIButton;
    [SerializeField] private GameObject connectionPanel;
    [SerializeField] private Image connectionIndicator;

    private string connectionStatus = "Disconnected";

    [Header("Connection Indicator Colors")]
    [SerializeField] private Color connectedColor = Color.green;
    [SerializeField] private Color disconnectedColor = Color.red;

    public override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        InitializeUI();

        NetworkClient.RegisterHandler<Message>(OnServerMessage);
        NetworkClient.RegisterHandler<ClientInfoMessage>(OnServerInfoMessage);

        StartClient();

        Debug.Log("Client has started automatically");
    }

    public override void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        base.OnDestroy();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        UpdateStatusText("Trying to connect ...");
        Debug.Log("Trying to connect ...");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        NetworkClient.RegisterHandler<Message>(OnServerMessage);
        NetworkClient.RegisterHandler<ClientInfoMessage>(OnServerInfoMessage);

        connectionIndicator.color = connectedColor;

        UpdateStatusText("Connected to server");
        Debug.Log("Connected to server");

        SceneManager.LoadScene("TransitionScene"); // REMOVE LATER
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        connectionIndicator.color = disconnectedColor;
        connectButton.interactable = true;

        UpdateStatusText("Disconnected from server");
        Debug.Log("Disconnected from server");
    }

    public override void OnClientError(TransportError error, string reason)
    {
        base.OnClientError(error, reason);

        connectButton.interactable = true;

        UpdateStatusText($"Client error : {reason}");
        Debug.Log($"Client error : {reason}");
    }

    private void OnServerMessage(Message message)
    {
        Debug.Log("Message from server : " + message.type + " - " + message.content);

        if (message.type == "START_VR")
        {
            OnMessageReceived?.Invoke(message);
        }

        if (message.type == "SUBMARINE_LIGHT_ROTATION")
        {
            OnMessageReceived?.Invoke(message);
        }

        if(message.type == "ADMIN_RELEASE_KRAKEN")
        {
            OnMessageReceived?.Invoke(message);
        }

        if (message.type == "KILL_KRAKEN")
        {
            OnMessageReceived?.Invoke(message);
        }
    }

    private void OnServerInfoMessage(ClientInfoMessage message)
    {
        clientId = message.connectionId;
        Debug.Log("Personal ConnectionId : " + clientId);

        ClientNameMessage nameMessage = new ClientNameMessage { name = clientName };
        NetworkClient.Send(nameMessage);
    }

    public void SendNetworkMessage(string type, string content)
    {
        Message message = new Message { type = type, content = content };
        NetworkClient.Send(message);
    }


    // ------------ UI MANAGEMENT ------------ //

    public void RefreshUIReferences()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            toggleUIButton = canvas.transform.Find("WifiButton")?.gameObject.GetComponent<Button>();

            connectionPanel = canvas.transform.Find("ConnectionPanel")?.gameObject;

            statusText = connectionPanel.transform.Find("StatusText")?.gameObject.GetComponent<TMP_Text>();

            ipInputField = connectionPanel.transform.Find("IPInputField")?.gameObject.GetComponent<TMP_InputField>();

            connectButton = connectionPanel.transform.Find("ConnectButton")?.gameObject.GetComponent<Button>();

            connectionIndicator = canvas.transform.Find("WifiButton")?.gameObject.transform.Find("WifiStatusImage")?.gameObject.GetComponent<Image>();
        }

        InitializeUI();
    }

    private void InitializeUI()
    {
        if (NetworkClient.isConnected)
        {
            connectionIndicator.color = connectedColor;
            UpdateStatusText("Connected to server");
        }
        else
        {
            connectionIndicator.color = disconnectedColor;
            UpdateStatusText("Disconnected from server");
        }

        ipInputField.text = "127.0.0.1";

        connectButton.onClick.AddListener(StartClientConnection);

        toggleUIButton.onClick.AddListener(ToggleConnectionPanel);
    }

    public void ToggleConnectionPanel()
    {
        if (connectionPanel.activeSelf)
        {
            connectionPanel.SetActive(false);
        }
        else
        {
            connectionPanel.SetActive(true);
        }
    }

    private void StartClientConnection()
    {
        string ipAddress = ipInputField.text;

        if (string.IsNullOrEmpty(ipAddress))
        {
            UpdateStatusText("Invalid IP address");
            return;
        }

        networkAddress = ipAddress;

        connectButton.interactable = false;
        UpdateStatusText("Trying to connect ...");
        StartClient();

    }

    private void UpdateStatusText(string newStatus)
    {
        connectionStatus = newStatus;
        statusText.text = $"Status : {connectionStatus}";
    }
}