using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
	[SerializeField] Transform cam;
	CharacterController controller;
	Animator anim;	

	private float speed = 1.7f;
	private float turnSmoothTime = 0.1f;
	private float turnSmoothVelocity;

	private void Start()
	{
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
	}
	void Update()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");		

		Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

		if (direction.magnitude >= 0.1f) anim.SetBool("isWalking", true);		
		else anim.SetBool("isWalking", false);	

		if (direction.magnitude >= 0.1f)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);

			Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

			controller.Move(moveDir.normalized * speed * Time.deltaTime);
		}
	}
}
