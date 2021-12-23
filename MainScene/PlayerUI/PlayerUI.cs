using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public RectTransform thrusterFuelFill;
    public RectTransform healthBarFill;
    public Text ammunitionText;

    public GameObject pauseMenuPanel;
    public GameObject scoreBoardPanel;

    public GameObject crossHair;

    private PlayerController controller;
    private Player player;
    private WeaponManager weaponManager;

    void Start()
    {
        PauseUIMenu.isActive = false;
    }

    void Update()
    {
        // set something
        SetFuelAmount(controller.GetThrusterFuelAmount());
        SetHealthAmount(player.GetHealth());
        SetAmmunitionAmount(weaponManager.GetCurrentWeapon().bullets);
 
        // show pausMenu panel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        // show scoreBoard panel
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            scoreBoardPanel.gameObject.SetActive(true);
            crossHair.gameObject.SetActive(false);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoardPanel.gameObject.SetActive(false);
            crossHair.gameObject.SetActive(true);
        }
           

    }

    // set all the variable above
    public void SetPlayer(Player _player)
    {
        player = _player.gameObject.GetComponent<Player>();
        controller = _player.gameObject.GetComponent<PlayerController>();
        weaponManager = _player.gameObject.GetComponent<WeaponManager>();
    }

    // set fule Amount
    private void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(_amount, 1, 1);
    }
    // set health Amount
    private void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(_amount, 1, 1);
    }
    // set ammunition Amount
    private void SetAmmunitionAmount(float _amount)
    {
        ammunitionText.text = _amount.ToString();
    }

    // toggle menu show
    public void TogglePauseMenu()
    { 
        pauseMenuPanel.gameObject.SetActive(!pauseMenuPanel.activeSelf);

        // set the property for this
        PauseUIMenu.isActive = pauseMenuPanel.activeSelf;
    }


}
