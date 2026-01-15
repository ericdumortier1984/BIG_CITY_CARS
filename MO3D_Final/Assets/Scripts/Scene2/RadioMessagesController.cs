using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadioMessagesController : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] private GameObject radioPanel;
	[SerializeField] private TextMeshProUGUI radioText;

	[Header("Settings")]
	[SerializeField] private float messageInterval; 
	[SerializeField] private float typeSpeed;   
	[TextArea]
	[SerializeField] private string[] messages;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip typpingSFX;

	private int currentMessageIndex = 0;

	public void StartRadio()
	{
		radioPanel.SetActive(true);
		if (radioText != null) StartCoroutine(RadioRoutine());
	}

	public void StopRadio()
	{
		StopAllCoroutines();
		radioText.text = "";
	}

	private IEnumerator RadioRoutine()
	{
		while (currentMessageIndex < messages.Length)
		{
			yield return ShowMessage(messages[currentMessageIndex]);
			currentMessageIndex++;
			yield return new WaitForSeconds(messageInterval);
		}
	}

	private IEnumerator ShowMessage(string message)
	{
		radioText.text = "";

		foreach (char letter in message)
		{
			radioText.text += letter;
			AudioManager.Instance.PlaySFX(typpingSFX);
			yield return new WaitForSeconds(typeSpeed);
		}

		yield return new WaitForSeconds(5f);
	}
}
