using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SayCollider : MonoBehaviour {
    public string[] possibleQuotes;
    public float speedCPS = 10;
    public float cooldown = 3;
    private bool onCooldown = false;
    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(Tags.PlayerTag) || onCooldown) return;
        onCooldown = true;

        string randomQuote = possibleQuotes[Random.Range(0, possibleQuotes.Length - 1)];
        PlayerEventBus.spawnSpeechBubble.Invoke(randomQuote, speedCPS);

        StaticUtils.DoSomethingAfter(cooldown, this, () => onCooldown = false);
    }
}
