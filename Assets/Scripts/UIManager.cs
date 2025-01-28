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
		joinModal.rootVisualElement.Query<TextField>().ToList().ForEach(textField => textField.dataSource = transport.ConnectionData);

		joinModal.rootVisualElement.Q<Button>().clicked += () =>
		{
			networkManager.StartClient();
			joinModal.gameObject.SetActive(false);
			start.gameObject.SetActive(false);
		};
	}
}
