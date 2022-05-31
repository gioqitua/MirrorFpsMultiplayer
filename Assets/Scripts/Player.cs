using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    [SyncVar][SerializeField] float speed = 450f;
    [SerializeField] public Camera playerCamera;
    [SerializeField] CharacterController characterController;
    [SerializeField] float rotationSpeed = 1f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (isLocalPlayer)
        {
            SetPlayerControlls();
            TurnOnCamera();
        }
    }
   
    public override void OnStartClient()
    {
        base.OnStartClient();
        InputManager.Instance.LockCursor();
    }
    private void TurnOnCamera()
    {
        var cameraSettings = playerCamera.GetComponent<PlayerCameraSettings>();
        cameraSettings.TurnOnCamera();
        InputManager.Instance.SetCamera(cameraSettings);
    }

    private void SetPlayerControlls()
    {
        InputManager.Instance.SetPlayer(this);
    }

    [Command]
    public void CmdMovePlayer(Vector3 movementDirection)
    {
        RpcMovePlayer(movementDirection);
    }

    [ClientRpc]
    private void RpcMovePlayer(Vector3 movementDirection)
    {
        Vector3 movement = transform.TransformDirection(movementDirection) * speed * Time.deltaTime;
        characterController.SimpleMove(movement);
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