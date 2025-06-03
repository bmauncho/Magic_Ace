using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FrameByFrameImageAnimator : MonoBehaviour
{
    [Tooltip("All animation frames in order.")]
    public Image [] frames;

    [Tooltip("Seconds per frame.")]
    public float frameDelay = 0.1f;

    [Tooltip("Should the animation loop?")]
    public bool loop = true;

    private int currentFrame = 0;
    private Coroutine animationCoroutine;

    void OnEnable ()
    {
        PlayAnimation();
    }

    void OnDisable ()
    {
        StopAnimation();
    }

    public void PlayAnimation ()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(Animate());
    }

    public void StopAnimation ()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        // Deactivate all frames
        foreach (var img in frames)
        {
            if (img != null) img.gameObject.SetActive(false);
        }
    }

    private IEnumerator Animate ()
    {
        while (true)
        {
            // Deactivate all images
            for (int i = 0 ; i < frames.Length ; i++)
            {
                frames [i].gameObject.SetActive(i == currentFrame);
            }

            yield return new WaitForSeconds(frameDelay);

            currentFrame = ( currentFrame + 1 ) % frames.Length;

            if (!loop && currentFrame == 0)
            {
                break;
            }
        }
    }
}
