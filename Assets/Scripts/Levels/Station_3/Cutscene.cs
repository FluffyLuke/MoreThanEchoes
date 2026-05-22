using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour {
    [SerializeField] private GameObject[] scenes;
    public float timeBetweenScenesSecs = 2;
    public UnityEvent cutsceneEnded = new();
    void Start() {
        foreach (GameObject s in scenes) {
            s.SetActive(false);
        }
    }
    public void PlayCutscene() {
        StartCoroutine(playCutscene(timeBetweenScenesSecs));
    }

    private IEnumerator playCutscene(float wait) {
        PlayerEventBus.changeState.Invoke(PlayerMode.Cinematic);
        
        foreach (GameObject s in scenes) {
            s.SetActive(true);
            yield return new WaitForSeconds(wait);
        }

        foreach (GameObject s in scenes) {
            s.SetActive(false);
        }

        PlayerEventBus.changeState.Invoke(PlayerMode.Normal);
        cutsceneEnded.Invoke();
    }
}