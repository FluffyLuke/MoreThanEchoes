using UnityEngine;

public class PlayerSpawnSpeechBubble : MonoBehaviour {
    [SerializeField] private GameObject bubblePrefab;
    void Start() {
        PlayerEventBus.spawnSpeechBubble.AddListener(Spawn);
    }
    public void Spawn(string whatToShow, float speed) {
        // Destroy all of children (previous speech bubbles) before spawning new speech bubble
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        Transform speechBubblePoint = PlayerBrain.GetSpeechBubbleTransform();

        var bubble = Instantiate(bubblePrefab, gameObject.transform).GetComponent<SpeechBubble>();
        bubble.point = speechBubblePoint;
        bubble.ShowText(whatToShow, speed);
    }
}