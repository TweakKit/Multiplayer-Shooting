using UnityEngine;

[System.Serializable]
public class PlayerWeapon{

    public string nameWeapon = "Ak47";

    public int damage = 10;
    public float range = 100;

    public float fireRate = 9;
    public int maxBullets = 999;

    [HideInInspector]
    public int bullets;

    public GameObject Graphics;

    public float reloadTime = 1;
    public PlayerWeapon()
    {
        bullets = maxBullets;
    }
}
