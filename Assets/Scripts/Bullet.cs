using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitPrefab;

    private Vector3 shootDirection;
    [SerializeField] private float moveSpeed;

    private HealthSystem health;

    public Guns playerGun;

    private int addDamage;
    private AudioSource audioSource;
    public enum BulletType
    {
        Simple,
        Bomb
    }
    public BulletType bulletType;

    private void Start()
    {
        addDamage = PlayerPrefs.GetInt("AddDamage", 1);

        if(bulletType == BulletType.Bomb)
            audioSource = GetComponent<AudioSource>();
    }

    public void Setup(Vector3 shootDirection)
    {
        this.shootDirection = shootDirection;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(shootDirection));
        
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        transform.position += shootDirection * moveSpeed * Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            try
            {
                int gunType = playerGun.currentGun + 1;
                int damage = 5 + gunType * 5 * addDamage;

                health = collision.GetComponent<EnemyHandler>().healthSystem;
                health.Damage(damage);
                PopUpHandler.Create(transform.position, damage);
                Destroy(gameObject);
            }
            catch (Exception e)
            {
               // Debug.Log(collision.tag + " вызвал " + e.Message);
            }
        }
        else if (collision.tag == "Player")
        {
            health = collision.GetComponent<PlayerHandler>().healthSystem;
            int damage = 10 + 5 * PlayerPrefs.GetInt("Level", 0);

            if (bulletType == BulletType.Simple)
            {
               
                health.Damage(damage);
                PopUpHandler.Create(transform.position, damage);
                Destroy(gameObject);
            }
            else
            {
                Vector3 pos = Camera.main.transform.position;                
                AudioSource.PlayClipAtPoint(audioSource.clip, new Vector3(pos.x, pos.y, pos.z + 1));

                damage += PlayerPrefs.GetInt("Level", 0) * 3;
                health.Damage(damage);
                PopUpHandler.Create(transform.position, damage);

                GameObject hit = Instantiate(hitPrefab, transform.position + new Vector3(0, 0, -0.2f), Quaternion.Euler(0, 0, -180));
                Destroy(hit, .5f);

                Destroy(gameObject);

            }


        }
        else if (collision.tag == "Wall" || collision.tag == "LockWall")
        {
            if (bulletType == BulletType.Bomb)
            {
                Vector3 pos = Camera.main.transform.position;
                AudioSource.PlayClipAtPoint(audioSource.clip, new Vector3(pos.x, pos.y, pos.z + 1));
            }

            GameObject hit = Instantiate(hitPrefab, transform.position + new Vector3(0, 0, -0.2f), Quaternion.Euler(0, 0, -180));
            Destroy(hit, .5f);
            Destroy(gameObject);
        }
        else if (collision.tag == "Shield" && this.tag != "BossBullet")
        {
            GameObject hit = Instantiate(hitPrefab, transform.position + new Vector3(0, 0, -0.2f), Quaternion.Euler(0, 0, -180));
            Destroy(hit, .5f);
            Destroy(gameObject);
        }

    }
}