using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript instance;

    private void Awake() {
        instance = this;
    }
    public IEnumerator CameraShake(float amount, float time)
    {
        float elapsed = 0;
        float duration = time;
        
        Vector2 startPos = transform.position;
        Vector3 zPos = transform.position;
        while(elapsed < duration)
        {
            var newPos =startPos + Random.insideUnitCircle * amount;
            elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
            Vector3 shakePos = new Vector3(newPos.x, newPos.y, zPos.z);
            transform.position = Vector3.Lerp(transform.position,shakePos, elapsed);
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(startPos.x, startPos.y, zPos.z);
    }
}
