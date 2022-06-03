using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Animator anime;
    void Start()
    {
        anime = GetComponentInChildren<Animator>();
    }
    public void WalkingAnimation(bool condition)
    {
        anime.SetBool("Walking", condition);
    }
    public void IdleAnimation(bool condition)
    {
        anime.SetBool("Idle", condition);
    }
    public void RunningAnimation(bool condition)
    {
        anime.SetBool("Running", condition);
    }
    public void JumpingAnimation()
    {
        anime.SetTrigger("Jump");
    }

}
