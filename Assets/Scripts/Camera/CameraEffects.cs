using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class CameraEffects : MonoBehaviour
{
    Vignette vignette;
    PostProcessVolume volume;
    Tween fadeInVign;
    void Start()
    {
        vignette = ScriptableObject.CreateInstance<Vignette>();
        vignette.enabled.Override(true);
        vignette.intensity.Override(1f);

        volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, vignette);
        volume.weight = 0f;

        
           /* .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 0f, 1f))
            .OnComplete(() =>
            {
               // RuntimeUtilities.DestroyVolume(volume, true, true);
               //Destroy(this);
            }); */
    }

    public void FadeInVignette()
    {
        DOTween.Sequence()
            .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 0.5f, 0.3f))
            .AppendInterval(1f);
    }
    public void FadeOutVignette()
    {
        DOTween.Sequence()
            .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 0f, 1f))
            .AppendInterval(1f);
    }
}