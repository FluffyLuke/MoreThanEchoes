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
    private SoundAsset sound;

    void Start() {
        if (!AmbientManager.instance.lookup.TryGetValue(id, out sound))
        {
            Debug.LogError($"Cannot found asset of id: \"{id}\"");
            Destroy(gameObject);
        }

        if (playOnStart) {
            StartCoroutine(playAmbient());
        }
    }
    public void Play() {
        StartCoroutine(playAmbient());
    }

    private IEnumerator playAmbient() {
        yield return new WaitForSeconds(delay);
        AmbientManager.instance.PlayAmbient(sound, fadeDuration);
    }
}