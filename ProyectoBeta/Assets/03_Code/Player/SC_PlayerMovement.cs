using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class SC_PlayerMovement : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 720f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundedGravity = -2f; // small downward force to keep grounded

    CharacterController characterController;
    InputAction moveAction;
    float verticalVelocity;

    [SerializeField] InputActionReference moveActionReference; // Optional: assign action from .inputactions asset

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.name = $"Player_{OwnerClientId}_NetId_{NetworkObjectId}";

        Debug.Log($"[{name}] NetId={NetworkObjectId} OwnerId={OwnerClientId} LocalId={NetworkManager.Singleton.LocalClientId} IsOwner={IsOwner}");

        AddCharacterController();
        AddMoveAction();
        EnableMoveAction();

        if (!IsOwner)
        {
            enabled = false;
        }
    }
    
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        DisableMoveAction();
    }

    void Update()
    {
        if (!IsOwner) return;

        Vector2 input = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;

        // Horizontal move (XZ)
        Vector3 horizontal = new Vector3(input.x, 0f, input.y);
        if (horizontal.sqrMagnitude > 1f) horizontal.Normalize();
        horizontal *= moveSpeed;

        // Gravity
        bool isGrounded = characterController != null && characterController.isGrounded;
        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedGravity;
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 motion = new Vector3(horizontal.x, verticalVelocity, horizontal.z) * Time.deltaTime;

        if (characterController != null)
        {
            characterController.Move(motion);
        }
        else
        {
            transform.Translate(motion, Space.World);
        }

        // Face movement direction (ignore vertical)
        Vector3 faceDir = new Vector3(horizontal.x, 0f, horizontal.z);
        if (faceDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(faceDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    private void AddCharacterController()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void AddMoveAction()
    {
        if (moveAction == null)
        {
            // Prefer referenced action from Input Actions asset if provided
            if (moveActionReference != null)
            {
                moveAction = moveActionReference.action;
            }
        }
        else
        {
            // Fallback: define bindings in code
            moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
        }
    }

    private void EnableMoveAction()
    {
        moveAction.Enable();
    }
    
    private void DisableMoveAction()
    {
        moveAction.Disable();
    }
}
