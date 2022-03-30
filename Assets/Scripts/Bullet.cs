using CodeMonkey.Utils;
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

    private void Start()
    {
        addDamage = PlayerPrefs.GetInt("AddDamage", 1);
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
                int gunType = playerGun.currentGun;
                int damage = 20 - gunType * 5 * addDamage;

                 //Debug.Log(damage);

                health = collision.GetComponent<EnemyHandler>().healthSystem;
                health.Damage(damage);
                Destroy(gameObject);
            }
            catch { }
        }
        else if (collision.tag == "Player")
        {
            health = collision.GetComponent<PlayerHandler>().healthSystem;

            health.Damage(10 * addDamage);
            Destroy(gameObject);
        }
        else if (collision.tag == "Wall" || collision.tag == "LockWall")
        {
            GameObject hit = Instantiate(hitPrefab, transform.position + new Vector3(0,0, -0.2f), Quaternion.Euler(0, 0, -180));
            Destroy(hit, .5f);
            Destroy(gameObject);
        }

    }
}