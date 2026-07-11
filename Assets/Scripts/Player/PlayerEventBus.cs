using UnityEngine;
using UnityEngine.Events;

public static class PlayerEventBus {
    public static bool canInteract = false;
    public static UnityEvent<string, float> spawnSpeechBubble = new();
    public static UnityEvent showNote = new();
    public static UnityEvent hideNote = new();
    // time
    public static UnityEvent<float> stun = new();
    // time, direction, speed
    public static UnityEvent<float, MoveDirection, float> stunAndMove = new();

    // == Player helper functions ===
    public static GameObject GetPlayer() {
        return GameObject.FindWithTag(Tags.PlayerTag);
    }
    public static T GetPlayerComponent<T>() {
        return GameObject.FindWithTag(Tags.PlayerTag).GetComponent<T>();
    }

    // === Player states ===
    public static UnityEvent stateCinematic = new();
    public static UnityEvent stateNormal = new();
    public static UnityEvent stateObstacle = new();
    public static UnityEvent<int> stateInspect = new();
    public static UnityEvent finishInspecting = new();
}