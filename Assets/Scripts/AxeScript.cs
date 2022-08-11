using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
	[SerializeField] private TrailRenderer trail;

	public bool activated;
	public float rotationSpeed;

	void Update()
	{
		if (activated)
		{
			trail.enabled = true;
			transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
		}
		else
		{
			trail.enabled = false;
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		activated = false;
		GetComponent<Rigidbody>().isKinematic = true;
	}
}