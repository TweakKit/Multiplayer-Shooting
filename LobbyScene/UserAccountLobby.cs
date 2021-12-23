using UnityEngine;
using UnityEngine.UI;

public class UserAccountLobby : MonoBehaviour {

    public Text userNameText;

    void Start()
    { 
        if (UserAccountManager.Instance.IsLoggedIn)
            userNameText.text = UserAccountManager.Instance.LoggedIn_Username; 
    }

    public void LogOut()
    {
        if (UserAccountManager.Instance.IsLoggedIn)
            UserAccountManager.Instance.LogOut();
    }
	 
}
