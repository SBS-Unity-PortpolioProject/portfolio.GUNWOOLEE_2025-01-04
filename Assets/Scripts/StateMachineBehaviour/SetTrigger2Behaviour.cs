using UnityEngine;
public class SetTrigger2Behaviour : StateMachineBehaviour
{   
    PlayerController playerController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerController = animator.gameObject.GetComponent<PlayerController>();
        
        playerController.OnDashInputAction();
        playerController.OnDashFlipInputAction();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerController.OnDashFlipInputAction();
    }
    
}
