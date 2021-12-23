using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
 
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead;

    [SyncVar] // sychronized from the server to all the clients in the game 
    public string userName ="My name is ...";

    public int kills;
    public int deaths;

    public bool isDead
    {
        get { return _isDead; }
        protected set
        {
            _isDead = value;
        }
    }

    public int maxHealth = 100;

    public Behaviour[] disableOnDeath; 
    public GameObject[] disableGameObjectOnDeath;

    public GameObject deathEffect;
    public GameObject spawnEffect;

    private bool[] wasEnable;

    [SyncVar] // we makr this variable like this to : synchronize all of the player can see each other
    private int currentHealth;

    private bool firstSetup = true;

    private WeaponManager weaponManager;

    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown(KeyCode.K))
            RpcTakeDamage(1000, "");
    }

    public void SetUpPlayer()
    {
        // cuz we this when respawn again so we have to check : isLocalPlayer
        if(isLocalPlayer)
        {
            // if we are in local machine player, turn off camera scene
            GameManager.Instance.SetSceneCameraActive(false); 
        }

        CmdBroadCastNewPlayerSetUp();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetUp()
    {
        RpcSetUpPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetUpPlayerOnAllClients()
    {
        if(firstSetup)
        {
            wasEnable = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnable.Length; i++)
            {
                wasEnable[i] = disableOnDeath[i].enabled;
            }
        }

        firstSetup = false;

        SetDefaults();
    }

    // set default for all the properties in player when he was set up at the beginning
    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        // set the number of bullets for this player
        weaponManager.GetCurrentWeapon().bullets = weaponManager.GetCurrentWeapon().maxBullets;

        // enable the components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnable[i];
        }

        // enable the game objects
        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].gameObject.SetActive(true);            
        }
        // enable the collider of player
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;


        // create a spawn effect
        GameObject spawnEffectGO = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(spawnEffectGO, 1);

    }

    // call when this player was shot by the other player
    [ClientRpc]
    public void RpcTakeDamage(int _amount,string _sourceID)
    {
        if (isDead)
            return;

        currentHealth -= _amount;
        if (currentHealth <= 0)
            currentHealth = 0;

        // Debug.Log(transform.name + " now has " + currentHealth + " health. ");

        if(currentHealth <= 0)
        {
            Die(_sourceID);
        } 
    }

    private void Die(string _sourceID)
    {

        Debug.Log(transform.name + " Is Dead !");

        //// get the player who shoot this player
        Player sourcePlayer = GameManager.Instance.GetPlayer(_sourceID);
        if (sourcePlayer != null)
        {
            sourcePlayer.kills++;

            // after this player was died, we invoke the event to handle something
            GameManager.Instance.onPlayerKillCallBack.Invoke(userName, sourcePlayer.userName);
        }

        isDead = true;

        deaths++;


        // disable the components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        // disable the game objects
        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].gameObject.SetActive(false);
        }
        // disable the collider of player
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        // create a death effect
        GameObject deathEffectGO = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectGO, 1);


        if(isLocalPlayer)
        {
            GameManager.Instance.SetSceneCameraActive(true);
        }

        // call respawn to restore player
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);

        // we will return one of the spawn points in the rigister in the network manager
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();

        transform.position = _spawnPoint.transform.position;
        transform.rotation = _spawnPoint.transform.rotation;

        yield return new WaitForSeconds(0.1f);

        SetUpPlayer();

        Debug.Log(transform.name + " has just respawned !");
    }

    // get the current health for this player to set UI healthBar by propotion
    public float GetHealth()
    {
        return (float)currentHealth / maxHealth;
    }

}
