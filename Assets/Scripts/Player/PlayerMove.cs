using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {
    public float walkSpeed = 10;
    public float runSpeed = 20;
    private CharacterController controller;
    [SerializeField] private GameObject body;
    private GameInput input;
    void Awake() {
        controller = GetComponent<CharacterController>();
        input = new GameInput();
    }

    void OnEnable() {
        input.Player.Enable();
    }

    void OnDisable() {
        input.Player.Disable();
    }

    void Update() {
        move();
    }

    private void move() {
        Vector2 moveBy = input.Player.Move.ReadValue<Vector2>();
        bool isSprinting = input.Player.Sprint.IsPressed();

        Vector3 newBodyScale = body.transform.localScale;
        if (moveBy.x > 0) newBodyScale.x = Mathf.Abs(newBodyScale.x);
        else if (moveBy.x < 0) newBodyScale.x = -Math.Abs(newBodyScale.x);
        body.transform.localScale = newBodyScale;


        float speed = moveBy.x * Time.deltaTime;
        speed *= isSprinting ? runSpeed : walkSpeed;

        controller.Move(new(speed,0,0));
    }
}