using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimationControl : MonoBehaviour
{

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("ChangeMovement");
            
        }
    }
}
