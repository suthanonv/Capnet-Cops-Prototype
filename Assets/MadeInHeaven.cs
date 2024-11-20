using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadeInHeaven : MonoBehaviour
{
    public void Pucci()
    {

        if (Time.timeScale <= 1.5f)
        {
            ChangeTimeScaleWithReturn(15f, 1f, 0f);
        }
        else if (Time.timeScale > 1.5f)
        {
            ChangeTimeScaleWithReturn(1.5f, 1f, 0f);
        }
    }
    public void ChangeTimeScaleWithReturn(float targetTimeScale, float transitionDuration, float waitDuration)
    {
        StartCoroutine(TransitionTimeScale(targetTimeScale, transitionDuration, waitDuration));
    }

    private System.Collections.IEnumerator TransitionTimeScale(float targetTimeScale, float transitionDuration, float waitDuration)
    {
        float startScale = 1.5f;

        // Transition to the target time scale
        yield return StartCoroutine(Transition(startScale, targetTimeScale, transitionDuration));

        // Wait for the specified duration
        yield return new WaitForSecondsRealtime(waitDuration);

        // Transition back to the original time scale
        yield return StartCoroutine(Transition(targetTimeScale, startScale, transitionDuration));
    }

    private System.Collections.IEnumerator Transition(float from, float to, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to avoid timeScale interference
            float t = elapsedTime / duration;

            // Ease-In-Out (Smoothstep)
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Adjust timeScale using the easing function
            Time.timeScale = Mathf.Lerp(from, to, smoothT);

            yield return null; // Wait for the next frame
        }

        Time.timeScale = to; // Ensure the final value is set
    }
}
