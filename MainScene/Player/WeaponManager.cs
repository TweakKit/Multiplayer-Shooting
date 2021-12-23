using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

    public Transform weaponHolder;
    public PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphic currentGraphic;

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    // initialize 
    private void EquipWeapon(PlayerWeapon _playerWeapon)
    {
        currentWeapon = _playerWeapon;
        GameObject _weaponInstance = Instantiate(_playerWeapon.Graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponInstance.transform.SetParent(weaponHolder);

        currentGraphic = _weaponInstance.GetComponent<WeaponGraphic>();
        if (currentGraphic == null)
            Debug.Log("No weapon");

        // if we are in own computer
        if (isLocalPlayer)
            Util.SetLayerRecursive(_weaponInstance, LayerMask.NameToLayer("Weapon"));
    }

    // return the current weapon
    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    // return the current graphic
    public WeaponGraphic GetCurrentGraphic()
    {
        return currentGraphic;
    }

    // reload bulet
    public void ReloadBullets()
    {
        if (isReloadingBullets)
            return;
        StartCoroutine(ReloadBulletsCoroutine());
    }
    [HideInInspector]
    public bool isReloadingBullets;
    private IEnumerator ReloadBulletsCoroutine()
    {
        isReloadingBullets = true;

        // handle

        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentWeapon.bullets = currentWeapon.maxBullets;
        isReloadingBullets = false;

    }

   
}
