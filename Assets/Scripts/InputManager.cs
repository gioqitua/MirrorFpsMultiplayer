using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] Player player;
    [SerializeField] PlayerCameraSettings playerCamera;
    AnimationManager animator;
    private Vector3 movementDirection;
    float xAxis, yAxis;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("more than 1 InputManager in Scene");
        }
    }

    private void Update()
    {


        if (!player) return;

        Aim();

        MouseInput();

        PlayerMovementInputs();
    }

    private void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            playerCamera.AimFOV();
        }
        else
        {
            playerCamera.DefaultFOV();
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void SetPlayer(Player _player)
    {
        player = _player;
        animator = _player.GetComponent<AnimationManager>();
    }
    public void SetCamera(PlayerCameraSettings camera)
    {
        playerCamera = camera;
    }
    private void PlayerMovementInputs()
    {
        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.z = Input.GetAxis("Vertical");

        player.CmdMovePlayer(movementDirection);
        AnimationHandler();
    }

    private void AnimationHandler()
    {
         
        if (movementDirection.magnitude > 0.05f)
        {
            animator.WalkingAnimation(true);
        }
        else
        {
            animator.WalkingAnimation(false);
        }
    }

    private void MouseInput()
    {
        xAxis += Input.GetAxisRaw("Mouse X"); 
        yAxis -= Input.GetAxisRaw("Mouse Y");
        yAxis = Mathf.Clamp(yAxis, -80, 80); 
        player.CmdSetRotation(xAxis, yAxis);

    }
}
