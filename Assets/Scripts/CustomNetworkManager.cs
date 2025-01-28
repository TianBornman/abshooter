using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : NetworkBehaviour
{
	public NetworkManager manager;

	private void Awake()
	{
		if (!IsServer)
			return;

		manager.OnClientConnectedCallback += OnClientConnect;
	}

	public void OnClientConnect(ulong id)
	{

	}
}
