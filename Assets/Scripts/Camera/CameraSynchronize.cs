using System.Collections;
using UnityEngine;

public class CameraSynchronize : MonoBehaviour {

   public OculusFadeCameraEffect fadeEffect;

    public void SyncCamera()
    {
        StartCoroutine("StartSynchronize");
    }

    IEnumerator StartSynchronize()
    {
        while (true)
        {
            yield return null;
            fadeEffect.FadeIn();
            yield return new WaitForSeconds(1.5f);
            fadeEffect.InstantFadeIn();
            UnityEngine.XR.InputTracking.Recenter();
            yield return new WaitForSeconds(1.5f);
            Debug.Log("Camera Centralized");
            fadeEffect.FadeOut();
            yield return new WaitForSeconds(1.5f);
            StopCoroutine("StartSynchronize");
        }

    }
}
