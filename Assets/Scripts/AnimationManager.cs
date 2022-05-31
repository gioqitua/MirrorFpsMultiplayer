using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator anime;
    void Start()
    {
        anime = GetComponentInChildren<Animator>();
    }

    public void WalkingAnimation(bool condition)
    {
        anime.SetBool("Walking", condition);
    }
}
