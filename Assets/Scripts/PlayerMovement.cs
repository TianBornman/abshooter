using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    // Stats
    public float speed;

    // Refs
    private Player player;
    private Animator animator;
    private CharacterController controller;

    private Vector2 movement;

    private NetworkVariable<float> x = new(0, writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> z = new(0, writePerm: NetworkVariableWritePermission.Owner);

	private void Awake()
	{
		player = GetComponent<Player>();
		animator = GetComponentInChildren<Animator>();
		controller = GetComponentInChildren<CharacterController>();
	}

	public override void OnNetworkSpawn()
	{
        if (IsOwner)
            SetPlayerSpawnPointServerRpc();

		base.OnNetworkSpawn();
    }

    private void Update()
    {
        // Update animation on other clients
        animator.SetFloat("X", x.Value);
        animator.SetFloat("Z", z.Value);

        if (IsOwner && IsClient && player.alive.Value)
            HandleClientInput();
    }

    private void HandleClientInput()
    {
        CheckEmote();
        CheckRotation();
        CheckMovement();
    }

    private void CheckEmote()
    {
        // Detect emote input
        if (Input.GetKeyDown(KeyCode.Alpha0))
            EmoteServerRpc(0);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EmoteServerRpc(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            EmoteServerRpc(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            EmoteServerRpc(3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            EmoteServerRpc(4);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            EmoteServerRpc(5);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            EmoteServerRpc(6);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            EmoteServerRpc(7);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            EmoteServerRpc(8);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            EmoteServerRpc(9);
    }

    [ServerRpc]
    private void EmoteServerRpc(float number)
    {
        Emote(number);
        EmoteClientRpc(number);
    }

    [ClientRpc]
    private void EmoteClientRpc(float number)
    {
        Emote(number);
    }

    private void Emote(float number)
    {
        animator.SetBool("Emote", true);
        animator.SetFloat("EmoteNumber", number);
    }

    [ServerRpc]
    private void StopEmoteServerRpc()
    {
        StopEmote();
        StopEmoteClientRpc();
    }

    [ClientRpc]
    private void StopEmoteClientRpc()
    {
        StopEmote();
    }

    private void StopEmote()
    {
        animator.SetBool("Emote", false);
    }

    private void CheckRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;

            // Calculate the direction from the player to the target position
            Vector3 direction = targetPosition - transform.position;

            // Set y-axis only to rotate horizontally
            direction.y = 0;

            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    private void CheckMovement()
    {
        movement = Vector2.zero;

        // Detect movement input
        if (Input.GetKey(KeyCode.W))
            movement += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            movement += Vector2.down;
        if (Input.GetKey(KeyCode.D))
            movement += Vector2.right;
        if (Input.GetKey(KeyCode.A))
            movement += Vector2.left;

        if (movement != Vector2.zero)
            StopEmoteServerRpc();

        // Normalize and scale movement
        Vector3 inputDirection = new Vector3(movement.x, 0f, movement.y).normalized;
        Vector3 worldMovement = inputDirection * speed * Time.deltaTime;

        // Move the character in the world space
        controller.Move(worldMovement);
        // Apply gravity, this is very sus, i had no time :(
        controller.Move(new Vector3(0, -9.8f) * Time.deltaTime);

        // Calculate local movement relative to the character's rotation
        Vector3 localMovement = transform.InverseTransformDirection(inputDirection);

        // Update animation parameters based on local movement
        x.Value = localMovement.x;
        z.Value = localMovement.z;
    }

    [ServerRpc]
    private void SetPlayerSpawnPointServerRpc()
    {
        var spawnPoints = GameObject.FindGameObjectWithTag("SpawnPoints");
        var roll = Random.Range(0, spawnPoints.transform.childCount);

        SetPlayerSpawnPoint(spawnPoints.transform.GetChild(roll).position);
        SetPlayerSpawnPointClientRpc(spawnPoints.transform.GetChild(roll).position);
    }

    [ClientRpc]
    private void SetPlayerSpawnPointClientRpc(Vector3 position)
    {
        SetPlayerSpawnPoint(position);
    }

    private void SetPlayerSpawnPoint(Vector3 position)
    {
        transform.position = position;
    }
}

