using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";

    public LayerMask maskPlayerShoot;
    public Camera camHit;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;


    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        if (PauseUIMenu.isActive || gameObject.GetComponent<Player>().isDead)
            return;

        currentWeapon = weaponManager.GetCurrentWeapon();

        // if we press R key, we reload bullets to maximum
        if (currentWeapon.bullets < currentWeapon.maxBullets)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                weaponManager.ReloadBullets();
                return;
            }
        }

        // shooting
        if (Input.GetButtonDown("Fire1"))
        {
            InvokeRepeating("Shoot", 0, 1 / currentWeapon.fireRate);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            CancelInvoke("Shoot");
        }

    }

    [Client]
    private void Shoot()
    {  

        // if who is not local player
        if (!isLocalPlayer || weaponManager.isReloadingBullets)
            return;

        // if we are running out of bullets
        if (currentWeapon.bullets <= 0)
        {
            weaponManager.ReloadBullets();
            return;
        }

        // decrease the number of bullets
        currentWeapon.bullets--;

        // make up effect when shooting 
        CmdOnShoot();

        // make a sound
        MusicManager.Instance.playSound(MusicManager.Instance.shoot, 1, 1);

        RaycastHit hit;
        if (Physics.Raycast(camHit.transform.position, camHit.transform.forward, out hit, currentWeapon.range, maskPlayerShoot))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage, transform.name);
            }

            // make up effect when hit on something
            CmdOnPlayerHit(hit.point, hit.normal); // use normal we always get the accurate position and rotation 

        }

        // check if the player run out of the bullets so restore
        if (currentWeapon.bullets <= 0)
            weaponManager.ReloadBullets();
    }

    // effect shooting
    [Command]
    private void CmdOnShoot()
    {
        RpcDoShootEffect();
    }
    [ClientRpc]
    private void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphic().FlareShoot.Play();
    }

    [Command]
    private void CmdPlayerShot(string _playerID, int _damage, string sourceID)
    {
        Debug.Log(_playerID + " has been shot");

        // take damage on itself
        Player _player = GameManager.Instance.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, sourceID);
    }

    // effect hitting
    [Command]
    private void CmdOnPlayerHit(Vector3 _position, Vector3 _normal)
    {
        RpcDoHitPlayerEffect(_position, _normal);
    }
    [ClientRpc]
    private void RpcDoHitPlayerEffect(Vector3 _position, Vector3 _normal)
    {
        GameObject _hitPlayerEffect = Instantiate(weaponManager.GetCurrentGraphic().hitCollisionEffect,
                                                 _position, Quaternion.LookRotation(_normal));


        Destroy(_hitPlayerEffect, 3f);
    }
}
