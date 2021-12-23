using UnityEngine;
using System.Collections.Generic; 
using System.Linq;  


public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    private const string PLAYER_ID_PREFIX = "Player ";

    public GameObject sceneCamera;
     
    private Dictionary<string, Player> players = new Dictionary<string, Player>();


    // delegate handle
    public delegate void OnPlayerKillCallBack(string _player, string _source);
    public OnPlayerKillCallBack onPlayerKillCallBack;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.Log("More than one Game manager in scene");
        }
        else
        {
            Instance = this;
        }  
    }

    // set camera deactivate
    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.gameObject.SetActive(isActive);
    }
     
    // register player
    public void RegisterPlayer(string _netID,Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID; 
    }

    // unregister player
    public void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    // get player
    public Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    // get all the current players in the game
    public Player[] GetAllPlayer()
    {
        return players.Values.ToArray();
    }
}
