using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
	[Header("Axe Attributes")]
	[SerializeField] private AxeScript axeScript;	
	[SerializeField] private Transform axe;
	[SerializeField] private Transform hand;
	private Rigidbody axeRb;
	private Animator animator;
	private int isAimingHash;
	private float throwForce = 50;

	[Header("Return Axe")]
	[SerializeField] private Transform target;
	[SerializeField] private Transform curve_point;
	private Vector3 old_pos;
	private bool isReturning = false;
	private float time = 0.0f;
	private bool hasAxe = true;

	[Header("Axe Effects")]
	public ParticleSystem catchParticle;
		
	void Start()
	{
		Cursor.visible = false;
		animator = GetComponent<Animator>();
		axeRb = axe.GetComponent<Rigidbody>();
		axeScript = axe.GetComponent<AxeScript>();
		isAimingHash = Animator.StringToHash("isAiming");		
	}
	
	void Update()
	{
		Aim();

		if (isReturning)
		{
			if(time < 1.0f)
			{
				axeScript.activated = true;
				axe.position = getBQCPoint(time, old_pos, curve_point.position, hand.position);				
				time += Time.deltaTime * 1.5f;
			}
			else
			{
				ResetAxe();
			}
		}
	}

	public void ThrowAxe()
	{
		hasAxe = false;
		isReturning = false;
		axeScript.activated = true;
		axe.parent = null;
		axe.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
		axeRb.isKinematic = false;
		axeRb.AddForce(Camera.main.transform.TransformDirection(Vector3.forward) * throwForce, ForceMode.Impulse);		
	}

	public void ReturnAxe()
	{		
		old_pos = axe.position;
		isReturning = true;
		hasAxe = true;		
		axeRb.velocity = Vector3.zero;
		axeRb.isKinematic = true;		
	}

	public void ResetAxe()
	{
		time = 0.0f;
		isReturning = false;
		axeScript.activated = false;
		axe.parent = hand;
		axe.position = target.position;
		axe.rotation = target.rotation;
	}

	Vector3 getBQCPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
		Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
		return p;
	}
	
	void Aim()
	{		
		bool isAiming = animator.GetBool(isAimingHash);

		if (Input.GetMouseButtonDown(1) && !isAiming )
		{
			animator.SetBool(isAimingHash, true);
			if(!hasAxe) ReturnAxe();
		}

		if (Input.GetMouseButtonUp(1) && isAiming) animator.SetBool(isAimingHash, false);			

		if (hasAxe)
		{
			if ((Input.GetMouseButtonDown(0) && isAiming) && !isReturning) animator.SetTrigger("Throw");					
		}		
	}
}
