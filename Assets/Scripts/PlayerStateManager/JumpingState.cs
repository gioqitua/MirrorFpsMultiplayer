public class JumpingState : BaseState
{
    public override void EnterState(PlayerFSM player)
    {
        player.animationManager.JumpingAnimation();

    }

    public override void UpdateState(PlayerFSM player)
    {
        if (player.inputManager.IsRunning()) ExitState(player, player.runningState);
        if (player.inputManager.IsMoving()) ExitState(player, player.walkingState);
        else
        {
            ExitState(player, player.idleState);
        }

    }
    void ExitState(PlayerFSM player, BaseState newState)
    {
        player.SwitchState(newState);
    }

}
