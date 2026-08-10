using System.Collections;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    [SerializeField] private string id;
    [SerializeField] private float delay = 0f;
    public void Play(Vector3 pos = default) {
        StartCoroutine(playSound(pos));
    }

    public void Play() {
        Play(default);
    }

    private IEnumerator playSound(Vector3 pos) {
        yield return new WaitForSeconds(delay);
        // Debug.Log($"Playing sound \"{id}\"");
        var newParent = new GameObject("PlaySound");
        newParent.transform.parent = transform;
        newParent.transform.position = pos;
        SoundManager.instance.PlayOneShot(id, newParent);
    }
}