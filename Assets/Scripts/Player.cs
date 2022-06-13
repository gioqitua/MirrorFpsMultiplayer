using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    [SyncVar][SerializeField] private float speed = 200f;
    [SyncVar][SerializeField] float runningSpeedMultiplier = 1.5f;
    [SyncVar][SerializeField] float jumpForce = 1f;
    [SerializeField] public Camera playerCamera;
    [SerializeField] CharacterController characterController;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] internal PlayerCameraSettings camSettings;
    float distanceToGround = 1.5f;
    internal Animator anime;
    internal AnimationManager animationManager;
    internal InputManager inputManager;
    internal PlayerFSM playerFSM;


    public override void OnStartClient()
    {
        characterController = GetComponent<CharacterController>();

        camSettings = playerCamera.GetComponent<PlayerCameraSettings>();

        anime = GetComponentInChildren<Animator>();

        animationManager = GetComponent<AnimationManager>();

        playerFSM = GetComponent<PlayerFSM>();

        if (isLocalPlayer)
        {
            SetPlayerControlls();
            TurnOnCamera();
            animationManager.SetPlayer(this);
            playerFSM.SetPlayer(this);
        }
    }
    public bool isGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, distanceToGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void TurnOnCamera()
    {
        var cameraSettings = playerCamera.GetComponent<PlayerCameraSettings>();
        cameraSettings.TurnOnCamera();
    }
    private void SetPlayerControlls()
    {
        InputManager.Instance.SetPlayer(this);
    }
    internal void SetInpManager(InputManager _inputManager)
    {
        inputManager = _inputManager;
    }
    [Command]
    public void CmdJumpPlayer()
    {
        RpcJumpPlayer();

    }
    [ClientRpc]
    void RpcJumpPlayer()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + jumpForce, transform.position.z);
    }
    internal void MovePlayer(Vector3 movementDirection, bool IsRunning)
    {
        Vector3 movement;

        if (IsRunning)
        {
            movement = transform.TransformDirection(movementDirection) * speed * runningSpeedMultiplier * Time.deltaTime;
            characterController.SimpleMove(movement);
        }
        else
        {
            movement = transform.TransformDirection(movementDirection) * speed * Time.deltaTime;
            characterController.SimpleMove(movement);
        }

    }
    internal void SetRotation(float yAxis, float xAxis)
    {
        transform.localRotation = Quaternion.Euler(0, yAxis * rotationSpeed, 0);

        playerCamera.transform.localRotation = Quaternion.Euler(xAxis * rotationSpeed, 0, 0);
    }

}