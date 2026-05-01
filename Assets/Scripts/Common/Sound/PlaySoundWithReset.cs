using System.Collections;
using UnityEngine;

public class PlaySoundWithReset : MonoBehaviour {
    [SerializeField] private string id;
    [SerializeField] private float delay = 0f;
    private Coroutine coroutine;
    private SoundHandle currentHandle;
    public void Play() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        if (currentHandle != null) {
            currentHandle.StopAndDestroy();
            currentHandle = null;
        }

        coroutine = StartCoroutine(playAmbient());
    }

    private IEnumerator playAmbient() {
        yield return new WaitForSeconds(delay);
        Debug.Log($"Playing sound \"{id}\"");
        SoundManager.instance.PlayOneShot(id, Vector3.zero, out SoundHandle handle);
        currentHandle = handle;
        coroutine = null;
    }
}