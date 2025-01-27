using Unity.Netcode;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
	public GameObject bullet;
	public Transform spawnPoint;

	private void Update()
	{
		if (IsOwner && Input.GetKeyDown(KeyCode.Mouse0))
		{
			ShootServerRpc();
		}
	}

	[ServerRpc]
	private void ShootServerRpc()
	{
		GameObject spawnedBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
		spawnedBullet.GetComponent<NetworkObject>().Spawn();
	}
}
