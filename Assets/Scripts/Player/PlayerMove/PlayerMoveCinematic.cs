using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveDirection {
    Left,
    Right,
    Stop,
}

[RequireComponent(typeof(PlayerMove))]
public class PlayerMoveCinematic : MonoBehaviour {
    private PlayerMove moveComponent;
    [HideInInspector] public Vector2 currentDirection;
    [HideInInspector] public bool isCurrentRunning;
    [HideInInspector] public float customSpeed;
    void Awake() {
        moveComponent = GetComponent<PlayerMove>();
    }

    void OnEnable() {
        currentDirection = new Vector2(0,0);
        isCurrentRunning = false;
    }

    void Update() {
        moveComponent.Move(currentDirection, isCurrentRunning, customSpeed);
    }

    // This function should be called from other components, like "DoSomething"
    public void SetMove(MoveDirection direction, bool isRunning) {
        customSpeed = 0;

        if (direction == MoveDirection.Right) {
            currentDirection = new(1,0);
        } else if (direction == MoveDirection.Left) {
            currentDirection = new(-1,0);
        } else {
            currentDirection = Vector2.zero;
        }

        isCurrentRunning = isRunning;
    }

    public void SetMove(MoveDirection direction, float speed) {
        if (direction == MoveDirection.Right) {
            currentDirection = new(1,0);
        } else if (direction == MoveDirection.Left) {
            currentDirection = new(-1,0);
        } else {
            currentDirection = Vector2.zero;
        }

        customSpeed = speed;
    }
}