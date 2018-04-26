using UnityEngine;
using System.Collections;
using System;

public class OculusFadeCameraEffect : AbstractFadeCameraEffect
{
    private OVRCameraRig cameraRig;
    private Coroutine fadeCoroutine;
    private CameraFader fader;
    private Material fadeMaterial = null;
    private float currentAlpha;

    public override void FadeIn()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(Fade(0, 1));
    }

    public override void FadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(Fade(1, 0));
    }

    public override void InstantFadeIn()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        currentAlpha = 1;
        SetMaterialAlpha();
    }

    public override void InstantFadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        currentAlpha = 0;
        SetMaterialAlpha();
    }

    public override void Startup()
    {
        cameraRig = FindObjectOfType<OVRCameraRig>();
        if (cameraRig == null)
            throw new NullReferenceException("Cant find OVRCameraRig in scene");
        fader = cameraRig.GetComponentInChildren<CameraFader>();
        if (fader == null)
        {
            GameObject fadeObj = Instantiate(Resources.Load("Prefabs/Fader", typeof(GameObject))) as GameObject;
            fadeObj.transform.SetParent(cameraRig.centerEyeAnchor, false);
            fader = fadeObj.GetComponent<CameraFader>();
        }
        fader.transform.localPosition = new Vector3(0, 0, cameraRig.leftEyeCamera.nearClipPlane * 2);
        fadeMaterial = new Material(fader.image.material);
        fader.image.material = fadeMaterial;
        InstantFadeIn();
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;
        float spendTime = fadeTime - Mathf.Clamp01(Mathf.Abs(endAlpha - currentAlpha)) * fadeTime;
        while (currentAlpha != endAlpha)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01((spendTime + elapsedTime) / fadeTime));
            SetMaterialAlpha();
            yield return new WaitForEndOfFrame();
        }
       
    }

    private void SetMaterialAlpha()
    {
        Color color = fadeColor;
        color.a = currentAlpha;
        if (fadeMaterial != null)
        {
            fadeMaterial.color = color;
            fader.image.material = fadeMaterial;
            fader.image.color = color;
        }
    }
}
