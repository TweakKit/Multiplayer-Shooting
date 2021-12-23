using UnityEngine;

public class KillFeed : MonoBehaviour {

    public GameObject killFeedItem;

    void Start()
    {
        // assign the mission for this below delegate
        // when the player was shot by other player, we call this
        GameManager.Instance.onPlayerKillCallBack += OnKill;
    }

    public void OnKill(string player,string source)
    {
        GameObject killFeedItemGO = Instantiate(killFeedItem, this.transform);
        killFeedItemGO.GetComponent<KillFeedItem>().SetUp(player, source);
        Destroy(killFeedItemGO, 3);
    }
}
