using Unity.Netcode;
using UnityEngine;

public class BulletMovement : NetworkBehaviour
{
	// Stats
	public float speed;
	public float lifetime;

	public override void OnNetworkSpawn()
	{
		if (IsServer)
			Invoke(nameof(DestroyBullet), lifetime);

		base.OnNetworkSpawn();
	}

	private void Update()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	private void DestroyBullet()
	{
		if (IsServer)
			Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!IsServer)
			return;

		if (other.TryGetComponent<Player>(out var player))
			player.Damage(1);

		Destroy(gameObject);
	}
}
