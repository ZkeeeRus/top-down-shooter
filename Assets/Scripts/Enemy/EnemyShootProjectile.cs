using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootProjectile : MonoBehaviour
{
    [SerializeField]
    private Transform bulletPrefab;

    [SerializeField]
    private Sprite shootFlashSprite;

    private AudioSource audioSource;

    private void Awake()
    {
        GetComponent<EnemyHandler>().OnShoot += EnemyShootProjectile_OnShoot;

        audioSource = GetComponent<AudioSource>();
    }

    private void EnemyShootProjectile_OnShoot(object sender, EnemyHandler.OnShootEventArgs e)
    {
        Transform bulletTransform = Instantiate(bulletPrefab, e.gunEndPointPosition, Quaternion.identity);

        Vector3 shootDirection = (e.shootPosition - e.gunEndPointPosition).normalized;

        bulletTransform.GetComponent<Bullet>().Setup(shootDirection);

        //UtilsClass.ShakeCamera(.05f, .2f);
        if(shootFlashSprite != null)
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