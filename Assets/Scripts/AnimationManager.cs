using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Player player;
    [SerializeField] Animator anime;
    internal void SetPlayer(Player _player)
    {
        player = _player;
        anime = player.anime;
    }

    public void WalkingAnimation(bool condition)
    {
        anime.SetBool("Walk", condition);
    }
    public void IdleAnimation(bool condition)
    {
        anime.SetBool("Idle", condition);
    }
    public void RunningAnimation(bool condition)
    {
        anime.SetBool("Run", condition);
    }
    public void JumpingAnimation()
    {
        anime.SetTrigger("Jump");
    }
    public void FireAnimation()
    { 
        anime.SetTrigger("Shoot"); 
    }


}
