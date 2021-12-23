using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class JoinGame : MonoBehaviour {

    List<GameObject> ListRooms = new List<GameObject>();

    public Text statusJoining;

    public GameObject ListRoomsItem;
    public Transform ListRoomContentParent;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;

        // start again matchMaker because it could be terminated at some points
        if (networkManager.matchMaker == null)
        {
            Debug.Log("Network match is created! Again !!!");
            networkManager.StartMatchMaker();
        }
            
        RefreshListRooms();
    }

    public void RefreshListRooms()
    {
        ClearListRooms();

        // after left,we drop connection(also network match).
        // so if we want to get into again , we have to do like this

        // make sure cuz it could be terminated at some points
        if (networkManager.matchMaker == null)
        {
            Debug.Log("Network match is created! after left !!!");
            networkManager.StartMatchMaker();
        }

        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        statusJoining.text = "Loading...";
    }

    private void ClearListRooms()
    {
        for (int i = 0; i < ListRooms.Count; i++)
        {
            Destroy(ListRooms[i]);
        }

        ListRooms.Clear();  
    }

    public void OnMatchList(bool success,string extendedInfo,List<MatchInfoSnapshot> matchList)
    {
        statusJoining.text = "";
        if(matchList == null)
        {
            statusJoining.text = "Couldn't get list rooms";
            return;
        }

        foreach (MatchInfoSnapshot match in matchList)
        {
            GameObject _ListRoomsItemGO = Instantiate(ListRoomsItem);
            _ListRoomsItemGO.transform.SetParent(ListRoomContentParent);

            // this will take care of setting up the name/amount of users
            // as well as setting up a call back functin that will join the game
            _ListRoomsItemGO.gameObject.GetComponent<ListRoomsItem>().SetUp(match, JoinRoom);

            ListRooms.Add(_ListRoomsItemGO); 
        }

        if (ListRooms.Count == 0)
            statusJoining.text = "No rooms were looked for !";
    }

    // event join
    public void JoinRoom(MatchInfoSnapshot _match)
    {
        // not only join by seconds, but also count down time
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0,0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoining());
    }

    private IEnumerator WaitForJoining()
    { 
        ClearListRooms();

        int timeCountDown = 10;
        while(timeCountDown > 0)
        {
            statusJoining.text = "JOINING.. " + timeCountDown;
            yield return new WaitForSeconds(1);
            timeCountDown--;
        }

        // Failed to connect 
        statusJoining.text = "Failed to connect !";
        yield return new WaitForSeconds(1);

        MatchInfo matchInfo = networkManager.matchInfo;
        if(matchInfo != null)
        { 
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }

        RefreshListRooms();
    }
}
