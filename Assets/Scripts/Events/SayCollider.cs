using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SayCollider : MonoBehaviour {
    public string[] possibleQuotes;
    public float speedCPS = 10;
    void OnTriggerEnter2D(Collider2D other) {
        string randomQuote = possibleQuotes[Random.Range(0, possibleQuotes.Length - 1)];

        Debug.Log($"Spawning Quote {randomQuote}");

        PlayerEventBus.spawnSpeechBubble.Invoke(randomQuote, speedCPS);
    }
}
