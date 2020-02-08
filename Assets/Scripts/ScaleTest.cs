using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTest : MonoBehaviour
{
   public bool isUpdated= false;
    Vector3 startScale;
    public float value;
    private void Start() {
        startScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        
    }
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //value +=1f;
            ScaleAndMove();
        }    
    }

    public void ScaleAndMove()
    {
        
     
        var newScale = new Vector3(transform.localScale.x,  transform.localScale.y + value, transform.localScale.z);
        transform.localScale = newScale;
        
     
        isUpdated = false;
    }
}
