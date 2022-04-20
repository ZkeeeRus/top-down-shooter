using CodeMonkey.Utils;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHandler : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }

    public event EventHandler OnEnemyDead;

    private Animator aimAnimator;

    private Transform target;
    private Transform aimTransform;
    private Transform bodyTransform;
    private Transform aimGunEndPointTransform;

    [SerializeField] private float speed;
    [SerializeField] private float range;

     float nextWaypointDistance = 3f;
     Path path;
     int currentWaypoint = 0;
     bool isEndOfPathReached = false;

     Seeker seeker;
     Rigidbody2D rb;


    public enum Enemy
    {
        Scout,
        Soldier,
        Boss,
        Generator
    }
    [Space]
    public Enemy enemyType;

    [SerializeField] private Sprite[] enemySprites;
    private SpriteRenderer bodySprite;

    [Space]
    public HealthBar healthBar;
    public HealthSystem healthSystem;

    [Space]
    [SerializeField] private GameObject explode;

    private float fireRate = .2f;
    private float cooldown;
    private int bulletInRow;
    private bool mayFire = true;
    private bool fire;
    public bool _fire { get { return fire; } set { fire = value; if (value == true) StartCoroutine(FireA()); } }

    private int level;

    private void Start()
    {
        level = PlayerPrefs.GetInt("Level", 0);

        target = FindObjectOfType<Movement>().transform;
        if (enemyType != Enemy.Generator)
        {
            aimTransform = transform.Find("EnemyAim");
            aimAnimator = aimTransform.GetComponent<Animator>();

            aimGunEndPointTransform = aimTransform.Find("GunEndPoint");
        }
        
        bodyTransform = transform.Find("Body");
        bodySprite = bodyTransform.GetComponent<SpriteRenderer>();




        if (enemyType == Enemy.Boss)
        {
            healthSystem = new HealthSystem(HealthSystem.HealthType.Boss, level);
            healthBar.Setup(healthSystem);

            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

            bulletInRow = 1;
            cooldown = 1f;
            _fire = true;
        }
        else if(enemyType == Enemy.Generator)
        {
            healthSystem = new HealthSystem(HealthSystem.HealthType.Object, level);
            healthBar.Setup(healthSystem);
            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }
        else
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();

            InvokeRepeating("UpdatePath", 0f, .5f);

            healthSystem = new HealthSystem(HealthSystem.HealthType.Enemy, level);
            healthBar.Setup(healthSystem);

            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }

    }
    private void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position - new Vector2(0f, 1.5f), target.position, OnPathComplete);
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        if (healthSystem.GetHealth() <= 0)
        {
            GameObject prfExplode = (GameObject)Instantiate(explode, transform.position + new Vector3(0, 0, -1.75f), Quaternion.identity);
            Destroy(prfExplode, .5f);
            if (OnEnemyDead != null)
            {
                
                OnEnemyDead(this, EventArgs.Empty);
            }

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (GameAssets.isPlayerDeath)
            return;

        if (enemyType == Enemy.Generator)
            return;

        if (enemyType != Enemy.Boss)
        {
            if (path == null)
                return;
            if (Vector3.Distance(target.position, transform.position) <= range)
            {
                FollowPlayer();
                AimAtPlayer();
            }
        }
        else
        {
            AimAtPlayer();
        }

    }
    IEnumerator FireA()
    {
        if (!mayFire)
            yield break;
        else
        {
            mayFire = false;
            for (var i = 0; i < bulletInRow; i++)
            {

                if (!_fire)
                {
                    mayFire = true;
                    yield break;
                }

                Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
                    OnShoot.Invoke(this, new OnShootEventArgs
                    {
                        gunEndPointPosition = aimGunEndPointTransform.position,
                        shootPosition = target.position + UtilsClass.GetRandomDir() * -1.5f
                    });
                    aimAnimator.SetTrigger("Shoot");

                yield return new WaitForSeconds(fireRate);
            }
            yield return new WaitForSeconds(cooldown);
            mayFire = true;

            if (_fire)
                StartCoroutine(FireA());
            yield break;
        }
    }


    public void FollowPlayer()
    {
        Vector3 previous = path.vectorPath[0];
        float allDistance = 0f;
        for (int i = 1; i < path.vectorPath.Count; i++)
        {
            allDistance += Vector3.Distance(previous, path.vectorPath[i]);
            previous = path.vectorPath[i];
        }
        //Vector3.Distance(target.position, transform.position) > range / 2.5
        
        if (allDistance > range / 2.5)
        {
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
            if (currentWaypoint >= path.vectorPath.Count)
            {
                isEndOfPathReached = true;
            }
            else
            {
                isEndOfPathReached = false;

                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 force = direction * speed * 500 * Time.deltaTime;
                rb.AddForce(force);
                
                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }

                _fire = false;
                return;
            }


        }
        if (Random.Range(1f, 5f) > 4.9f)
        {
            if (enemyType == Enemy.Soldier)
            {
                bulletInRow = 3;
                cooldown = .6f;
                _fire = true;
            }
            else if (enemyType == Enemy.Scout)
            {
                bulletInRow = 4;
                cooldown = .5f;
                _fire = true;
            }
        }
        else
            _fire = false;


    }
    public void AimAtPlayer()
    {
        Vector3 aimDirection = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        //Debug.Log(angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
            aimLocalScale.y = -1f;
        else
            aimLocalScale.y = +1f;

        aimTransform.localScale = aimLocalScale;

        switch (enemyType)
        {
            case Enemy.Scout:
                {
                    bodySprite.flipX = false;

                    //bodySprite.sortingLayerName = "Scout";

                    if (angle < 135 && angle > 45)
                        bodySprite.sprite = enemySprites[2];
                    else if (angle < 45 && angle > -45)
                        bodySprite.sprite = enemySprites[0];
                    else if (angle < -45 && angle > -135)
                        bodySprite.sprite = enemySprites[1];
                    else if (angle > 135 || angle < -135)
                    {
                        bodySprite.sprite = enemySprites[0];
                        bodySprite.flipX = true;
                    }
                }
                break;


            case Enemy.Soldier:
                {
                    bodySprite.flipX = false;

                    if (angle < 135 && angle > 45)
                        bodySprite.sprite = enemySprites[5];
                    else if (angle < 45 && angle > -45)
                        bodySprite.sprite = enemySprites[4];
                    else if (angle < -45 && angle > -135)
                        bodySprite.sprite = enemySprites[3];
                    else if (angle > 135 || angle < -135)
                    {
                        bodySprite.sprite = enemySprites[4];
                        bodySprite.flipX = true;
                    }
                   // bodySprite.sortingLayerName = "Soldier";
                }
                break;
        }
        if (enemyType != Enemy.Boss)
        {
            if (angle > 45 && angle < 135)
            {
                if (enemyType == Enemy.Soldier)
                    bodySprite.sortingLayerName = "SoldierUp";
                else if (enemyType == Enemy.Scout)
                    bodySprite.sortingLayerName = "ScoutUp";

                bodySprite.sortingOrder = 12;
            }
            else
            {
                if (enemyType == Enemy.Soldier)
                    bodySprite.sortingLayerName = "Soldier";
                else if (enemyType == Enemy.Scout)
                    bodySprite.sortingLayerName = "Scout";

                bodySprite.sortingOrder = 10;
            }
        }
        
    }
}
