using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBehaviour_DestroyOnExit : StateMachineBehaviour {
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //Destroy(animator.gameObject);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Destroy(animator.gameObject);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        // todo OnStateMachineExit가 호출되고 바로 OnStateMachineEnter가 호출되는 이유가 뭘까?
        //Destroy(animator.gameObject);
    }
}
