using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StressManager : MonoBehaviour
{
    [Header("Script Reference")]
    [SerializeField] private TaxiCab taxiCab;

    [Header("UI References")]
    [SerializeField] private Slider stressBar;
    [SerializeField] private TextMeshProUGUI stressText;

    private float maxStress = 100f;
    private float collisionStress = 20f;
    private float driveStress = 5f;
    private float recoveryHealth = 0.5f;
    private float currentStress = 0f;
    private Rigidbody carRb;
    private MissionManager missionManager;
    private bool isMissionActive = false;

	private void Start()
	{
        missionManager = FindObjectOfType<MissionManager>();
		carRb = FindObjectOfType<WheelController>().GetComponent<Rigidbody>();
        stressBar.maxValue = maxStress;
        stressBar.value = 0f;
        stressBar.gameObject.SetActive(false);
        stressBar.interactable = false;
	}

	private void Update()
	{
        UpdateStress();
	}

	public void BeginStressActivity()
    {
        isMissionActive = true;
        stressBar.value = 0f;
        currentStress = 0f;
        stressBar.gameObject.SetActive(true);
		stressText.text = "STRESS BAR";
        stressText.gameObject.SetActive(true);
	}

    public void StopStressActivity()
    {
        isMissionActive = false;
        stressBar.gameObject.SetActive(false);
        stressText.gameObject.SetActive(false);
    }

    private void UpdateStress()
    {
		if (!isMissionActive || carRb == null) { return; }

		float speed = carRb.velocity.magnitude * 3.6f;
		float angularSpeed = carRb.angularVelocity.magnitude;

		currentStress = Mathf.Clamp(currentStress, 0f, maxStress);
        stressBar.value = currentStress;

        if (speed > 15f || angularSpeed > 5f)
        {
            currentStress += driveStress * Time.deltaTime;
        }
        if (currentStress >= maxStress)
        {
			StopStressActivity();
            taxiCab.StressedOut();
		}
        else
        {
			currentStress = Mathf.Clamp(currentStress - recoveryHealth * Time.deltaTime, 0f, maxStress);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!isMissionActive) return;

        if(collision.relativeVelocity.magnitude > 5f)
		{
			currentStress += collisionStress;
			currentStress = Mathf.Clamp(currentStress, 0f, maxStress);
			stressBar.value = currentStress;

			if (currentStress >= maxStress)
			{
				StopStressActivity();
                taxiCab.StressedOut();
			}
		}
	}
}
