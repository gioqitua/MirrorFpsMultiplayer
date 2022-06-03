public class RunningState : BaseState
{
    public override void EnterState(PlayerFSM player)
    {
        player.animationManager.RunningAnimation(true);
    }

    public override void UpdateState(PlayerFSM player)
    { 
        if (!player.inputManager.IsRunning()) ExitState(player, player.walkingState);
    }
    void ExitState(PlayerFSM player, BaseState newState)
    {
        player.animationManager.RunningAnimation(false);
        player.SwitchState(newState);
    }
}
