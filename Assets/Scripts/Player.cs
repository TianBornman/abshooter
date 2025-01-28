using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
	// Stats
	public float maxHp;

	private NetworkVariable<float> hp = new();
	public NetworkVariable<Color> primaryColor = new();
	public NetworkVariable<Color> secondaryColor = new();

	private void Awake()
	{
		primaryColor.OnValueChanged += UpdatePrimaryColor;
		secondaryColor.OnValueChanged += UpdateSecondaryColor;
	}

	private void Start()
	{
		if (IsServer)
		{
			hp.Value = maxHp;
			var colorPair = ColorHelper.ColorPairs[Random.Range(0, ColorHelper.ColorPairs.Count)];
			primaryColor.Value = colorPair.Key;
			secondaryColor.Value = colorPair.Value;
		}

		if (IsLocalPlayer)
		{
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().target = transform;
		}

		var renderers = GetComponentsInChildren<Renderer>();
		renderers[0].material.color = primaryColor.Value;
		renderers[1].material.color = secondaryColor.Value;
	}

	private void UpdatePrimaryColor(Color prevColor, Color newColor)
	{
		// Update the model colors
		var renderers = GetComponentsInChildren<Renderer>();
		renderers[0].material.color = newColor;
	}	
	
	private void UpdateSecondaryColor(Color prevColor, Color newColor)
	{
		// Update the model colors
		var renderers = GetComponentsInChildren<Renderer>();
		renderers[1].material.color = newColor;
	}

	public void Damage(int damage)
	{
		hp.Value -= damage;

		if (hp.Value <= 0)
			Die();
	}

	public void Die()
	{
		if (IsServer)
			Destroy(gameObject);
	}
}
