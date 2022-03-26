using CodeMonkey.Utils;
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

    private enum Enemy
    {
        Turret,
        Soldier
    }
    [Space]
    [SerializeField] private Enemy enemyType;

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
        aimTransform = transform.Find("EnemyAim");
        aimAnimator = aimTransform.GetComponent<Animator>();

        aimGunEndPointTransform = aimTransform.Find("GunEndPoint");
        bodyTransform = transform.Find("Body");
        bodySprite = bodyTransform.GetComponent<SpriteRenderer>();

        healthSystem = new HealthSystem(100 + 10 * level);
        healthBar.Setup(healthSystem);

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        if (healthSystem.GetHealth() <= 0)
        {
            GameObject prfExplode = (GameObject)Instantiate(explode, transform.position, Quaternion.identity);
            Destroy(prfExplode, .5f);
            if (OnEnemyDead != null)
                OnEnemyDead(this, EventArgs.Empty);

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(target.position, transform.position) <= range)
        {
            FollowPlayer();
            AimAtPlayer();
        }
    }

    //public void ShootAtPlayer()
    //{
    //    aimAnimator.SetTrigger("Shoot");
    //    OnShoot.Invoke(this, new OnShootEventArgs
    //    {
    //        gunEndPointPosition = aimGunEndPointTransform.position,
    //        shootPosition = target.position + UtilsClass.GetRandomDir() * -1.5f
    //    });
    //}
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
        if (Vector3.Distance(target.position, transform.position) > range / 1.3)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
            _fire = false;
            return;
        }
        if (Random.Range(1f, 5f) > 4.9f)
        {
            if (enemyType == Enemy.Soldier)
            {
                bulletInRow = 3;
                cooldown = .5f;
                _fire = true;
            }
            else if (enemyType == Enemy.Turret)
            {
                bulletInRow = 4;
                cooldown = .4f;
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
            case Enemy.Turret:
                {
                    bodySprite.flipX = false;

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
                }
                break;
        }

        if (angle > 45 && angle < 135)
            bodySprite.sortingOrder = 12;
        else
            bodySprite.sortingOrder = 10;
    }
}
