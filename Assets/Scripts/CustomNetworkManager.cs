using Unity.Netcode;

public class CustomNetworkManager : NetworkBehaviour
{
    public NetworkManager manager;

    // Menu Input
    public string playerName;

    public void RespawnAllClients()
    {
        if (!IsServer)
            return;

        foreach (var client in manager.ConnectedClients)
            client.Value.PlayerObject.GetComponent<Player>().RespawnServer();
    }
}
