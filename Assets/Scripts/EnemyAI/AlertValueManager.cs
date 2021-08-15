using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertValueManager : MonoBehaviour
{
    [Range(0,100F)]
    public float Value;
    [SerializeField]
    GameObject FillImg;
    Image Image;
    // Start is called before the first frame update
    void Start()
    {
        Image = FillImg.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAlertUI();
    }
    public void SetAlertUI()
    {
        FillImg.transform.localScale = new Vector3(Value / 100, Value / 100, Value / 100);
        if(Value>0)
        Image.color = new Color(1,1-Value/100,0,1);
    }
}

