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

    public void TransitionOut(float seconds, Ease ease = Ease.Linear, Action onComplete = null) {
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

    public void TransitionIn(float seconds, Ease ease = Ease.Linear, Action onComplete = null) {
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