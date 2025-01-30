using Unity.Netcode;

public class CustomNetworkManager : NetworkBehaviour
{
    public NetworkManager manager;

    // Menu Input
    public string playerName;

    private void Start()
    {
        InvokeRepeating("RespawnAllClients", 0, 30);
    }

    public void RespawnAllClients()
    {
        if (!IsServer)
            return;

        foreach (var client in manager.ConnectedClients)
            client.Value.PlayerObject.GetComponent<Player>().RespawnServer();
    }
}
