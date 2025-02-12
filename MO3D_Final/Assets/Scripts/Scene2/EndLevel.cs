using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{

	public Text mScoreText;

	private void Start()
	{
		int mScore = PlayerPrefs.GetInt("SCORE", 0);
		mScoreText.text = "SCORE" + mScore.ToString();
	}
}
