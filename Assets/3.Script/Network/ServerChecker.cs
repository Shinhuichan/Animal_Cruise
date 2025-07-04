using UnityEngine;
using Mirror;
using System;
using System.Collections.Generic;
using System.IO;
using kcp2k;
using LitJson;


public enum Type
{
    Empty = 0,
    Client,
    Server
}

public class Item
{
    public string License;
    public string ServerIP;
    public string Port;

    public Item (string L_index, string IPValue, string port)
    {
        License = L_index;
        ServerIP = IPValue;
        Port = port; 
    }
}

public class ServerChecker : MonoBehaviour
{
    public Type type;

    private NetworkRoomManager manager;
    private KcpTransport kcp;

    [SerializeField] private string path;

    public string ServerIP {get; private set;}
    public string Port {get; private set;}

    /*\
    [
        {
            "Lisence":"2",
            "Server_IP":"3.36.74.143",
            "Port":"7777"
        }
    ]
    */
    
    void Awake()
    {
        // Headless 환경에서만 적용
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            Debug.Log("Headless 서버 모드 감지: 렌더러/오디오 비활성화");

            // 모든 카메라 비활성화
            Camera[] cameras = FindObjectsOfType<Camera>();
            foreach (Camera cam in cameras)
            {
                cam.enabled = false;
            }

            // 오디오 완전 비활성화
            AudioListener[] listeners = FindObjectsOfType<AudioListener>();
            foreach (AudioListener listener in listeners)
            {
                listener.enabled = false;
            }

            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource source in audioSources)
            {
                source.enabled = false;
            }

            // Quality Level을 최하로
            QualitySettings.SetQualityLevel(0, true);
            
            // VSync 끄기
            QualitySettings.vSyncCount = 0;
 
        }
    }
    private void OnEnable()
    {
        //총 두번 작업 해야함.
        path = Application.dataPath + "/License";

        //폴더 검사
        if (!File.Exists(path))
        {

            Directory.CreateDirectory(path);
        }
        //파일 검사
        if (!File.Exists(path + "/License.json"))
        {
            DefaultData(path);
        }

        manager = GetComponent<NetworkRoomManager>();
        kcp = (KcpTransport)manager.transport;
    }

    private void DefaultData(string path)
    {
        //json 만드는 작업
        List<Item> item = new List<Item>();
        item.Add(new Item("0", "127.0.0.1", "7777"));

        JsonData data = JsonMapper.ToJson(item);
        File.WriteAllText(path + "/License.json", data.ToString());
    }

    private Type License_type()
    {
        Type t = Type.Empty;
        try
        {
            string jsonString = File.ReadAllText(path + "/License.json");
            JsonData itemdata = JsonMapper.ToObject(jsonString);

            string type_s = itemdata[0]["License"].ToString();
            string ip_s = itemdata[0]["ServerIP"].ToString();
            string port_s = itemdata[0]["Port"].ToString();

            ServerIP = ip_s;
            Port = port_s;
            type = (Type)Enum.Parse(typeof(Type), type_s);

            manager.networkAddress = ServerIP;
            kcp.port = ushort.Parse(Port);

            return type;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            return Type.Empty;
        }
    }

    //웹 GL: 차단
    private void Start()
    {
        type = License_type();

        if(type.Equals(Type.Server))
        {
            Start_Server();
        }
        else
        {
            Client_Server();
        }
    }

    public void Start_Server()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL Cannot be Server");
        }
        else
        {
            manager.StartServer();
            Debug.Log($"{manager.networkAddress} Start Server...");
            NetworkServer.OnConnectedEvent += (NetworkConnectionToClient)=>
            {
                Debug.Log($"new Client Connect : {NetworkConnectionToClient.address}");
            };
            NetworkServer.OnDisconnectedEvent += (NetworkConnectionToClient)=>
            {
                Debug.Log($"new Client Disconnect : {NetworkConnectionToClient.address}");
            };
        }
    }
    public void Client_Server()
    {
        manager.StartClient();
        Debug.Log($"{manager.networkAddress} : Start Client...");
    }

    private void OnApplicationQuit()
    {
        if(NetworkClient.isConnected)
        {
            manager.StopClient();
        }

        if(NetworkServer.active)
        {
            manager.StopServer();
        }
    }
}
