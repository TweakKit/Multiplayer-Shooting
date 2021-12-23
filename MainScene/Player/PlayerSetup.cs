using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    public Behaviour[] componentsToBeDisabled;

    private const string remotePlayer_LayerName = "RemotePlayer";
    private const string dontDraw_LayerName = "DontDraw";

    public GameObject playerGraphics;
    public GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;


    void Start()
    { 
        // if the men who are not a local player
        if(!isLocalPlayer)
        {
            DisableComponents();
            AssignRemotePLayerLayerName();
        }
        // we are now in the game of our own computer
        else
        {
            // Don't let camera capture graphics in LocalPlayer
            SetLayerName(playerGraphics);

            // create UI for this player
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
            playerUIInstance.gameObject.transform.SetParent(this.gameObject.transform.GetChild(2).transform);
            
            // configure playerUI: initialize all the variable on PlayerUI script
            playerUIInstance.GetComponent<PlayerUI>().SetPlayer(this.gameObject.GetComponent<Player>());

            // set up all the property for this player ( also other players. I don't know )
            gameObject.GetComponent<Player>().SetUpPlayer();

            string userName = transform.name;
            if (UserAccountManager.Instance.IsLoggedIn)
                userName = UserAccountManager.Instance.LoggedIn_Username;
            CmdSetUserName(transform.name, userName);
        }

      
    }

    // called every time a local player is set up in their own computer
    public override void OnStartClient()
    {
        base.OnStartClient();
        // in player have a component which is called : NetworkIdentity, used to get the ID when joining
        string _networkID = GetComponent<NetworkIdentity>().netId.ToString(); 
        Player _player = this.gameObject.GetComponent<Player>();

        // when this player was created, we register this player to Dictionary in GameManger
        GameManager.Instance.RegisterPlayer(_networkID, _player);
    }

    // set name layer for objects by means of recursion
    private void SetLayerName(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer(dontDraw_LayerName);
        foreach (Transform children in obj.transform)
        {
            SetLayerName(children.gameObject);
        }
    }

    private void DisableComponents()
    {
        for (int i = 0; i < componentsToBeDisabled.Length; i++)
        {
            componentsToBeDisabled[i].enabled = false;
        }
    }
    private void AssignRemotePLayerLayerName()
    {
        gameObject.layer = LayerMask.NameToLayer(remotePlayer_LayerName);
    }

    [Command]
    private void CmdSetUserName(string _playerID,string userName)
    {
        Player player = GameManager.Instance.GetPlayer(_playerID);
        if(player != null)
        {
            Debug.Log(userName + " has just joined !");
            player.userName = userName; 
        }
    }

    // when this player is dead, we reactive something 
    private void OnDisable()
    {
        Destroy(playerUIInstance);
        if (isLocalPlayer)
            GameManager.Instance.SetSceneCameraActive(true);

        // cus this player was disappeared, so we have to call this method
        GameManager.Instance.UnRegisterPlayer(transform.name); 
    }

}
