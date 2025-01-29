using Unity.Netcode;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
	// Refs
	private Player player;
	public GameObject bullet;
	public Transform spawnPoint;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
	{
		if (IsOwner && Input.GetKeyDown(KeyCode.Mouse0) && player.alive.Value)
			ShootServerRpc();
	}

	[ServerRpc]
	private void ShootServerRpc()
	{
		GameObject spawnedBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
		spawnedBullet.GetComponent<NetworkObject>().Spawn();
	}
}
