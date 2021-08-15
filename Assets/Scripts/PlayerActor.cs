using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour
{
    public GameObject model;
    public PlayerInput PI;
    [SerializeField]
    private Animator anim;




    void Awake()
    {
        anim = model.GetComponent<Animator>();
        PI = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Forward", PI.Dmag);
        if (PI.Dmag > 0.1f) 
        model.transform.forward = PI.Dvector3;
    }
}
