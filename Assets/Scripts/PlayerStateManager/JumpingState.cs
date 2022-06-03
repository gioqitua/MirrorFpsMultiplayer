using System.Collections;
using UnityEngine;

public class JumpingState : BaseState
{
    public override void EnterState(PlayerFSM player)
    {
        player.animationManager.JumpingAnimation();
        player.SwitchState(player.idleState);
    }

    public override void UpdateState(PlayerFSM player)
    {

    }

}
