using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpHandler : MonoBehaviour
{
    private TextMesh popUp;
    private Color textColor;
    private float disappearTime;
    private Vector3 moveVector;

    private void Awake()
    {
        popUp = transform.GetComponent<TextMesh>();
        moveVector = new Vector3(Random.Range(-1f, 1f), 1) * 6f;
    }
    public static PopUpHandler Create(Vector2 position, int damage)
    {
        var popUpPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/Damage PopUp"), position, Quaternion.identity);
        PopUpHandler damagePopUp = popUpPrefab.GetComponent<PopUpHandler>();
        damagePopUp.Setup(damage);

        //Destroy(popUpPrefab, .7f);
        return damagePopUp;
    }

    private void Setup(int damage)
    {
        popUp.text = damage.ToString();
        textColor = popUp.color;
        disappearTime = .5f;
    }

    private void FixedUpdate()
    {
        //float moveSpeed = 6f;
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 6f * Time.deltaTime;

        disappearTime -= Time.deltaTime;

        if (disappearTime <= 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            popUp.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}