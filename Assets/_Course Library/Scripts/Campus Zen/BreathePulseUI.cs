using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class BreathPulseUI : MonoBehaviour
{
    [Header("Size")]
    public float minScale = 0.8f;
    public float maxScale = 1.25f;

    [Header("Opacity (Alpha)")]
    [Range(0f, 1f)] public float alphaAtMin = 1f;     // exhale end
    [Range(0f, 1f)] public float alphaAtMax = 0.35f;  // inhale end

    [Header("Timing (seconds)")]
    public float inhaleDuration = 4f;
    public float holdAfterInhale = 0.5f;
    public float exhaleDuration = 4f;
    public float holdAfterExhale = 0.5f;

    [Header("Easing")]
    public AnimationCurve inhaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve exhaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Labels (assign one)")]
    public Text uiText;          // UGUI
    public TMP_Text tmpText;     // TextMeshPro

    public string inhaleLabel = "breathe in";
    public string exhaleLabel = "breathe out";

    [Header("Label Fade")]
    [Tooltip("Fade-in time at the start of inhale/exhale.")]
    public float labelFadeInDuration = 0.6f;
    [Tooltip("Fade-out time finishing exactly at the start of the hold.")]
    public float labelFadeOutDuration = 0.6f;

    [Header("Behavior")]
    public bool useUnscaledTime = true;
    public bool playOnEnable = true;

    RectTransform rt;
    Image img;
    bool running;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        // Auto-find if not assigned
        if (tmpText == null) tmpText = GetComponentInChildren<TMP_Text>(true);
        if (uiText == null) uiText = GetComponentInChildren<Text>(true);
    }

    void OnEnable() { if (playOnEnable) StartBreathing(); }
    void OnDisable() { StopBreathing(); }

    public void StartBreathing()
    {
        if (running) return;
        running = true;
        StopAllCoroutines();
        StartCoroutine(BreathLoop());
    }

    public void StopBreathing()
    {
        if (!running) return;
        running = false;
        StopAllCoroutines();
    }

    IEnumerator BreathLoop()
    {
        ApplyCircle(minScale, alphaAtMin);
        SetLabelAlpha(0f); // hidden at rest
        SetLabelText(exhaleLabel); // next phase will override anyway

        while (running)
        {
            // INHALE: grow + fade out circle; label fades in then out
            SetLabelText(inhaleLabel);
            yield return AnimatePhase(
                duration: inhaleDuration,
                curve: inhaleCurve,
                scaleFrom: minScale, scaleTo: maxScale,
                alphaFrom: alphaAtMin, alphaTo: alphaAtMax,
                doLabelFade: true
            );

            if (holdAfterInhale > 0f)
                yield return Hold(holdAfterInhale);

            // EXHALE: shrink + fade in circle; label fades in then out
            SetLabelText(exhaleLabel);
            yield return AnimatePhase(
                duration: exhaleDuration,
                curve: exhaleCurve,
                scaleFrom: maxScale, scaleTo: minScale,
                alphaFrom: alphaAtMax, alphaTo: alphaAtMin,
                doLabelFade: true
            );

            if (holdAfterExhale > 0f)
                yield return Hold(holdAfterExhale);
        }
    }

    IEnumerator AnimatePhase(float duration, AnimationCurve curve,
                             float scaleFrom, float scaleTo,
                             float alphaFrom, float alphaTo,
                             bool doLabelFade)
    {
        duration = Mathf.Max(0.0001f, duration);

        // Sanity for fade windows: ensure they fit the phase
        float maxHalf = duration * 0.5f; // avoid overlap
        float fadeIn = Mathf.Clamp(labelFadeInDuration, 0f, maxHalf);
        float fadeOut = Mathf.Clamp(labelFadeOutDuration, 0f, maxHalf);

        // Start of phase: label begins faded out; fade in
        if (doLabelFade) SetLabelAlpha(fadeIn > 0f ? 0f : 1f);

        float t = 0f;
        while (t < duration)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;
            float u = Mathf.Clamp01(t / duration);
            float e = curve.Evaluate(u);

            // Circle scale + alpha
            float scale = Mathf.Lerp(scaleFrom, scaleTo, e);
            float circleA = Mathf.Lerp(alphaFrom, alphaTo, e);
            ApplyCircle(scale, circleA);

            // Label alpha (fade in at start, fade out before hold)
            if (doLabelFade)
            {
                float labelA = 1f;

                // Fade in [0, fadeIn]
                if (t <= fadeIn && fadeIn > 0f)
                {
                    labelA = Mathf.InverseLerp(0f, fadeIn, t);
                }
                // Fade out [duration - fadeOut, duration]
                else if (t >= duration - fadeOut && fadeOut > 0f)
                {
                    labelA = 1f - Mathf.InverseLerp(duration - fadeOut, duration, t);
                }
                else
                {
                    labelA = 1f;
                }

                SetLabelAlpha(labelA);
            }

            yield return null;
        }

        // End of phase: ensure label is fully hidden entering hold
        if (doLabelFade) SetLabelAlpha(0f);
    }

    IEnumerator Hold(float seconds)
    {
        if (seconds <= 0f) yield break;
        float t = 0f;
        SetLabelAlpha(0f); // keep hidden during hold
        while (t < seconds)
        {
            t += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            yield return null;
        }
    }

    void ApplyCircle(float scale, float alpha)
    {
        rt.localScale = new Vector3(scale, scale, 1f);
        var c = img.color; c.a = Mathf.Clamp01(alpha); img.color = c;
    }

    void SetLabelText(string s)
    {
        if (tmpText) tmpText.text = s;
        if (uiText) uiText.text = s;
    }

    void SetLabelAlpha(float a)
    {
        a = Mathf.Clamp01(a);
        if (tmpText)
        {
            // Use vertex alpha where possible
            var c = tmpText.color; c.a = a; tmpText.color = c;
            tmpText.alpha = a; // TMP also respects this
        }
        if (uiText)
        {
            var c = uiText.color; c.a = a; uiText.color = c;
        }
    }
}
