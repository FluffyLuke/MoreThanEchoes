using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DoSomethingAfter : MonoBehaviour {
    public float defaultDelay = 1;
    [SerializeField] private bool runOnStart = true;
    public UnityEvent timeOut;
    private Coroutine coroutine;
    void Start() {
        if (runOnStart) {
            coroutine = StartCoroutine(display(defaultDelay));
        }
    }

    public void StartCounting(float delay = -1) {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        if (delay <= 0) {
            coroutine = StartCoroutine(display(defaultDelay));
            return;
        }
        coroutine = StartCoroutine(display(delay));
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

}