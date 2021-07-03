using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Camera : MonoBehaviour
{
    public Transform followTarget;
    public Vector2 rotate;
    public float rotateSpeed;
    public float moveSpeed;
    [Header("视野的垂直朝向偏角，正前方向上从0开始，例如:x为-80，y为80")]
    public Vector2 MinMaxY;
    //public float maxY;
    //public float minY;
    [Header("视野FOV")]
    public float ViewSize;
    public Vector2 MinMaxViewSize;
    public float DefalutAngle;
    public float distance;
    public float height;
    public bool MouuseVisiable;
    public CursorLockMode lockMode = CursorLockMode.Confined;
    public Camera controlCamera;
    void Start()
    {
        controlCamera = this.GetComponent<Camera>();
        if (MinMaxViewSize.x > MinMaxViewSize.y) Debug.LogError("最小ViewSize不得大于等于最大ViewSize");
        if (MinMaxY.y <= 0 || MinMaxY.y<= MinMaxY.x) Debug.LogError("MinMaxY不建议小于0或者MinMaxY.x");
        if (MinMaxY.x >= 0 || MinMaxY.x>= MinMaxY.y) Debug.LogError("MinMaxY不建议小大于0或者MinMaxY.y");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        FixedCamera();
    }
    private void LateUpdate()
    {
        LateCamera();
    }
    void FixedCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        rotate.x += mouseX * rotateSpeed;
        rotate.y += mouseY * rotateSpeed;
        ViewSize += -Input.mouseScrollDelta.y * 3;
        if (ViewSize < MinMaxViewSize.x) ViewSize = MinMaxViewSize.x;
        if (ViewSize > MinMaxViewSize.y) ViewSize = MinMaxViewSize.y;
        if (rotate.x >= 360 || rotate.x <= 0) rotate.x = 0;
        if (rotate.y > MinMaxY.y) rotate.y = MinMaxY.y;//最大仰角设置，为正数
        if (rotate.y < MinMaxY.x) rotate.y = MinMaxY.x;//最小仰角设置，为负数
        controlCamera.fieldOfView = ViewSize;
        Cursor.visible = MouuseVisiable;
        Cursor.lockState = lockMode;

    }
    void LateCamera()
    {
        Transform self = controlCamera.transform;
        Vector3 startPos = self.position;
        Vector3 endPos = self.position;

        Vector3 targetPos = followTarget.position;
        targetPos.y += height;

        Vector2 v1 = CaculateAbsulatePoint(rotate.x,distance);
        endPos = targetPos + new Vector3(v1.x, 0, v1.y);
        //相机观察点
        Vector2 v2 = CaculateAbsulatePoint(rotate.x + DefalutAngle, 1);
        Vector3 ViewPoint = new Vector3(v2.x, 0, v2.y) + targetPos;

        float dist = Vector3.Distance(endPos, ViewPoint);
        Vector2 v3 = CaculateAbsulatePoint(rotate.y, dist);
        endPos += new Vector3(0, v3.y, 0);

        RaycastHit hit;
        if(Physics.Linecast(targetPos,endPos,out hit))
        {
            string name = hit.collider.gameObject.tag;
            Debug.LogError("要检查击中对象的名字与if条件是否满足预期目标");
            if (name != "MainCamera" || name != "Player")
            {
                endPos = hit.point - (endPos - hit.point).normalized * 0.2f;
            }
        }
        self.position = Vector3.Lerp(startPos, endPos, Time.deltaTime * moveSpeed);
        Quaternion quaternion = Quaternion.LookRotation(ViewPoint - endPos);
        self.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * moveSpeed);
        
    }

    Vector2 CaculateAbsulatePoint(float angle,float dist )
    {
        float radian = -angle * (Mathf.PI / 180);
        float x = dist * Mathf.Cos(radian);
        float y = dist * Mathf.Sin(radian);
        return new Vector2(x, y);
    }

}
