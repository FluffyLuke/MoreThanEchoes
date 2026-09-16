using System.Collections;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    [SerializeField] private string id;
    [SerializeField] private float delay = 0f;
    public bool persist = false;
    public void Play(Vector3 pos = default, Transform parent = null) {
        StartCoroutine(playSound(pos, parent));
    }

    public void Play() {
        if (persist) {
            Play(pos: default, parent: SoundManager.instance.transform);
        } else {
            Play(default, null);
        }
    }

    private IEnumerator playSound(Vector3 pos, Transform parent) {
        yield return new WaitForSeconds(delay);
        // Debug.Log($"Playing sound \"{id}\"");
        var newParent = new GameObject("PlaySound");

        if (parent != null) {
            newParent.transform.parent = parent;
        } else {
            newParent.transform.parent = transform;
        }
        newParent.transform.position = pos;
        SoundManager.instance.PlayOneShot(id, newParent);
    }
}