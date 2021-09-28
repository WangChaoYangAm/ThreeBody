using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("=======�����趨=======")]
    [Header("����������Сд")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string KeyRight = "d";

    public string KeyA;
    public string KeyB;
    public string KeyC;
    public string KeyD;

    [Header("=======����ź�=======")]
    public float Dup;
    public float Dright;
    public float Dmag;//����
    public Vector3 Dvector3;
    //��ѹʽ�ź�
    public bool run;
    //���δ���ʽ�ź�
    public bool jump;
    private bool lastJump;
    [Header("=======�����趨=======")]
    public bool InputEnable=true;

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;






    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        targetDup = (Input.GetKey(KeyUp) ? 1.0f : 0f) - (Input.GetKey(KeyDown) ? 1.0f : 0f);
        targetDright = (Input.GetKey(KeyRight) ? 1.0f : 0f) - (Input.GetKey(KeyLeft) ? 1.0f : 0f);

        if (InputEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempAxis = SquareToCircle(new Vector2(Dright, Dup));//����ת�����ˮƽ���봹ֱ���ϵ�������¼�ڸñ�����
        float Dright2 = tempAxis.x;
        float Dup2 = tempAxis.y;

        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);//�������Ǻ������������Ӧ�����ϵ��ƶ�����
        Dvector3 = Dright2 * transform.right + Dup2 * transform.forward;

        run = Input.GetKey(KeyA);
        bool newJump = Input.GetKey(KeyB);
        if(newJump !=lastJump && newJump == true)
        {
            jump = true;
            Debug.Log("���´���");
        }
        else
        {
            jump = false;
        }
        lastJump = newJump;
    }
    /// <summary>
    /// ����ת��б���ٶȹ���
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private Vector2 SquareToCircle(Vector2 input)//float Dup,float Dright
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - input.y * input.y * 0.5f);
        output.y = input.y * Mathf.Sqrt(1 - input.x * input.x * 0.5f);
        return output;
    }
} 
