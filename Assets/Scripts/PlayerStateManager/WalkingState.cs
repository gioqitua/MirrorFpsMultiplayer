public class WalkingState : BaseState
{
    public override void EnterState(PlayerFSM player)
    {
        player.animationManager.WalkingAnimation(true);
    }

    public override void UpdateState(PlayerFSM player)
    { 
        if (!player.inputManager.IsMoving()) ExitState(player, player.idleState);

        if (player.inputManager.IsRunning()) ExitState(player, player.runningState);
    }
    void ExitState(PlayerFSM player, BaseState newState)
    {
        player.animationManager.WalkingAnimation(false);

        player.SwitchState(newState);
    }
}
