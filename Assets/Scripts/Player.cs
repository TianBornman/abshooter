using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
	// Stats
	public float maxHp;

    // Refs
    private Animator animator;
	private TextMeshProUGUI nameText;

    private NetworkVariable<float> hp = new();
    public NetworkVariable<bool> alive = new(true);
    public NetworkVariable<FixedString32Bytes> playerName = new();
	private NetworkVariable<Color> primaryColor = new();
	private NetworkVariable<Color> secondaryColor = new();

	private void Awake()
	{
        animator = GetComponentInChildren<Animator>();
		nameText = GetComponentInChildren<TextMeshProUGUI>();

		playerName.OnValueChanged += PlayerNameChange;
        primaryColor.OnValueChanged += UpdatePrimaryColor;
		secondaryColor.OnValueChanged += UpdateSecondaryColor;
	}

    public override void OnNetworkSpawn()
	{
        base.OnNetworkSpawn();

		UpdatePlayerName(playerName.Value);
        UpdateRendererColor(0, primaryColor.Value);
        UpdateRendererColor(1, secondaryColor.Value);

        if (!IsOwner)
			return;

		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().target = transform;

		var name = GameObject.FindGameObjectWithTag("Managers")
							.GetComponent<CustomNetworkManager>().playerName;

		SetupPlayerServerRpc(name);
    }

	private void LateUpdate()
    {
        nameText.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                       Camera.main.transform.rotation * Vector3.up);
    }

    #region Network Variable Callbacks

    private void PlayerNameChange(FixedString32Bytes prevName, FixedString32Bytes newName)
    {
        UpdatePlayerName(newName);
    }
	
	private void UpdatePlayerName(FixedString32Bytes name)
    {
		nameText.text = name.Value;
    }

    private void UpdatePrimaryColor(Color prevColor, Color newColor)
	{
        UpdateRendererColor(0, newColor);
    }

    private void UpdateSecondaryColor(Color prevColor, Color newColor)
    {
        UpdateRendererColor(1, newColor);
    }

    private void UpdateRendererColor(int index, Color color)
	{
        // Update the model colors
        var renderers = GetComponentsInChildren<Renderer>();
        renderers[index].material.color = color;
    }

    #endregion

	#region Name

	[ServerRpc]
	private void SetupPlayerServerRpc(string name)
	{
        hp.Value = maxHp;
        playerName.Value = name;

        var colorPair = ColorHelper.ColorPairs[Random.Range(0, ColorHelper.ColorPairs.Count)];
        primaryColor.Value = colorPair.Key;
        secondaryColor.Value = colorPair.Value;
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

    public void Damage(int damage)
    {
        hp.Value -= damage;

        if (hp.Value <= 0)
            DieServer();
    }
}
