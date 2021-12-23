using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseUIMenu : MonoBehaviour {

    public static bool isActive;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId,
                                                 0, networkManager.OnDropConnection);
        // STOP AND GO BACK THE LOBBY SCENE
        networkManager.StopHost();
    }
}
