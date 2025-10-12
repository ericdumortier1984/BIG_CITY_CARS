using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float startTimeInMinutes;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    private bool isBlinking = false;
    private Coroutine blinkingTime;

    private bool isTimeRunning = false;
    private float currentTime;

    public bool IsTimeRunning { get { return isTimeRunning; } }
    public bool IsTimeUp { get { return currentTime <= 0f; } }

    private void Start()
    {
        currentTime = startTimeInMinutes * 60f;
    }


    private void Update()
    {
        if (!isTimeRunning) { return; }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0f;
            isTimeRunning = false;
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format($"TIME: {minutes:00}:{seconds:00} ");

        if (currentTime <= 30f && !isBlinking)
        {
            isBlinking = true;
            blinkingTime = StartCoroutine(BlinkingCoroutine());
        }
    }

    public void StartTimer()
    {
        isTimeRunning = true;
        timerText.gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        isTimeRunning = false;
        timerText.gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        currentTime = startTimeInMinutes * 60f;
        UpdateTimerUI();
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    private IEnumerator BlinkingCoroutine()
    {
		Color normalColor = new Color(0xF2 / 255f, 0xE2 / 255f, 0x44 / 255f);
		while (isBlinking)
        {
            timerText.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            timerText.color = normalColor;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
