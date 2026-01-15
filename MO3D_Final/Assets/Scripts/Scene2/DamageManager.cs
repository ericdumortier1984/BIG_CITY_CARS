using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DamageManager : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] private GameObject target;
	[SerializeField] private Slider healthBar;
	[SerializeField] private TextMeshProUGUI healthText;

	[Header("Health Settings")]
	[SerializeField] private float maxHealth;
	[SerializeField] private float recoveryHealth;

	[Header("FX")]
	[SerializeField] private ParticleSystem destroyedParticle;

	[Header("References")]
	[SerializeField] private Rigidbody carRb;
	[SerializeField] private MissionManager missionManager;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip damageSFX;

	// FLOATS
	private float currentHealth;

	// BOOLEANS
	private bool isDamageActive = false;
	private bool isDestroyed = false;

	// ACTIONS
	public System.Action OnGameObjectDestroyed;

	private void Start()
	{
		carRb = GetComponent<Rigidbody>();
		InitHealth();
	}

	private void Update()
	{
		if (!isDamageActive) { return; }

		if (recoveryHealth > 0 && currentHealth < maxHealth)
		{
			currentHealth = Mathf.Clamp(currentHealth + recoveryHealth * Time.deltaTime, 0f, maxHealth);
			UpdateHealthUI();
		}
	}

	public void InitHealth()
	{
		currentHealth = maxHealth;

		// BAR SETUP
		healthBar.maxValue = maxHealth;
		healthBar.value = maxHealth;
		healthBar.interactable = false;
	}

	public void BeginDamageSystem()
	{
		isDamageActive = true;
		isDestroyed = false;
		currentHealth = maxHealth;

		// BAR SETUP
		healthBar.maxValue = maxHealth;
		healthBar.value = currentHealth;
		healthBar.gameObject.SetActive(true);

		// TEXT SETUP
		healthText.text = "FURGON HEALTH BAR";
		healthText.gameObject.SetActive(true);
	}

	public void StopDamageSystem()
	{
		isDamageActive = false;
		healthBar.gameObject.SetActive(false);
		healthText.gameObject.SetActive(false);
	}

	public void TakeDamage(float damageAmount)
	{
		if (!isDamageActive || isDestroyed) { return; }

		currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0f, maxHealth);
		UpdateHealthUI();

		if (currentHealth <= 0f)
		{
			DestroyGameObject();
		}
	}

	private void UpdateHealthUI()
	{
		healthBar.value = currentHealth;
	}

	private void DestroyGameObject()
	{
		isDestroyed = true;
		destroyedParticle.Play();
		OnGameObjectDestroyed?.Invoke();
		AudioManager.Instance.PlaySFX(damageSFX);
		StopDamageSystem();
	}
}
