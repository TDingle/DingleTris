using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("Left",true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool("Left", false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetBool("Right", true);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetBool("Right", false);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetBool("Down", true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            animator.SetBool("Down", false);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("RLeft", true);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            animator.SetBool("RLeft", false);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("RRight", true);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            animator.SetBool("RRight", false);
        }
    }
}
