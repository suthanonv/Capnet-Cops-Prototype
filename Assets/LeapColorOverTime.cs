using System.Collections;
using UnityEngine;

public class LeapColorOverTime : MonoBehaviour
{
    [SerializeField] private Color DefaultColor = Color.white;
    [SerializeField] private Color HighlightColor = Color.black;
    public float LeapSpeed = 1f;

    private SpriteRenderer _spriteRenderer;
    private Coroutine colorCoroutine;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this object.");
            return;
        }

        // If DefaultColor is white (unset), use the current sprite color
        if (DefaultColor == Color.white)
        {
            DefaultColor = _spriteRenderer.color;
        }
    }

    private void OnEnable()
    {
        if (colorCoroutine == null && _spriteRenderer != null)
        {
            colorCoroutine = StartCoroutine(UpdateColor());
        }
    }

    private void OnDisable()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
            colorCoroutine = null;
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = DefaultColor; // Reset to default when disabled
        }
    }

    private IEnumerator UpdateColor()
    {
        float currentTime = 0;

        while (true)
        {
            if (_spriteRenderer != null)
            {
                // Ping-pong between DefaultColor and HighlightColor
                Color leapedColor = Color.Lerp(DefaultColor, HighlightColor, Mathf.PingPong(currentTime, 1f));
                _spriteRenderer.color = leapedColor;
                currentTime += Time.deltaTime * LeapSpeed;
            }

            yield return null; // Wait for the next frame
        }
    }
}
