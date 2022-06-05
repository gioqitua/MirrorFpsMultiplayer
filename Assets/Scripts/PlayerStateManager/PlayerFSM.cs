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
    public Player player { get; private set; }
    internal void SetPlayer(Player _player)
    {
        player = _player;

        animationManager = player.animationManager;

        inputManager = player.inputManager;

        playerCamera = player.camSettings;

        SwitchState(idleState);
    }

    private void Update()
    {
        if (!player) return;
        if (!isLocalPlayer) return;

        currentState.UpdateState(this);
    }
    public void SwitchState(BaseState newState)
    {
        currentState = newState;
        newState.EnterState(this);
    }
}
