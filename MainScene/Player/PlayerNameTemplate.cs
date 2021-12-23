using UnityEngine;
using UnityEngine.UI;

public class PlayerNameTemplate : MonoBehaviour {

    public Text userNameText;
    public RectTransform healthBarFill;
    public Player player;

    void Update()
    {
        userNameText.text = player.userName;
        healthBarFill.localScale = new Vector3(player.GetHealth(), 1, 1);
    }

}
