using System.Collections; 
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour {

    private int lastKills = 0;
    private int lastDeaths = 0;

    private Player player;

    void Start()
    {
        player = GetComponent<Player>(); 
        
        // we will update every single 5 seconds the data player to database 
        StartCoroutine(SyncScoreLoop());
        
    }

    private IEnumerator SyncScoreLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(5);

            SynDetailed();
        }
    }

    private void SynDetailed()
    {
        if(UserAccountManager.Instance.IsLoggedIn)
        {
            UserAccountManager.Instance.GetData(OnDataReceived);
        }
    }

    private void OnDataReceived(string data)
    {
       
        // if we are not the change in number of kills and deaths, return !
        if (player.kills <= lastKills && player.deaths <= lastDeaths)
            return;

        // count the number of the last data and current data
        int killsSinceLast = player.kills - lastKills;
        int deathsSinceLast = player.deaths - lastDeaths;
         
        // get the data saved on database to count on
        int kills = DataTranslator.DataToKills(data);
        int deaths = DataTranslator.DataToDeath(data);

        // the new data is being prepaerd to save on database after 5s
        int newKills = killsSinceLast + kills;
        int newDeaths = deathsSinceLast + deaths;

        // convert ...
        string newData = DataTranslator.ValuesToData(newKills, newDeaths);
        UserAccountManager.Instance.SendData(newData);

        lastKills = player.kills;
        lastDeaths = player.deaths;
         
    }
}
