using UnityEngine;
using UnityEngine.UI;

public class PlayerStas : MonoBehaviour {

    public Text killsCountText;
    public Text DeathsCountText;

    void Start()
    {
        if (UserAccountManager.Instance.IsLoggedIn)
            UserAccountManager.Instance.GetData(OnReceiveData);
    }

    private void OnReceiveData(string data)
    {
        if (killsCountText == null || DeathsCountText == null)
            return;

        killsCountText.text = "Kills : " + DataTranslator.DataToKills(data).ToString();
        DeathsCountText.text = "Deaths : " + DataTranslator.DataToDeath(data).ToString();
    }
	 
}
