using UnityEngine;
public class SetTriggerBehaviour : StateMachineBehaviour
{
    PlayerController playerController;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerController = animator.GetComponent<PlayerController>();
        // 여기에 대쉬하는 함수 호출
        playerController.OnDashInputAction();
    }
}
