using UnityEngine;

public abstract class AbstractFadeCameraEffect : MonoBehaviour, IStartup
{
    [SerializeField]
    protected Color fadeColor = new Color(0f, 0f, 0f, 1f);

    [SerializeField]
    protected float fadeTime = 2f;

    public abstract void Startup();

    public abstract void FadeIn();

    public abstract void FadeOut();

    public abstract void InstantFadeIn();

    public abstract void InstantFadeOut();

}