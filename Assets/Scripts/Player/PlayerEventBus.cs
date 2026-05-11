using UnityEngine;
using UnityEngine.Events;

public static class PlayerEventBus {
    public static bool canInteract = false;
    public static UnityEvent<string, float> spawnSpeechBubble = new();
    public static UnityEvent<PlayerMode> changeState = new();
    public static UnityEvent showNote = new();
    public static UnityEvent hideNote = new();
}