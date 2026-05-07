using UnityEngine;
using UnityEngine.Events;

public static class PlayerEventBus {
    public static UnityEvent<string, float> spawnSpeechBubble = new();
}