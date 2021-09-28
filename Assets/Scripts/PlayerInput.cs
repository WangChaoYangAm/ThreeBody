using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("=======按键设定=======")]
    [Header("按键必须是小写")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string KeyRight = "d";

    public string KeyA;
    public string KeyB;
    public string KeyC;
    public string KeyD;

    [Header("=======输出信号=======")]
    public float Dup;
    public float Dright;
    public float Dmag;//长度
    public Vector3 Dvector3;
    //按压式信号
    public bool run;
    //单次触发式信号
    public bool jump;
    private bool lastJump;
    [Header("=======其它设定=======")]
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

        Vector2 tempAxis = SquareToCircle(new Vector2(Dright, Dup));//经过转换后的水平轴与垂直轴上的向量记录在该变量中
        float Dright2 = tempAxis.x;
        float Dup2 = tempAxis.y;

        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);//利用三角函数，开方求对应方向上的移动长度
        Dvector3 = Dright2 * transform.right + Dup2 * transform.forward;

        run = Input.GetKey(KeyA);
        bool newJump = Input.GetKey(KeyB);
        if(newJump !=lastJump && newJump == true)
        {
            jump = true;
            Debug.Log("按下触发");
        }
        else
        {
            jump = false;
        }
        lastJump = newJump;
    }
    /// <summary>
    /// 用来转换斜向速度过快
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
