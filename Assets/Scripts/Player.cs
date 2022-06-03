using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    [SyncVar][SerializeField] private float speed = 350f;
    [SyncVar][SerializeField] float runningSpeedMultiplier = 1.5f;
    [SyncVar][SerializeField] float jumpForce = 3f;
    [SerializeField] public Camera playerCamera;
    [SerializeField] CharacterController characterController;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] internal PlayerCameraSettings camSettings;
    internal CapsuleCollider capsuleCollider;
    const float distanceToGround = 1.5f;

   
    internal bool isGrounded()
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
    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        camSettings = playerCamera.GetComponent<PlayerCameraSettings>();

        capsuleCollider = GetComponent<CapsuleCollider>();

        if (isLocalPlayer)
        {
            SetPlayerControlls();
            TurnOnCamera();
        }
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
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

    [Command]
    public void CmdMovePlayer(Vector3 movementDirection, bool IsRunning)
    {
        RpcMovePlayer(movementDirection, IsRunning);
    }

    [ClientRpc]
    private void RpcMovePlayer(Vector3 movementDirection, bool IsRunning)
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

    [Command]
    internal void CmdSetRotation(float yAxis, float xAxis)
    {
        RpcSetRotation(yAxis, xAxis);
    }

    [ClientRpc]
    private void RpcSetRotation(float yAxis, float xAxis)
    {
        transform.localRotation = Quaternion.Euler(0, yAxis * rotationSpeed, 0);

        playerCamera.transform.localRotation = Quaternion.Euler(xAxis * rotationSpeed, 0, 0);
    }
}