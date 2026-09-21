using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class MinigameHint : MonoBehaviour {
    [HideInInspector] public CanvasGroup group;
    public float showDuration = 1;
    public float showTime = 5;
    public float hideDuration = 1;
    Sequence fadeSequence;
    
    void Start() {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
        PlayerEventBus.stateInspect.AddListener(show);
        PlayerEventBus.finishInspecting.AddListener(hide);
    }

    private void show(int _) {
        group.alpha = 0;

        fadeSequence?.Kill();
        fadeSequence = DOTween.Sequence();
        fadeSequence.AppendInterval(1);
        fadeSequence.Append(group.DOFade(1, showDuration));
        fadeSequence.AppendInterval(showTime);
        fadeSequence.Append(group.DOFade(0, hideDuration));
    }

    private void hide() {
        fadeSequence?.Kill();
        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(group.DOFade(0, hideDuration));
    }
}