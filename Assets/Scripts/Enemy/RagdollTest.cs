using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollTest : MonoBehaviour
{
    public Rigidbody2D[] bodyParts;
    public Animator animator;
    public bool turnOnDoll;

    private void Start() 
    {
         for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].bodyType = RigidbodyType2D.Static;
        }    
    }
    private void Update() {
        if(turnOnDoll)
        {
            ToggleRigidbody();
        }
    }


    private void ToggleRigidbody()
    {
        animator.enabled = false;
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].bodyType = RigidbodyType2D.Dynamic;
            bodyParts[i].mass = 3f;
        }
    }
}
