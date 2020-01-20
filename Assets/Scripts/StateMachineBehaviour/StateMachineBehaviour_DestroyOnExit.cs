using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBehaviour_DestroyOnExit : StateMachineBehaviour {
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //Destroy(animator.gameObject);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Destroy(animator.gameObject);
        
    }
}
