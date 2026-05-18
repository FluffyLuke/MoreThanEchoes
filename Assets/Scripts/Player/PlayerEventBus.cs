using UnityEngine;
using UnityEngine.Events;

public static class PlayerEventBus {
    public static bool canInteract = false;
    public static UnityEvent<string, float> spawnSpeechBubble = new();
    public static UnityEvent<PlayerMode> changeState = new();
    public static UnityEvent showNote = new();
    public static UnityEvent hideNote = new();
    // time
    public static UnityEvent<float> stun = new();
    // time, direction, speed
    public static UnityEvent<float, MoveDirection, float> stunAndMove = new();
    public static GameObject GetPlayer() {
        return GameObject.FindWithTag(Tags.PlayerTag);
    }
    public static T GetPlayerComponent<T>() {
        return GameObject.FindWithTag(Tags.PlayerTag).GetComponent<T>();
    }
}