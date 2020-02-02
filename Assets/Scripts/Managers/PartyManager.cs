using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public ParticleSystem jumpSmoke;
    public static PartyManager partyInstance;

private void Awake() {
    partyInstance = this;
}
    public void InstantiateSmoke(Vector2 pos)
    {
        var smokeClone = Instantiate(jumpSmoke,pos, Quaternion.Euler(-90,0,0));
        smokeClone.Emit(60);
        StartCoroutine(DestroyParticle(2f,smokeClone.gameObject));
    }

    public IEnumerator DestroyParticle(float time, GameObject objToDestroy)
    {
        float duration = time;
        float elapsed = 0;
        while (duration > elapsed)
        {
            elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        DestroyImmediate(objToDestroy);
    }

}
