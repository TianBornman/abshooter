using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
	// Stats
	public float maxHp;
	//public string playerName;

    // Refs
    private Animator animator;
	private TextMeshProUGUI nameText;

    private NetworkVariable<float> hp = new();
    public NetworkVariable<bool> alive = new(true);
    public NetworkVariable<NetworkString> playerName = new(new (string.Empty));
	private NetworkVariable<Color> primaryColor = new();
	private NetworkVariable<Color> secondaryColor = new();

	private void Awake()
	{
        animator = GetComponentInChildren<Animator>();
		nameText = GetComponentInChildren<TextMeshProUGUI>();

		playerName.OnValueChanged += UpdatePlayerName;
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

			var name = GameObject.FindGameObjectWithTag("Managers")
							 .GetComponent<CustomNetworkManager>().playerName;

			SetPlayerNameServerRpc(name);
        }

		var renderers = GetComponentsInChildren<Renderer>();
		renderers[0].material.color = primaryColor.Value;
		renderers[1].material.color = secondaryColor.Value;

        nameText.text = playerName.Value.Value;
    }

	private void LateUpdate()
    {
        nameText.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                       Camera.main.transform.rotation * Vector3.up);
    }

    private void UpdatePlayerName(NetworkString prevName, NetworkString newName)
    {
		nameText.text = newName.Value;
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
            DieServer();
	}

	#region Name

	[ServerRpc]
	private void SetPlayerNameServerRpc(string name)
	{
        playerName.Value = new NetworkString(name);
    }

	#endregion

	#region Die

	private void DieServer()
	{
		Die();
        DieClientRpc();
    }

	[ClientRpc]
    private void DieClientRpc()
	{
		Die();
    }

	private void Die()
	{
		if (IsServer)
			alive.Value = false;

        animator.SetBool("Dead", true);
    }

    #endregion

    #region Respawn

    public void RespawnServer()
	{
		Respawn();
		RespawnClientRpc();
    }

	[ClientRpc]
    private void RespawnClientRpc()
	{
		Respawn();
    }

    private void Respawn()
    {
		if (IsServer)
		{
			alive.Value = true;
			hp.Value = maxHp;
		}

        animator.SetBool("Dead", false);
    }

    #endregion
}
