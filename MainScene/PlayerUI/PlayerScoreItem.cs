using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreItem : MonoBehaviour {

    public Text playerNameText;
    public Text killsText;
    public Text deathsText;

    public void SetUp(string playerName,int kills,int deaths)
    {
        playerNameText.text = playerName;
        killsText.text = "Kills: " + kills;
        deathsText.text = "Deaths: " + deaths;
    }
}
