using UnityEngine;

public class SirenLight : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Renderer prefabRenderer;
	[SerializeField] private Color colorA;
	[SerializeField] private Color colorB;
	[SerializeField] private float speed;

	private Material prefabMaterial;

	void Start()
	{
		prefabMaterial = prefabRenderer.material;
	}

	void Update()
	{
		float interval = Mathf.PingPong(Time.time * speed, 1f);
		prefabMaterial.color = Color.Lerp(colorA, colorB, interval);
		prefabMaterial.SetColor("_EmissionColor", prefabMaterial.color * 2f);
		DynamicGI.SetEmissive(prefabRenderer, prefabMaterial.GetColor("_EmissionColor"));
	}
}

