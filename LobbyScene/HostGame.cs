using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {
    private uint roomSize = 6;
    private string roomName;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;

        // create a network match in the NETWORK MANAGER
        if(networkManager.matchMaker == null)
        {
            Debug.Log("Network match is created!");
            networkManager.StartMatchMaker(); 
        }
    }

    public void SetRoomName(string _roomName)
    {
        roomName = _roomName;
    }

    // create and go to the room 
    public void CreateRoom()
    {
        if(roomName != null && roomName != "")
        {
           // Debug.Log("Creating room .. " + "Room name: " + roomName + " Amount: " + roomSize);
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate); 
        }
    }
	 
}
