using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{
    [SerializeField]
    private Transform bulletPrefab;

    [SerializeField]
    private Sprite shootFlashSprite;
    private AudioSource audioSource;
    private void Awake()
    {
        GetComponent<PlayerAim>().OnShoot += PlayerShootProjectile_OnShoot;
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayerShootProjectile_OnShoot(object sender, PlayerAim.OnShootEventArgs e)
    {
        Transform bulletTransform = Instantiate(bulletPrefab, e.gunEndPointPosition, Quaternion.identity);

        Vector3 shootDirection = (e.shootPosition - e.gunEndPointPosition).normalized;

        Bullet bullet =  bulletTransform.GetComponent<Bullet>();

        bullet.playerGun = transform.Find("Aim").GetComponentInChildren<Guns>();

        bullet.Setup(shootDirection);

        //UtilsClass.ShakeCamera(.05f, .2f);
        CreateShootFlash(e.gunEndPointPosition);

        Vector3 pos = Camera.main.transform.position;
        AudioSource.PlayClipAtPoint(audioSource.clip, new Vector3(pos.x, pos.y, pos.z + 1));
    }
    private void CreateShootFlash(Vector3 spawnPosition)
    {
        World_Sprite worldSprite = World_Sprite.Create(spawnPosition, shootFlashSprite);

        FunctionTimer.Create(worldSprite.DestroySelf, .1f);
    }
}