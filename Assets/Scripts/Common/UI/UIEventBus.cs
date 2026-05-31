using System;
using DG.Tweening;
using UnityEngine.Events;

public static class UIEventBus {
    public static UnityEvent<float, Ease, Action> transitionOut = new();
    public static UnityEvent<float, Ease, Action> transitionIn = new();
    public static UnityEvent stopTransition = new();
}