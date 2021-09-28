using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 在外部命名信号的时候需要与触发器的名称相对应
/// 由于挂载在状态机的某个动画上，建议不要删除debug或注销debug
/// </summary>
public class FSMClearSignals : StateMachineBehaviour
{
    public string[] ClearAtEnter;//清除trigger本身机制导致的多次信号
    public string[] ClearAtExit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.LogFormat("执行该脚本的animattor是{0},承载其的对象是{1}", animator, animator.gameObject.name);
        foreach (var signal in ClearAtEnter)
        {
            animator.ResetTrigger(signal);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var signal in ClearAtExit)
        {
            animator.ResetTrigger(signal);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
