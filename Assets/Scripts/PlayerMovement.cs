using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	// Stats
	public float speed;

	// Refs
	private Animator animator;

    private Vector2 movement;

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
	}

	private void Update()
    {
		Move();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		movement = context.ReadValue<Vector2>();
	}

	[ClientRpc]
	private void Move()
	{
		animator.SetFloat("X", movement.x);
		animator.SetFloat("Y", movement.y);

		Vector3 toMove = new Vector3(movement.x, 0f, movement.y).normalized * speed * Time.deltaTime;
		toMove = transform.TransformDirection(toMove);

		transform.position += toMove;
	}
}
