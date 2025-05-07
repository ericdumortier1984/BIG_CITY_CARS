using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SocialLinkManager : MonoBehaviour
{
	public void YouTubelink(string mLink)
	{
		Application.OpenURL(mLink);
	}

	public void GitHubLink(string mLink)
	{
		Application.OpenURL(mLink); 
	}

	public void Itch_iolink(string mLink)
	{
		Application.OpenURL(mLink);
	}
}
