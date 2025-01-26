using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; private set; }

	// Public refs
	public NetworkManager networkManager;
	public UIDocument start;

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
			networkManager.StartClient();
			start.gameObject.SetActive(false);
		};

		start.rootVisualElement.Q<Button>("Solo").clicked += () =>
		{
			networkManager.StartHost();
			start.gameObject.SetActive(false);
		};
	}
}
