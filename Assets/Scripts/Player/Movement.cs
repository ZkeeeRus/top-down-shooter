using CodeMonkey;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour 
{
	public float speed;
	public float acceleration;

	private Vector3 direction;
	private Rigidbody2D body;

	public float CamOffset;

	private float endurance = 100;
	private bool isTired = false;

	private GameObject enduranceBar;

	private void Start()
	{
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;
		body.gravityScale = 0;

		enduranceBar = transform.Find("EnduranceBar").gameObject;
	}

	private void FixedUpdate()
	{
		if (GameAssets.isPlayerDeath)
			return;

		HandleShift();
		HandleMovement();
	}

	private void HandleShift()
    {
		if ( !isTired && endurance > 1.2f && Input.GetKey(KeyCode.LeftShift) && direction.magnitude > 0.1)
		{
			endurance -= 35f * Time.deltaTime;

			speed = 3.5f;
			acceleration = 170;

			if (!enduranceBar.activeInHierarchy)
				enduranceBar.SetActive(true);
		}
		else
		{
			if (endurance < 100)
			{
				if (endurance <= 1.2)
					isTired = true;

				endurance += 25f * Time.deltaTime;

				if (!enduranceBar.activeInHierarchy)
					enduranceBar.SetActive(true);
			}
			else
			{
				isTired = false;
				endurance = 100;

				if (enduranceBar.activeInHierarchy)
					enduranceBar.SetActive(false);
			}

			speed = 2.5f;
			acceleration = 170;
		}
		if(enduranceBar.activeInHierarchy)
			enduranceBar.transform.Find("Endurance").localScale = new Vector3(endurance / 100f, 1);
		//Debug.Log(endurance);
	}

	private void HandleMovement()
    {
		body.AddForce(direction * body.mass * speed * acceleration);

		if (Mathf.Abs(body.velocity.x) > speed)
		{
			body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * speed, body.velocity.y);
		}

		if (Mathf.Abs(body.velocity.y) > speed)
		{
			body.velocity = new Vector2(body.velocity.x, Mathf.Sign(body.velocity.y) * speed);
		}
	}

	private void Update()
	{
		direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		//LookAtCursor();
	}

	//private void HandleDodgeSliding()
	//   {
	//	transform.position += slideDir * slideSpeed * Time.fixedDeltaTime;

	//	slideSpeed -= slideSpeed * 7f * Time.fixedDeltaTime;

	//	if (slideSpeed < 5f)
	//		state = State.Normal;
	//   }
	//private void HandleDodge()
	//   {
	//	if (Input.GetKeyDown(KeyCode.Space) && state == State.Normal)
	//       {
	//		//slideDir = (UtilsClass.GetMouseWorldPosition() - transform.position).normalized;
	//		slideDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	//		slideSpeed = 100f;

	//		state = State.Dodge;
	//	}
	//}

	//private void LookAtCursor()
	//{
	//	Vector3 lookPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
	//	lookPos = lookPos - transform.position;
	//	float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg - CamOffset;
	//	transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	//}

}
