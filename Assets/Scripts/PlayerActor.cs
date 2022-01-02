using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour
{
    public GameObject model;
    public PlayerInput PI;
    public float walkSpeed = 1.4f;
    public float runMuliplier = 2.0f;
    public float jumpVelocity = 5f;
    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 PlanarVec;
    private Vector3 thrustVec;//冲量，保证跳跃

    private bool lockPlanar = false;//锁死平面

    void Awake()
    {
        anim = model.GetComponentInChildren<Animator>();
        PI = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Forward", PI.Dmag * Mathf.Lerp(anim.GetFloat("Forward"), (PI.run ? 2.0f : 1.0f), 0.1f));//做插值方便加速显得均匀
        if (rigid.velocity.magnitude > 5.0f)
        {
            anim.SetTrigger("Roll");
        }
        if (PI.jump)
        {
            anim.SetTrigger("Jump");
        }
        if (PI.Dmag > 0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, PI.Dvector3, 0.3f);//做插值方便动画播放显得均匀
        }
        if (lockPlanar == false)
        {
            PlanarVec = PI.Dmag * model.transform.forward * walkSpeed * (PI.run ? runMuliplier : 1.0f);

        }
    }
    private void FixedUpdate()
    {
        //rigid.position += PlanarVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(PlanarVec.x, rigid.velocity.y, PlanarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }
    /// <summary>
    /// 该方法由状态机中的FSMOnEnter调用
    /// </summary>
    private void OnJumpEnter()
    {
        PI.InputEnable = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, 5, 0);
    }
    /// <summary>
    /// 该方法由状态机中的FSMOnExit调用
    /// </summary>
    //private void OnJumpExit()
    //{
    //    PI.InputEnable = true;
    //    lockPlanar = false;

    //}
    private void IsGround()
    {
        print("IsGround");
        anim.SetBool("IsGround", true);
    }
    private void IsNotGround()
    {
        print("IsNotGround");
        anim.SetBool("IsGround", false);

    }
    void OnGroundEnter()
    {
        Debug.Log("一致性");
        PI.InputEnable = true;
        lockPlanar = false;
    }
    /// <summary>
    /// 禁用进入坠落状态可以操作移动方法
    /// </summary>
    void OnFallEnter()
    {
        PI.InputEnable = false;
        lockPlanar = true;

    }
}
