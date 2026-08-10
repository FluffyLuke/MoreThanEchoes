using System;
using System.Collections;
using UnityEngine;

public class SoundHandle {
    public AudioSource source;
    public bool Destroyed {
        get;
        private set;
    }
    // public bool IsPlaying {
    //     get;
    //     private set;
    // }
    // private Coroutine coroutine = null;

    public SoundHandle(AudioSource source) {
        this.source = source;
    }

    // public void PlayOneShot(bool destroyWhenFinished, GameObject o) {
    //     IsPlaying = true;
    //     source.PlayOneShot(clip);
    //     if (coroutine != null) {
    //         coroutine = MonoBehaviour.StartCoroutine(DestroyDelay(), o);
    //     }
    // }

    public IEnumerator OnClipEnded(Action action) {
        yield return new WaitUntil(() => source.time >= source.clip.length);
        action.Invoke();
    }
    public void StopAndDestroy() {
        if (Destroyed) return;
        Destroyed = true;
        source.Stop();
        GameObject.Destroy(source.gameObject);
        //IsPlaying = false;
    }
    // public IEnumerator DestroyDelay() {
    //     yield return new WaitForSeconds(clip.length);
    //     StopAndDestroy();
    // }
}