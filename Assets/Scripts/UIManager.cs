using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; private set; }

	// Public refs
	public NetworkManager networkManager;
	public UnityTransport transport;
	public UIDocument start;
	public UIDocument joinModal;

	private void Awake()
	{
		#region Singleton

		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;


		#endregion

		BindStart();
		BindJoinModal();
	}

	private void BindStart()
	{
		start.rootVisualElement.Q<Button>("Host").clicked += () =>
		{
			networkManager.StartServer();
			start.gameObject.SetActive(false);
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

		joinModal.rootVisualElement.Q<Button>().clicked += () =>
		{
			networkManager.StartClient();
			joinModal.gameObject.SetActive(false);
			start.gameObject.SetActive(false);
		};
	}
}
