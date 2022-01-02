using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{
    [Range(1, 100)]
    public float MaxAlertValue;
    public float AlertValue;
    public float AlertRatio;
    public float AlertSpeed;

    [Header("V3,x-横向半偏角,y-纵向半偏角，z-距离深度")]
    public EnemyAlertViewData AlertViewConfigData;
    private AlertViewConfig ForwardView;
    private AlertViewConfig FlankView;
    private AlertViewConfig IntuitionView;
    [Header("要检测的身体部位")]
    public Transform[] BodyCheckPoints;
    Vector3 playerDir_X;//水平方向偏角范围
    Vector3 playerDir_Y;//垂直方向偏角范围
    public bool isInSight;
    private bool PlayerInAround;
    [SerializeField]
    private Transform TargetTrans;
    [Header("听觉系统")]
    private NavMeshAgent MeshAgent;
    private float EarDistance;
    private float AudioSourceValue;//声源强度
    private float AttenuationRatio;//衰减系数
    public LineRenderer LineRenderer;
    [Header("警戒UI")]
    [SerializeField]
    private GameObject AlertImg;
    [SerializeField]
    private Vector3 AlertImgOffset;
    public SphereCollider SphereCollider;
    void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
        //if (ForwardView.X_Angle > 180 || FlankView.X_Angle > 180 || IntuitionView.X_Angle > 180) Debug.LogError("视野半偏角之和超过180，请检查");
        //if (ForwardView.Y_Angle > 90 || FlankView.Y_Angle > 90 || IntuitionView.Y_Angle > 90) Debug.LogError("某视野垂直半偏角超过90，请检查");
        //if (VisionRatio.x < 0 || VisionRatio.y < 0 || VisionRatio.z < 0) Debug.LogError("视觉警戒值系数不得低于0，请检查");
        AlertRatio = 0.5f;
        AlertSpeed = 5f;
        MeshAgent = GetComponent<NavMeshAgent>();
        LineRenderer = GetComponent<LineRenderer>();
        AudioSourceValue = 5;
        AttenuationRatio = 0.5f;
        MaxAlertValue = 100;
        AlertValue = 0;
        AlertImgOffset = new Vector3(0, 35, 0);

    }

    // Update is called once per frame
    void Update()
    {
        SphereCollider.radius = AlertViewConfigData.AlertViewConfig[0].Z_Distance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerInAround = true;

        }
    }
    public void OnTriggerStay(Collider other)
    {

        if (PlayerInAround)
        {
            AlertImg.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, MeshAgent.height, 0)) + AlertImgOffset;
            #region 视觉模拟
            Vector3 forward = transform.forward;
            playerDir_X = new Vector3(other.transform.position.x, forward.y, other.transform.position.z) - new Vector3(transform.position.x, forward.y, transform.position.z);
            playerDir_Y = new Vector3(forward.x, other.transform.position.y, other.transform.position.z) - new Vector3(forward.x, transform.position.y, transform.position.z);
            float tmpX = Vector3.Angle(forward, playerDir_X);
            float tmpY = Vector3.Angle(forward, playerDir_Y);
            float tmpZ = Vector3.Distance(transform.position, other.transform.position);
            #region 不同的增长速率
            if (tmpX < AlertViewConfigData.AlertViewConfig[0].X_Angle && tmpY < AlertViewConfigData.AlertViewConfig[0].Y_Angle && tmpZ < AlertViewConfigData.AlertViewConfig[0].Z_Distance)//在正前方的视野中
            {
                //Debug.Log(forward + ";" + tmpX + ";" + tmpY + "距离为" + tmpZ + "正前方");
                if (tmpZ < AlertViewConfigData.AlertViewConfig[0].AlertDistance)
                {
                    AlertValue = MaxAlertValue;
                    TargetTrans = other.transform;
                }
                else
                {
                    AlertRatio = AlertViewConfigData.AlertViewConfig[0].DefaultRatio;

                }
            }
            else if (tmpX < AlertViewConfigData.AlertViewConfig[1].X_Angle && tmpY < AlertViewConfigData.AlertViewConfig[1].Y_Angle && tmpZ < AlertViewConfigData.AlertViewConfig[1].Z_Distance)//侧面，余光感知范围
            {
                //Debug.Log(forward + ";" + tmpX + ";" + tmpY + "距离为" + tmpZ + "侧面");
                if (tmpZ < AlertViewConfigData.AlertViewConfig[1].Z_Distance)
                {
                    AlertValue = MaxAlertValue;
                    TargetTrans = other.transform;
                }
                else
                {
                    AlertRatio = AlertViewConfigData.AlertViewConfig[1].DefaultRatio;

                }
            }
            else if (tmpX < AlertViewConfigData.AlertViewConfig[2].X_Angle && tmpY < AlertViewConfigData.AlertViewConfig[2].Y_Angle && tmpZ < AlertViewConfigData.AlertViewConfig[2].Z_Distance)//侧后方，直觉感知范围
            {
                //Debug.Log(forward + ";" + tmpX + ";" + tmpY + "距离为" + tmpZ + "侧后方直觉范围");
                if (tmpZ < AlertViewConfigData.AlertViewConfig[2].Z_Distance)
                {
                    AlertValue = MaxAlertValue;
                    TargetTrans = other.transform;
                }
                else
                {
                    AlertRatio = AlertViewConfigData.AlertViewConfig[2].DefaultRatio;

                }

            }
            #endregion
            #endregion
            //Debug.LogFormat("AlertSpeed为{0},AlertRatio为{1},CheckBodyPoints{2}", AlertSpeed, AlertRatio, CheckBodyPoints());
            if (BodyCheckPoints.Length >= 0)
            {
                isInSight = false;

                if (AlertValue < MaxAlertValue)//&& CheckBodyPoints()暂时将该条件拿出来
                {
                    AlertValue += Time.deltaTime * AlertSpeed * AlertRatio;
                }
                else if (AlertValue > MaxAlertValue)//该条件由于是大于，所以与最初的判断相呼应，设为MaxAlertValue后不再增长
                {
                    AlertValue = MaxAlertValue;
                    TargetTrans = other.transform;
                }
                else
                {
                    //已处于满警戒值状态
                    isInSight = true;
                }


            }
            else
            {
                Debug.LogError("未找到BodyCheckPoints");
            }
            #region 听觉模拟
            if (AudioSourceValue - AttenuationRatio * EarDistance > 0)
            {

            }
            //Debug.LogError("听到了");
            TargetTrans = other.transform;//这儿应该是player的？
            NavMeshPath Path = new NavMeshPath();
            if (MeshAgent.CalculatePath(TargetTrans.transform.position, Path))
            {
                Vector3[] wayPoints = new Vector3[Path.corners.Length + 2];
                wayPoints[0] = transform.position;
                wayPoints[wayPoints.Length - 1] = TargetTrans.position;
                for (int i = 0; i < Path.corners.Length; i++)
                {
                    wayPoints[i + 1] = Path.corners[i];
                }
                LineRenderer.positionCount = Path.corners.Length;
                for (int i = 1; i < wayPoints.Length; i++)
                {
                    EarDistance += (wayPoints[i] - wayPoints[i - 1]).magnitude;
                    LineRenderer.SetPositions(Path.corners);
                }
            }
            #endregion
        }
    }
    private void LateUpdate()
    {
        AlertImg.GetComponent<AlertValueManager>().Value = AlertValue;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerInAround = false;
            AlertValue = 0;
            TargetTrans = null;
        }
    }
    bool CheckBodyPoints()
    {
        int BodyPointsInSight = 0;
        for (int i = 0; i < BodyCheckPoints.Length; i++)
        {
            Vector3 tmpDir = BodyCheckPoints[i].position - transform.position;
            RaycastHit hit;
            Physics.Raycast(transform.position, BodyCheckPoints[i].position, out hit, ForwardView.Z_Distance);
            if (hit.collider.tag == "Player")
            {
                BodyPointsInSight += 1;
            }
        }
        if (BodyPointsInSight > 2)
        {
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        NavMeshPath Path = new NavMeshPath();
        if (TargetTrans != null)
        {
            if (MeshAgent.CalculatePath(TargetTrans.transform.position, Path))
            {
                Vector3[] wayPoints = new Vector3[Path.corners.Length + 2];
                wayPoints[0] = transform.position;
                wayPoints[wayPoints.Length - 1] = TargetTrans.position;
                for (int i = 0; i < Path.corners.Length; i++)
                {
                    wayPoints[i + 1] = Path.corners[i];
                }
                for (int i = 1; i < wayPoints.Length; i++)
                {
                    EarDistance += (wayPoints[i] - wayPoints[i - 1]).magnitude;
                    Gizmos.DrawLine(wayPoints[i - 1], wayPoints[i]);
                    //Gizmos.DrawCube(wayPoints[i], Vector3.one);
                }

            }


        }


    }



}



