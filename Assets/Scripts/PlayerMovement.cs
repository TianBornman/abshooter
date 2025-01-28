using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    // Stats
    public float speed;

    // Refs
    private Animator animator;
    private CharacterController controller;

    private Vector2 movement;

    private NetworkVariable<float> x = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> z = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponentInChildren<CharacterController>();
    }

    private void Update()
    {
        animator.SetFloat("X", x.Value);
        animator.SetFloat("Z", z.Value);

        if (IsOwner && IsClient)
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Emote(1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Emote(2);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            Emote(3);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            Emote(4);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            Emote(5);

        if (Input.GetKeyDown(KeyCode.Alpha6))
            Emote(6);

        if (Input.GetKeyDown(KeyCode.Alpha7))
            Emote(7);

        if (Input.GetKeyDown(KeyCode.Alpha8))
            Emote(8);

        if (Input.GetKeyDown(KeyCode.Alpha9))
            Emote(9);
    }

    private void Emote(float number)
    {
        animator.SetBool("Emote", true);
        animator.SetFloat("EmoteNumber", number);
    }

    private void CheckRotation()
    {
        // Get the mouse position in screen space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Get the point where the ray hits
            Vector3 targetPosition = hitInfo.point;

            // Calculate the direction from the player to the target position
            Vector3 direction = targetPosition - transform.position;

            // Set y-axis only to rotate horizontally
            direction.y = 0;

            // Ignore zero-length directions to avoid errors
            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    private void CheckMovement()
    {
        // Gets the input from the user
        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            movement += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            movement += Vector2.down;
        if (Input.GetKey(KeyCode.D))
            movement += Vector2.right;
        if (Input.GetKey(KeyCode.A))
            movement += Vector2.left;

        Vector3 toMove = new Vector3(movement.x, 0f, movement.y).normalized * speed * Time.deltaTime;
        toMove = transform.TransformDirection(toMove);

        // Moves the object locally, but due to Network Transform,
        // this gets synced on the server and other clients
        controller.Move(toMove);

        x.Value = controller.velocity.normalized.x;
        z.Value = controller.velocity.normalized.z;
    }
}
