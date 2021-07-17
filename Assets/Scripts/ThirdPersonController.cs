using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public Transform Target;    //Ŀ��

    [SerializeField] private float m_CamMoveSpeed = 1f;
    [Range(0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;

    private Transform m_CamTrans;
    private Transform m_PivotTrans;

    private float m_LookAngle;  //���ҽ�  ��Y��
    private float m_TiltAngle;  //���½�  ��X��

    private Vector3 m_PivotEulers;
    private Quaternion m_PivotRotation;
    private Quaternion m_TransformRotation;

    [SerializeField] private float m_XRotateMax = 30f;   //��X�����Ƕ�
    [SerializeField] private float m_XRotateMin = -60f;   //��X����С�Ƕ�
    [SerializeField] private bool m_LockCursor = false;   //�������

    private void Awake()
    {
        RefreshCursor();

        m_CamTrans = GetComponentInChildren<Camera>().transform;
        m_PivotTrans = m_CamTrans.parent;

        m_PivotEulers = m_PivotTrans.rotation.eulerAngles;
        m_PivotRotation = m_PivotTrans.transform.localRotation;
        m_TransformRotation = transform.localRotation;
    }

    private void Update()
    {
        HandleRotationMovement();

        RefreshCursor();
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (Target == null)
        {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * m_CamMoveSpeed);
    }

    void HandleRotationMovement()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        m_LookAngle += x * m_TurnSpeed;
        m_TransformRotation = Quaternion.Euler(0f, m_LookAngle, 0f);

        m_TiltAngle -= y * m_TurnSpeed;
        //��X��ת�ǶȽ���Clamp����
        m_TiltAngle = Mathf.Clamp(m_TiltAngle, m_XRotateMin, m_XRotateMax);
        m_PivotRotation = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y, m_PivotEulers.z);

        m_PivotTrans.localRotation = m_PivotRotation;
        transform.localRotation = m_TransformRotation;
    }

    void RefreshCursor()
    {
        Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !m_LockCursor;
    }

}
