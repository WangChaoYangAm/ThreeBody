using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMove : MonoBehaviour
{
    private EnemySight m_enemySight;
    private Animator m_animator;

    //private float 最大追击距离;
    //private float 追击距离;
    [SerializeField]
    private GameObject[] 巡逻点集合;
    private Transform 初始化位置;
    private Transform 当前位置;
    private Transform 目标巡逻点;
    private NavMeshAgent meshAgent;
    private Transform[] 路径点集合;
    private int 路径索引;
    private Quaternion 目标朝向;
    private float 转向速度=10;
    private float 等待时间=3;//巡逻到目标点后等待的时间
    private float 巡逻等待状态计时器;
    private enum 敌人状态 
    { 
        原地站立,
        移动向巡逻点,
        追击,
        返回巡逻路线
    }

    void Start()
    {
        m_enemySight = GetComponent<EnemySight>();
        m_animator = GetComponent<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
        当前位置 = GetComponent<Transform>();
        路径索引 = 0;
        目标巡逻点 = 巡逻点集合[路径索引].transform;
        meshAgent.destination = 目标巡逻点.position;
        meshAgent.isStopped = false;
        meshAgent.updatePosition = true;
        meshAgent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        巡逻();
    }

    void 计算当前位置至目标位置的路径点()
    {
        //NavMeshPath 路径 = new NavMeshPath();
        //meshAgent.CalculatePath(目标巡逻点, 路径);
        //路径点集合 = new Vector3[路径.corners.Length + 2];
        //路径点集合[0] = 当前位置;
        //路径点集合[路径点集合.Length - 1] = 目标巡逻点;
    }

    void 巡逻()
    {
        if (!m_enemySight.isInSight)//如果NPC没有发现玩家
        {
            if (meshAgent.remainingDistance<0.5f)
            {
                meshAgent.isStopped=true;//距离小于0.5的时候认为它已经到达目标地点
                巡逻等待状态计时器 += Time.deltaTime;
                if (巡逻等待状态计时器 > 等待时间)
                {
                    路径索引++;
                    路径索引 %= 巡逻点集合.Length;//循环路径，通过求余实现
                    meshAgent.destination = 巡逻点集合[路径索引].transform.position;
                    巡逻等待状态计时器 = 0;
                    meshAgent.isStopped = false;
                }
            }
            else
            {

            }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (var tmp in 巡逻点集合)
        {
            Gizmos.DrawSphere(tmp.transform.position, 0.3f);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(巡逻点集合[路径索引].transform.position, 0.3f);

    }
}

