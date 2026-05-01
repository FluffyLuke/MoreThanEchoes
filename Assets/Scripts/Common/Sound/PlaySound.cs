using System.Collections;
using UnityEngine;

public class PlaySound : MonoBehaviour {
    [SerializeField] private string id;
    [SerializeField] private float delay = 0f;
    public void Play() {
        StartCoroutine(playAmbient());
    }

    private IEnumerator playAmbient() {
        yield return new WaitForSeconds(delay);
        //Debug.Log($"Playing sound \"{id}\"");
        SoundManager.instance.PlayOneShot(id, Vector3.zero);
    }
}