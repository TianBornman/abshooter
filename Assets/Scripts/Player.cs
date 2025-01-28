using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
	// Stats
	public float maxHp;

	private NetworkVariable<float> hp = new();

	private void Start()
	{
		if (IsServer)
			hp.Value = maxHp;

		if (IsLocalPlayer)
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().target = transform;
    }

	public void Damage(int damage)
	{
		hp.Value -= damage;

		if (hp.Value <= 0)
			Die();
	}

	public void Die()
	{
		if (IsServer)
			Destroy(gameObject);
	}
}
