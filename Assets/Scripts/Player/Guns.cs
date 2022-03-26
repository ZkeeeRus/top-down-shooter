using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    [SerializeField]
    private Sprite[] gunsSpirtes;
    private SpriteRenderer sprite;

    public int currentGun = 0;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        sprite.sprite = gunsSpirtes[currentGun];
    }

    private void Update()
    {
        ChangeGun();
    }
    private void ChangeGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            currentGun = (currentGun - 1) % gunsSpirtes.Length;

            if (currentGun == -1)
                currentGun = gunsSpirtes.Length - 1;
            sprite.sprite = gunsSpirtes[currentGun];

        }
        else if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            currentGun = (currentGun + 1) % gunsSpirtes.Length;

            sprite.sprite = gunsSpirtes[currentGun];
        }

        PlayerAim.gunType = currentGun;
    }
}