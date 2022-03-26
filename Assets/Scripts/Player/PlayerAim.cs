using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerAim : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }

    private Transform aimTransform;
    private Animator aimAnimator;
    private Transform aimGunEndPointTransform;

    [SerializeField]
    private Sprite[] playerSprites;
    private SpriteRenderer bodySprite;

    public static int gunType;

    private const float fireRate = .2f;
    private const float cooldown = .5f;
    private int bulletInRow;
    private bool mayFire = true;
    private bool fire;
    public bool _fire { get { return fire; } set { fire = value; if (value == true) StartCoroutine(FireA()); } }

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimAnimator = aimTransform.GetComponent<Animator>();
        aimGunEndPointTransform = aimTransform.Find("GunEndPoint");
        bodySprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();
    }
    private void HandleAiming()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
            aimLocalScale.y = -1f;
        else
            aimLocalScale.y = +1f;

        aimTransform.localScale = aimLocalScale;

        if (angle > 135 || angle < -135)
            bodySprite.sprite = playerSprites[0];
        else if (angle > 45 && angle < 135)
            bodySprite.sprite = playerSprites[3];
         else if (angle < 45 && angle > -45)
            bodySprite.sprite = playerSprites[1];
        else if (angle < -45 && angle > -135)
            bodySprite.sprite = playerSprites[2];

        if (angle > 45 && angle < 135)
            bodySprite.sortingOrder = 12;
        else
            bodySprite.sortingOrder = 10;
    }
    private void HandleShooting()
    {
        //if (gunType == 0 && Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        //    if (Vector3.Distance(mousePosition, aimGunEndPointTransform.position) >= 1.9f)
        //    {
        //        aimAnimator.SetTrigger("Shoot");
        //        OnShoot.Invoke(this, new OnShootEventArgs
        //        {
        //            gunEndPointPosition = aimGunEndPointTransform.position,
        //            shootPosition = mousePosition /* + UtilsClass.GetRandomDir() */
        //        });
        //    }
        //}
        if (Input.GetMouseButton(0))
        {
            if (gunType == 0)
                bulletInRow = 1;
            else if (gunType == 1)
                bulletInRow = 3;
            else if (gunType == 2)
                bulletInRow = 5;
            _fire = true;
        }
        else
            _fire = false;
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
                if (Vector3.Distance(mousePosition, aimGunEndPointTransform.position) >= 1.9f)
                {
                    OnShoot.Invoke(this, new OnShootEventArgs
                    {
                        gunEndPointPosition = aimGunEndPointTransform.position,
                        shootPosition = mousePosition /* + UtilsClass.GetRandomDir() */
                    });
                    aimAnimator.SetTrigger("Shoot");
                }

                yield return new WaitForSeconds(fireRate);
            }
            yield return new WaitForSeconds(cooldown);
            mayFire = true;

            if (_fire)
                StartCoroutine(FireA());
            yield break;
        }
    }
}
