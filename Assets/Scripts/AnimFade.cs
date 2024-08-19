using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFade : MonoBehaviour
{
    public Animator transition;

    public void StartAnim() {
        // Play animation, calling the start trigger
        transition.SetTrigger("Start");
    }
}
