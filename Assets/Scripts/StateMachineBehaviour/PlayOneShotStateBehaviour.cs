using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public bool playOnEnter = true;
    public bool playOnExit = false;
    public bool playAfterDelay = false;
    
    private float timeSinceEntered = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceEntered = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay)
        {
            timeSinceEntered += Time.deltaTime;
        }
    }

}
