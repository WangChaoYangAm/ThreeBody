using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Camera : MonoBehaviour
{
    private Transform player;
    private Vector3 offsetPosition; //位置偏移
    private bool isRotate;

    public Vector3 offset;
    public float distance;
    public float scrollSpeed = 10;
    public float RotateSpeed = 2;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(player.position);
        offsetPosition = transform.position - player.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offsetPosition + player.position;

        RotateView();

        ScrollView();
    }

    void ScrollView()
    {
        //鼠标滑轮拉近拉远视野
        distance = offsetPosition.magnitude;
        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, 2, 18);//限制镜头缩放最远以及最近距离
        offsetPosition = offsetPosition.normalized * distance;
    }

    void RotateView()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRotate = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotate = false;
        }

        if (isRotate)
        {
            transform.RotateAround(player.position , player.up, RotateSpeed * Input.GetAxis("Mouse X"));

            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;

            transform.RotateAround(player.position, transform.right, -RotateSpeed * Input.GetAxis("Mouse Y"));
            //垂直和水平都会影响两个值

            float x = transform.eulerAngles.x;

            if (x < 10 || x > 80)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }
        }
        offsetPosition = transform.position - player.position + offset;
    }


}
