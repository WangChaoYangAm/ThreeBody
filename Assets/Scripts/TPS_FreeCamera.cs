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
    private float tmpEulerX;//用来限制角度，存储绕x轴的视角

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

        Vector3 tmpModelEuler = player.transform.eulerAngles;//存储人物模型的欧拉角，以便后面在欧拉角被改动后重置它
        playerHandle.transform.Rotate(Vector3.up, InputX * Time.deltaTime * RotateSpeed);
        tmpEulerX -= InputY * Time.deltaTime * RotateSpeed;
        tmpEulerX = Mathf.Clamp(tmpEulerX, -85, 85);
        cameraHandle.transform.localEulerAngles = new Vector3(tmpEulerX, 0, 0);
        player.transform.eulerAngles = tmpModelEuler;//重置欧拉角，营造出一种人物未动的效果，实际上playerHandle已发生转向
        if (Input.GetKey(KeyCode.W))
        {
            LerpAngle();

        }
        animator.SetFloat("Speed", Input.GetAxis("Vertical"));
        m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, transform.position, 1f);
        m_camera.transform.eulerAngles = transform.eulerAngles;
    }
    /// <summary>
    /// 手动实现转向插值，因为localEulerAngles真正的值是0-360之间
    /// 将localEulerAngles恢复为Vector3.zero
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
