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

    private void Awake()
    {
        GetComponent<EnemyHandler>().OnShoot += EnemyShootProjectile_OnShoot;
    }

    private void EnemyShootProjectile_OnShoot(object sender, EnemyHandler.OnShootEventArgs e)
    {
        Transform bulletTransform = Instantiate(bulletPrefab, e.gunEndPointPosition, Quaternion.identity);

        Vector3 shootDirection = (e.shootPosition - e.gunEndPointPosition).normalized;

        bulletTransform.GetComponent<Bullet>().Setup(shootDirection);

        //UtilsClass.ShakeCamera(.05f, .2f);
        CreateShootFlash(e.gunEndPointPosition);
    }
    private void CreateShootFlash(Vector3 spawnPosition)
    {
        World_Sprite worldSprite = World_Sprite.Create(spawnPosition, shootFlashSprite);

        FunctionTimer.Create(worldSprite.DestroySelf, .1f);
    }
}