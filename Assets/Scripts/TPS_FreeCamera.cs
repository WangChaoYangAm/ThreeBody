using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_FreeCamera : MonoBehaviour
{
    public float RotateSpeed;
    public GameObject player;
    public GameObject m_camera;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tmpEulerX;//�������ƽǶȣ��洢��x����ӽ�

    float InputX, InputY;
    Animator animator;
    private void Awake()
    {
        RotateSpeed = 100f;
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
    }
    private void Start()
    {
        animator = player.GetComponent<Animator>();
    }
    private void Update()
    {
        InputX = Input.GetAxis("Mouse X");
        InputY = Input.GetAxis("Mouse Y");

        Vector3 tmpModelEuler = player.transform.eulerAngles;//�洢����ģ�͵�ŷ���ǣ��Ա������ŷ���Ǳ��Ķ���������
        playerHandle.transform.Rotate(Vector3.up, InputX * Time.deltaTime * RotateSpeed);
        tmpEulerX -= InputY * Time.deltaTime * RotateSpeed;
        tmpEulerX = Mathf.Clamp(tmpEulerX, -85, 85);
        cameraHandle.transform.localEulerAngles = new Vector3(tmpEulerX, 0, 0);
        player.transform.eulerAngles = tmpModelEuler;//����ŷ���ǣ�Ӫ���һ������δ����Ч����ʵ����playerHandle�ѷ���ת��
        if (Input.GetKey(KeyCode.W))
        {
            LerpAngle();

        }
        animator.SetFloat("Speed", Input.GetAxis("Vertical"));
        m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, transform.position, 1f);
        m_camera.transform.eulerAngles = transform.eulerAngles;
    }
    /// <summary>
    /// �ֶ�ʵ��ת���ֵ����ΪlocalEulerAngles������ֵ��0-360֮��
    /// ��localEulerAngles�ָ�ΪVector3.zero
    /// </summary>
    void LerpAngle()
    {
        Vector3 tmp;

        if (Mathf.Abs(player.transform.localEulerAngles.y) - 0 > 1)
        {
            if (player.transform.localEulerAngles.y > 180)
            {
                tmp = new Vector3(0, player.transform.localEulerAngles.y + Time.deltaTime * 200, 0);
                player.transform.localEulerAngles = tmp;
            }
            else
            {
                tmp = new Vector3(0, player.transform.localEulerAngles.y - Time.deltaTime * 200, 0);
                player.transform.localEulerAngles = tmp;
                Debug.LogError(player.transform.localEulerAngles.y + ";;" + (Mathf.Abs(player.transform.localEulerAngles.y) - 0));
            }
        }
        else
        {
            player.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

    }

}
