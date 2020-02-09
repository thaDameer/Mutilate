using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderOnHitTest : MonoBehaviour
{
   public Material shader;
   string dissolveAmount;
   float disolveValue = 1;
   public float health = 2;

   private void Start() {
       shader.SetFloat("dissolveAmount", 1);
   }

    private void OnCollisionEnter2D(Collision2D other) 
    {
//        StartCoroutine(CameraScript.instance.CameraShake(0.5f, 0.5f));
        StartCoroutine(DissolveObject(3f, gameObject.GetComponent<MeshRenderer>().material));
    }
   IEnumerator DissolveObject(float time, Material mat)
   {
       health -= 1;
       float elapsed = 0;
       float duration = time;
       float fadevalue;
       while(elapsed < duration)
       {
           fadevalue = Mathf.Lerp(disolveValue, 0, elapsed);
           elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
           mat.SetFloat("dissolveAmount", fadevalue);
           yield return new WaitForEndOfFrame();
       }
       mat.SetFloat("dissolveAmount", 0);
   }
}
