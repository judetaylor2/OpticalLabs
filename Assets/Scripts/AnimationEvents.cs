using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Animator anim;
    void SetBool(string animationName)
    {
        anim.SetBool(animationName, false);
    }
}
