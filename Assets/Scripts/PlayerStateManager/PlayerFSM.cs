using Mirror;
using UnityEngine;

public class PlayerFSM : NetworkBehaviour
{
    public AnimationManager animationManager;
    BaseState currentState;
    public IdleState idleState = new IdleState();
    public JumpingState jumpingState = new JumpingState();
    public WalkingState walkingState = new WalkingState();
    public RunningState runningState = new RunningState();
    internal InputManager inputManager;
    internal PlayerCameraSettings playerCamera;

    private void Start()
    {
        animationManager = GetComponent<AnimationManager>();

        inputManager = InputManager.Instance;

        playerCamera = GetComponentInChildren<PlayerCameraSettings>();

        SwitchState(idleState);
    }
    private void Update()
    {
        if (!isLocalPlayer) return;

        if (inputManager.Jumping()) SwitchState(jumpingState);

        currentState.UpdateState(this);
    }
    public void SwitchState(BaseState newState)
    {
        currentState = newState;
        newState.EnterState(this);
    }
}
