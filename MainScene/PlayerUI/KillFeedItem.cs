using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour {

    public Text killFeedText;

    public void SetUp(string player,string source)
    {
        killFeedText.text = source + " killed " + player;
    }
}
