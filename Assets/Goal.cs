using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Animator ReadyAnimator;

    public void SetRady()
    {
        ReadyAnimator.SetTrigger("ready");
    }
}
