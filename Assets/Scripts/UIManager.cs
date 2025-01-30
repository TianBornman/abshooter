using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
	// Public refs
	public NetworkManager networkManager;
	public CustomNetworkManager customNetworkManager;
	public UnityTransport transport;

	public UIDocument start;
	public UIDocument joinModal;
	public UIDocument serverTools;

	private void Awake()
	{
		BindStart();
		BindJoinModal();
		BindServerTools();
    }

	private void BindStart()
	{
		start.rootVisualElement.Q<Button>("Host").clicked += () =>
		{
			networkManager.StartServer();
			start.gameObject.SetActive(false);
			serverTools.rootVisualElement.Q<Button>("Respawn").RemoveFromClassList("hidden");
        };

		start.rootVisualElement.Q<Button>("Join").clicked += () =>
		{
			joinModal.rootVisualElement.Q<VisualElement>("JoinModal").RemoveFromClassList("hidden");
		};

		start.rootVisualElement.Q<Button>("Solo").clicked += () =>
		{
			networkManager.StartHost();
			start.gameObject.SetActive(false);
		};
	}

	private void BindJoinModal()
	{
		var ipText = joinModal.rootVisualElement.Q<TextField>("IP");
		ipText.value = transport.ConnectionData.Address;

		ipText.RegisterValueChangedCallback(evt =>
		{
			var connectionData = transport.ConnectionData;
			connectionData.Address = evt.newValue;
			transport.ConnectionData = connectionData;
		});

		var portText = joinModal.rootVisualElement.Q<TextField>("Port");
		portText.value = transport.ConnectionData.Port.ToString();

		portText.RegisterValueChangedCallback(evt =>
		{
			var connectionData = transport.ConnectionData;
			connectionData.Port = ushort.Parse(evt.newValue);
			transport.ConnectionData = connectionData;
		});

		joinModal.rootVisualElement.Q<TextField>("Name").dataSource = customNetworkManager;

        joinModal.rootVisualElement.Q<Button>().clicked += () =>
		{
			if (string.IsNullOrEmpty(customNetworkManager.playerName))
				return;

			networkManager.StartClient();
			joinModal.gameObject.SetActive(false);
			start.gameObject.SetActive(false);
		};
	}

	private void BindServerTools()
	{
		serverTools.rootVisualElement.Q<Button>("Respawn").clicked += () =>
		{
            customNetworkManager.RespawnAllClients();
        };
	}
}
