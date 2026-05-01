using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PlayAmbient : MonoBehaviour {
    [SerializeField] private string id;
    [SerializeField] private float delay = 0f;
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private bool playOnStart = false;

    void Start() {
        if (playOnStart) {
            StartCoroutine(playAmbient());
        }
    }
    public void Play() {
        StartCoroutine(playAmbient());
    }

    private IEnumerator playAmbient() {
        yield return new WaitForSeconds(delay);
        AmbientManager.instance.PlayAmbient(id, fadeDuration);
    }
}