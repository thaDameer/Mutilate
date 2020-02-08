using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderOnHitTest : MonoBehaviour
{
   public Material shader;
   string dissolveAmount;
   float disolveValue = 1;

   private void Start() {
       shader.SetFloat("dissolveAmount", 1);
   }

    private void OnCollisionEnter2D(Collision2D other) 
    {
//        StartCoroutine(CameraScript.instance.CameraShake(0.5f, 0.5f));
        StartCoroutine(DissolveObject(3f));
    }
   IEnumerator DissolveObject(float time)
   {
       float elapsed = 0;
       float duration = time;
       float fadevalue;
       while(elapsed < duration)
       {
           fadevalue = Mathf.Lerp(disolveValue, 0, elapsed);
           elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
           shader.SetFloat("dissolveAmount", fadevalue);
           yield return new WaitForEndOfFrame();
       }
       shader.SetFloat("dissolveAmount", 0);
   }
}
