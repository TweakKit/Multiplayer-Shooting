using UnityEngine;

public class ScoreBoard : MonoBehaviour {

    public GameObject PlayerScoreItem;
    public Transform ListPlayerScoreContainer;

    // call when script was enable. Because we use the trick: active and deactive 
    // so we do like this to set value in scoreBoard
    void OnEnable()
    { 
        Player[] players = GameManager.Instance.GetAllPlayer();

        foreach (Player player in players)
        {
            GameObject PlayerScoreItemGO = Instantiate(PlayerScoreItem, ListPlayerScoreContainer);
            PlayerScoreItemGO.GetComponent<PlayerScoreItem>().SetUp(player.userName, player.kills, player.deaths);
        }
    }

    void OnDisable()
    {
        foreach (Transform child in ListPlayerScoreContainer)
        {
            Destroy(child.gameObject);
        }
    }

}
