using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] Player player;
    private Vector3 movementDirection = Vector3.zero;
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
        if (!player.isLocalPlayer) return;
        Jumping();

        IsAiming();

        MouseInput();

        PlayerMovementInputs();
    }
    public Vector3 Direction()
    {
        return movementDirection;
    }
    public bool IsMoving()
    {
        if (movementDirection.magnitude > 0.05) return true;
        else return false;
    }
    public bool IsRunning()
    {
        if (Input.GetKey(KeyCode.LeftShift) && IsMoving())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool Jumping()
    {
        if (!player.isGrounded()) return false;
        if (Input.GetKey(KeyCode.Space))
        {
            player.CmdJumpPlayer();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void IsAiming()
    {
        if (Input.GetMouseButton(1))
        {
            player.camSettings.AimFOV();
        }
        else
        {
            player.camSettings.DefaultFOV();
        }
    }
    public void SetPlayer(Player _player)
    {
        player = _player;
        player.SetInpManager(this);
    }



    private void PlayerMovementInputs()
    {
        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.z = Input.GetAxis("Vertical");

        player.MovePlayer(movementDirection, IsRunning());
    }

    private void MouseInput()
    {
        xAxis += Input.GetAxisRaw("Mouse X");
        yAxis -= Input.GetAxisRaw("Mouse Y");
        yAxis = Mathf.Clamp(yAxis, -80, 80);

        player.SetRotation(xAxis, yAxis);

    }
}
