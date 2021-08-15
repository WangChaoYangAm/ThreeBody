using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("按键必须是小写")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string KeyRight = "d";

    public float Dup;
    public float Dright;
    public float Dmag;//长度
    public Vector3 Dvector3;

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
        Dmag = Mathf.Sqrt(Dup * Dup + Dright * Dright);
        Dvector3 = Dright * transform.right + Dup * transform.forward;
    }
}
