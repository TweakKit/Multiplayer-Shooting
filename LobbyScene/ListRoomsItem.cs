using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class ListRoomsItem : MonoBehaviour {

    public Text roomNameText;
    public Text roomAmountText;

    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);

    private JoinRoomDelegate joinRoomDelegateCallBack;
    private MatchInfoSnapshot match;

    // setUp room name and amount in List Rooms
    public void SetUp(MatchInfoSnapshot _match,JoinRoomDelegate _joinRoomDelegateCallBack)
    {
        match = _match;
        joinRoomDelegateCallBack = _joinRoomDelegateCallBack;

        roomNameText.text = match.name;
        roomAmountText.text = "(" + match.currentSize + "/" + match.maxSize + ")"; 
    }

    public void JoinRoom()
    {
        joinRoomDelegateCallBack.Invoke(match);
    }
}
