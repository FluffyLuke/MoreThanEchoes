using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Blackness : MonoBehaviour {
    private Image blackness;
    private Tween tween;    
    void Awake() {
        blackness = GetComponent<Image>();
    }

    // This can be used in editor
    public void TransitionOut(float seconds) {
        TransitionOut(seconds, Ease.Linear, null);
    }

    public void TransitionOut(float seconds, Ease ease, Action onComplete) {
        tween?.Kill();

        blackness.color = new Color(0,0,0,1);
        tween = blackness
            .DOColor(new Color(0,0,0,0), seconds)
            .SetEase(ease)
            .OnComplete(() => {
                if (onComplete != null) onComplete.Invoke();
                tween = null;
            });
    }

    public void TransitionIn(float seconds) {
        TransitionOut(seconds);
    }

    public void TransitionIn(float seconds, Ease ease, Action onComplete) {
        tween?.Kill();

        blackness.color = new Color(0,0,0,0);
        tween = blackness
            .DOColor(new Color(0,0,0,1), seconds)
            .SetEase(ease)
            .OnComplete(() => {
                if (onComplete != null) onComplete.Invoke();
                tween = null;
            });
    }

    public void StopTransition() {
        tween?.Kill();
        blackness.color = new Color(0,0,0,0);
    }
}