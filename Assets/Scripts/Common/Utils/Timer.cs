using System.Collections;
using UnityEngine;

public struct Timer {
    public float timeToWait;
    public float t;
    public Timer(float timeToWait) {
        this.timeToWait = timeToWait;
        t = 0;
    }

    public void UpdateTimer() {
        t += Time.deltaTime;
    }

    public bool Finished() {
        return timeToWait <= t;
    }

    public void ResetTimer(float newTime = 0) {
        if (newTime > 0) {
            timeToWait = newTime;
        }

        t = 0;
    }
}