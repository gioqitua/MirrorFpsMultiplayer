public class IdleState : BaseState
{
    public override void EnterState(PlayerFSM player)
    {
        player.animationManager.IdleAnimation(true);
    }

    public override void UpdateState(PlayerFSM player)
    { 
        if (player.inputManager.IsMoving()) ExitState(player, player.walkingState); 
    }
    void ExitState(PlayerFSM player, BaseState newState)
    {
        player.playerCamera.DefaultFOV();
        player.animationManager.IdleAnimation(false);
        player.SwitchState(newState);
    }
}
