using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_MoveController : MonoBehaviour
{
    [Header("�������")]
    public float freeDistance = 2;
    [Header("����������")]
    public float minDistance = 0.5f;
    [Header("�����Զ����")]
    public float maxDistance = 20;
    [Header("�Ƿ�ɿ����������(����м�)")]
    public bool canControlDistance = true;
    [Header("�������������ٶ�")]
    public float distanceSpeed = 1;

    [Header("�ӽ�������")]
    public float rotateSpeed = 1;
    [Header("����ת���ֵ(������,ȡֵΪ0��1)")]
    public float TargetBodyRotateLerp = 0.3f;
    [Header("��Ҫת�������")]
    public GameObject TargetBody;//�˽ű��ܲ���ת�������
    [Header("�����������")]
    public GameObject CameraPivot;//�����������  
    [Header("===����===")]
    public GameObject lockTarget = null;
    public float lockSlerp = 1;
    public GameObject lockMark;
    private bool marked;

    [Header("�Ƿ�ɿ�������ת��")]
    public bool CanControlDirection = true;
    [Header("����(0-89)")]
    public float maxDepression = 80;
    [Header("����(0-89)")]
    public float maxEvelation = 80;


    private Vector3 PredictCameraPosition;
    private Vector3 offset;
    private Vector3 wallHit;
    private GameObject tmpMark;
    // Use this for initialization
    void Start()
    {

        offset = transform.position - CameraPivot.transform.position+new Vector3(0.6f,0,0);
        if (TargetBody == null)
        {
            TargetBody = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("δ��Ŀ�����壬Ĭ���滻ΪPlayer��ǩ������");
        }
        if (!CameraPivot)
        {
            Debug.LogError("δ�������������");
        }

    }

    void LockTarget()
    {
        if (lockTarget)
        {
            lockTarget = null;
            marked = false;
            Destroy(tmpMark);
            return;
        }

        Vector3 top = transform.position + new Vector3(0, 1, 0) + transform.forward * 5;
        LayerMask mask = (1 << LayerMask.NameToLayer("Mob")); //�������Layer����ΪIgnore Raycast,Player��Mob��������������ߣ���Ȼ���������ĳЩ����ǰ,�������,��ҵ�,

        Collider[] cols = Physics.OverlapBox(top, new Vector3(0.5f, 0.5f, 5), transform.rotation, mask);
        foreach (var col in cols)
        {
            lockTarget = col.gameObject;
        }
    }

    bool Inwall()
    {

        //RaycastHit hit;
        //LayerMask mask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Mob")) | (1 << LayerMask.NameToLayer("Weapon")); //�������Layer����ΪIgnore Raycast,Player��Mob��������������ߣ���Ȼ���������ĳЩ����ǰ,�������,��ҵ�,
        //mask = ~mask;//�����ϵ�maskȡ��,��ʾ���߽���������ϵĲ�
        //             //Debug.DrawLine(CameraPivot.transform.position, transform.position - transform.forward, Color.red);

        //PredictCameraPosition = CameraPivot.transform.position + offset.normalized * freeDistance;//Ԥ������λ��
        //if (Physics.Linecast(CameraPivot.transform.position, PredictCameraPosition, out hit, mask))//��ײ��������ײ��,ע��,��Ϊ���û����ײ��,�����ǲ�����ײ�������,Ҳ����û����ײ��ʱ˵��û���ڵ�
        //{//Ҳ����˵�����if����ָ���ڵ������


        //    wallHit = hit.point;//��ײ��λ��
        //    //Debug.DrawLine(transform.position, wallHit, Color.green);
        //    return true;
        //}
        //else//û��ײ����Ҳ����˵û���ϰ���
        //{
        //    return false;
        //}

        return false;
    }


    void FreeCamera()
    {
        offset = offset.normalized * freeDistance;
        transform.position = CameraPivot.transform.position + offset;//����λ��

        if (CanControlDirection)//���ƽ�ɫ���򿪹�
        {
            Quaternion TargetBodyCurrentRotation = TargetBody.transform.rotation;

            if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    TargetBody.transform.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(TargetBody.transform.localEulerAngles.x, transform.localEulerAngles.y - 45, TargetBody.transform.localEulerAngles.z)), TargetBodyRotateLerp);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    TargetBody.transform.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(TargetBody.transform.localEulerAngles.x, transform.localEulerAngles.y - 135, TargetBody.transform.localEulerAngles.z)), TargetBodyRotateLerp);
                }


                else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                {
                    TargetBody.transform.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(TargetBody.transform.localEulerAngles.x, transform.localEulerAngles.y - 90, TargetBody.transform.localEulerAngles.z)), TargetBodyRotateLerp);
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    TargetBody.transform.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(TargetBody.transform.localEulerAngles.x, transform.localEulerAngles.y + 45, TargetBody.transform.localEulerAngles.z)), TargetBodyRotateLerp);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    TargetBody.transform.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(TargetBody.transform.localEulerAngles.x, transform.localEulerAngles.y + 135, TargetBody.transform.localEulerAngles.z)), TargetBodyRotateLerp);
                }

                else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                {
                    TargetBody.transform.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(TargetBody.transform.localEulerAngles.x, transform.localEulerAngles.y + 90, TargetBody.transform.localEulerAngles.z)), TargetBodyRotateLerp);
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                TargetBody.transform.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(TargetBody.transform.localEulerAngles.x, transform.localEulerAngles.y, TargetBody.transform.localEulerAngles.z)), TargetBodyRotateLerp);

            }
            else if (Input.GetKey(KeyCode.S))
            {
                TargetBody.transform.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(TargetBody.transform.localEulerAngles.x, transform.localEulerAngles.y - 180, TargetBody.transform.localEulerAngles.z)), TargetBodyRotateLerp);

            }
        }

        if (canControlDistance)//���ƾ��뿪��
        {
            freeDistance -= Input.GetAxis("Mouse ScrollWheel") * distanceSpeed;
        }

        freeDistance = Mathf.Clamp(freeDistance, minDistance, maxDistance);

        if (!lockTarget)
        {


            transform.LookAt(lockTarget ? (lockTarget.transform.position) : CameraPivot.transform.position);
        }
        else
        {
            Quaternion tmp = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lockTarget.transform.position - transform.position), lockSlerp * Time.fixedDeltaTime);
            transform.rotation = tmp;

        }

        float eulerX = transform.localEulerAngles.x;//�����xŷ����,Ҳ���Ǵ�ֱ����.
        float inputY = Input.GetAxis("Mouse Y");


        if (!lockTarget)
        {
            //��ֱ��Ұ����
            if (!lockTarget)
            {
                transform.RotateAround(CameraPivot.transform.position, Vector3.up, rotateSpeed * Input.GetAxis("Mouse X"));//x��������
            }

            if (eulerX > maxDepression && eulerX < 90)//�����ϽǶ�Խ��ʱ
            {
                if (inputY > 0)//������ʱ�����»���
                    transform.RotateAround(CameraPivot.transform.position, Vector3.right, -rotateSpeed * inputY);//������
            }
            else if (eulerX < 360 - maxEvelation && eulerX > 270)
            {
                if (inputY < 0)
                    transform.RotateAround(CameraPivot.transform.position, Vector3.right, -rotateSpeed * inputY);
            }
            else//�Ƕ�����ʱ
            {

                transform.RotateAround(CameraPivot.transform.position, Vector3.right, -rotateSpeed * inputY);


            }
        }
        if (lockTarget)
        {
            offset = CameraPivot.transform.position - (lockTarget.transform.position);
        }
        else
        {
            offset = transform.position - CameraPivot.transform.position;//���Ϸ������˱仯,��¼�µķ�������
        }

        offset = offset.normalized * freeDistance;

        ///��һ��FixedUpdate��,��ʱ��¼�µ���ת���λ��,Ȼ��õ�����,Ȼ���ж��Ƿ񼴽����ڵ�,���Ҫ���ڵ�,������ƶ��������Ĳ��ᱻ�ڵ���λ��
        ///������ᱻ�ڵ�,�����λ��Ϊ�������λ��+����ĵ�λ����*����
        ///
        if (Inwall())//Ԥ��ᱻ�ڵ�
        {
            //print("Inwall");

            transform.position = CameraPivot.transform.position + (wallHit - CameraPivot.transform.position) * 0.8f;

            return;


        }
        else
        {
            transform.position = CameraPivot.transform.position + offset;

        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FreeCamera();
        if (lockTarget)
        {

            if (!marked)
            {
                tmpMark = Instantiate(lockMark, lockTarget.transform.position + new Vector3(0, 2.5f, 0), transform.rotation);
                tmpMark.transform.forward = -Vector3.up;
                marked = true;
            }

            else
            {
                tmpMark.transform.position = lockTarget.transform.position + new Vector3(0, 2.5f, 0);
                //tmpMark.transform.forward= -transform.up;
                tmpMark.transform.Rotate(Vector3.up * 30 * Time.fixedDeltaTime, Space.World);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LockTarget();
        }
    }

//��������������������������������
//��Ȩ����������ΪCSDN���������Ͽա���ԭ�����£���ѭCC 4.0 BY-SA��ȨЭ�飬ת���븽��ԭ�ĳ������Ӽ���������
//ԭ�����ӣ�https://blog.csdn.net/qq_37724011/article/details/80292500

}
