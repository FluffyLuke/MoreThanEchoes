using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DoSomethingAfter : MonoBehaviour {
    public float defaultDelay = 1;
    [SerializeField] private bool runOnStart = false;
    public UnityEvent timeOut;
    private Coroutine coroutine;
    void Start() {
        if (runOnStart) {
            coroutine = StartCoroutine(display(defaultDelay));
        }
    }

    public void StartCounting() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(display(defaultDelay));
    }

    public void Skip() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        timeOut.Invoke();
    }

    private IEnumerator display(float delay) {
        yield return new WaitForSeconds(delay);
        timeOut.Invoke();
    }

    public static void After(MonoBehaviour caller, float timeSecs, Action action) {
        caller.StartCoroutine(doAfter(timeSecs, action));
    }

    private static IEnumerator doAfter(float timeSecs, Action action) {
        yield return new WaitForSeconds(timeSecs);
        action.Invoke();
    }
}