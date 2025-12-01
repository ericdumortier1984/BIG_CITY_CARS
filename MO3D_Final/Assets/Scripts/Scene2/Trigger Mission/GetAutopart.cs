using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAutopart : MonoBehaviour
{
	[Header("ID")]
	public int autopartID;

	[Header("FX")]
	[SerializeField] private ParticleSystem getAutopartParticle;

	private GetTheAutoparts getTheAutoparts;

	private void Start()
	{
		getTheAutoparts = FindObjectOfType<GetTheAutoparts>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("4x4"))
		{
			getAutopartParticle.transform.position = transform.position;
			getAutopartParticle.gameObject.SetActive(true);
			getAutopartParticle.Play();

			if (getTheAutoparts != null)
			{
				getTheAutoparts.CollectAutopart(this);
			}
			gameObject.SetActive(false);
		}
	}
}
