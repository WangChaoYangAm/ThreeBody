using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertViewGizmos : MonoBehaviour
{
    public Color color = Color.red;

    [SerializeField]

    [Header("X半偏角")]

    private int Alert_X = 60;

    [SerializeField]

    [Header("Y半偏角")]

    private int Alert_Y = 30;

    [SerializeField]

    [Header("X轴细分曲面角度偏移")]

    [Range(1, 10)]

    private int Angel_X = 5;

    [SerializeField]

    [Header("Y轴细分曲面角度偏移")]

    [Range(1, 10)]

    private int Angel_Y = 5;

    [Header("距离")]

    [Range(1, 10)]

    private int Distance = 10;

    [SerializeField]
    [Header("是否遮挡剔除")]

    private bool allowCullingMask;

    [SerializeField]

    [Header("是否同时禁用点线绘制，优先于绘制点与绘制线")]

    private bool allowDrawLineAndCube;

    [SerializeField]

    [Header("是否绘制点方块")]

    private bool allowDrawCube;

    [SerializeField]

    [Header("是否绘制线")]

    private bool allowDrawLine;

    void DrawUpRight()
    {
        Vector3[] points = new Vector3[(Alert_X / Angel_X + 1) * (Alert_Y / Angel_Y + 1)];
        Vector3[] vertices = new Vector3[(Alert_X / Angel_X) * (Alert_Y / Angel_Y) * 4];
        int[] tri = new int[(Alert_X / Angel_X + 1) * (Alert_Y / Angel_Y + 1) * 6];
        Mesh mesh = new Mesh();
        mesh.Clear();
        for (int k = 0; k < (Alert_Y / Angel_Y) + 1; k++)//31
        {
            for (int i = 0; i < (Alert_X / Angel_X) + 1; i++)//61
            {
                Quaternion rotation = Quaternion.Euler(0, 30f, 0) * transform.rotation;
                points[k * ((Alert_X / Angel_X) + 1) + i] = (new Vector3(
                    Mathf.Cos(k * Angel_Y * Mathf.Deg2Rad) * Mathf.Sin(i * Angel_X * Mathf.Deg2Rad) * 10,
                    Mathf.Sin(k * Angel_Y * Mathf.Deg2Rad) * 10,
                    Mathf.Cos(k * Angel_Y * Mathf.Deg2Rad) * Mathf.Cos(i * Angel_X * Mathf.Deg2Rad) * 10
                    ));

                if (allowCullingMask)
                {
                    RaycastHit hit;
                    Physics.Linecast(transform.position, points[k * ((Alert_X / Angel_X) + 1) + i], out hit);
                    if (hit.collider != null)
                    {
                        points[k * ((Alert_X / Angel_X) + 1) + i] = hit.point;
                    }
                }
            }
        }

        if (allowDrawLineAndCube)
        {
            for (int k = 0; k < (Alert_Y / Angel_Y) + 1; k++)//31
            {
                for (int i = 0; i < (Alert_X / Angel_X) + 1; i++)//61
                {
                    if (allowDrawCube)
                    {
                        Gizmos.DrawCube(points[k * ((Alert_X / Angel_X) + 1) + i], new Vector3(0.1f, 0.1f, 0.1f));
                    }
                    if (allowDrawLine)
                    {
                        if (i < (Alert_X / Angel_X))
                            Gizmos.DrawLine(points[k * ((Alert_X / Angel_X) + 1) + i], points[k * ((Alert_X / Angel_X) + 1) + i + 1]);
                    }
                }
            }
        }
        //逆时针为Z指向的方向（朝外）
        for (int k = 0; k < (Alert_Y / Angel_Y); k++)
        {
            for (int i = 0; i < (Alert_X / Angel_X); i++)
            {
                tri[(k * (Alert_X / Angel_X) + i) * 6] = k * (Alert_X / Angel_X + 1) + i;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 1] = k * (Alert_X / Angel_X + 1) + i + 1;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 2] = (k + 1) * (Alert_X / Angel_X + 1) + i + 1;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 3] = (k + 1) * (Alert_X / Angel_X + 1) + i + 1;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 4] = (k + 1) * (Alert_X / Angel_X + 1) + i;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 5] = k * (Alert_X / Angel_X + 1) + i;
            }
        }
        mesh.vertices = points;
        mesh.triangles = tri;
        mesh.RecalculateNormals();
        Gizmos.color = Color.yellow;
        Gizmos.DrawMesh(mesh);
    }

    void DrawForwardPoint()
    {
        Transform Target = this.transform;
        Quaternion rotation = Quaternion.Euler(-10f, 30f, 0f) * Target.rotation;
        Vector3 newPos = rotation * new Vector3(10f, 0f, 0f);
        Gizmos.DrawCube(newPos,Vector3.one);
        //Debug.DrawLine(newPos, Vector3.zero, Color.red);
        //Debug.Log("newpos " + newPos + " nowpos " + Target.position + " distance " + Vector3.Distance(newPos, Target.position));

        /*
        Vector3[] points = new Vector3[(Alert_X / Angel_X + 1) * (Alert_Y / Angel_Y + 1)];
        Vector3[] vertices = new Vector3[(Alert_X / Angel_X) * (Alert_Y / Angel_Y) * 4];
        int[] tri = new int[(Alert_X / Angel_X + 1) * (Alert_Y / Angel_Y + 1) * 6];
        Mesh mesh = new Mesh();
        mesh.Clear();
        for (int k = 0; k < (Alert_Y / Angel_Y) + 1; k++)//31
        {
            for (int i = 0; i < (Alert_X / Angel_X) + 1; i++)//61
            {
                //points[k * ((Alert_X / Angel_X) + 1) + i] = new Vector3(
                //    Mathf.Cos(k * Angel_Y * Mathf.Deg2Rad) * Mathf.Sin(i * Angel_X * Mathf.Deg2Rad) * 10,
                //    Mathf.Sin(k * Angel_Y * Mathf.Deg2Rad) * 10,
                //    Mathf.Cos(k * Angel_Y * Mathf.Deg2Rad) * Mathf.Cos(i * Angel_X * Mathf.Deg2Rad) * 10
                //    );
                //Quaternion rotation=Quaternion.eu
                if (allowCullingMask)
                {
                    RaycastHit hit;
                    Physics.Linecast(transform.position, points[k * ((Alert_X / Angel_X) + 1) + i], out hit);
                    if (hit.collider != null)
                    {
                        points[k * ((Alert_X / Angel_X) + 1) + i] = hit.point;
                    }
                }
            }
        }

        if (allowDrawLineAndCube)
        {
            for (int k = 0; k < (Alert_Y / Angel_Y) + 1; k++)//31
            {
                for (int i = 0; i < (Alert_X / Angel_X) + 1; i++)//61
                {
                    if (allowDrawCube)
                    {
                        Gizmos.DrawCube(points[k * ((Alert_X / Angel_X) + 1) + i], new Vector3(0.1f, 0.1f, 0.1f));
                    }
                    if (allowDrawLine)
                    {
                        if (i < (Alert_X / Angel_X))
                            Gizmos.DrawLine(points[k * ((Alert_X / Angel_X) + 1) + i], points[k * ((Alert_X / Angel_X) + 1) + i + 1]);
                    }
                }
            }
        }
        //逆时针为Z指向的方向（朝外）
        for (int k = 0; k < (Alert_Y / Angel_Y); k++)
        {
            for (int i = 0; i < (Alert_X / Angel_X); i++)
            {
                tri[(k * (Alert_X / Angel_X) + i) * 6] = k * (Alert_X / Angel_X + 1) + i;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 1] = k * (Alert_X / Angel_X + 1) + i + 1;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 2] = (k + 1) * (Alert_X / Angel_X + 1) + i + 1;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 3] = (k + 1) * (Alert_X / Angel_X + 1) + i + 1;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 4] = (k + 1) * (Alert_X / Angel_X + 1) + i;
                tri[(k * (Alert_X / Angel_X) + i) * 6 + 5] = k * (Alert_X / Angel_X + 1) + i;
            }
        }
        mesh.vertices = points;
        mesh.triangles = tri;
        mesh.RecalculateNormals();
        Gizmos.color = Color.yellow;
        Gizmos.DrawMesh(mesh);
        */
    }
    private void OnDrawGizmos()
    {
        //DrawUpRight();
        DrawForwardPoint();
    }
}
