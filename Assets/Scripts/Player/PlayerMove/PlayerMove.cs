using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMove : MonoBehaviour {
    public float walkSpeed = 10;
    public float runSpeed = 20;
    private CharacterController2D controller;
    private PlayerLook look;
    [SerializeField] private GameObject body;
    [SerializeField] private Animator animator;
    private GameInput input;
    void Awake() {
        controller = GetComponent<CharacterController2D>();
        look = GetComponent<PlayerLook>();
        input = new GameInput();
    }

    void OnEnable() {
        input.Player.Enable();
    }

    void OnDisable() {
        controller.SetMotion(new(0,0));
        input.Player.Disable();
    }

    void Update() {
        moveInput();
    }

    [Header("Sound options")]
    public float walkStepDelta;
    public float sprintStepDelta;
    [SerializeField] private PlaySound walkSoundPlayer;
    private float moveT = 0;
    private void moveInput() {
        Vector2 moveBy = input.Player.Move.ReadValue<Vector2>();
        bool isSprinting = input.Player.Sprint.IsPressed();
        Move(moveBy, isSprinting);
    }

    // FIX: make footstep sounds be relative to speed
    public void Move(Vector2 direction, bool isSprinting, float customSpeed = 0) {
        animator.SetBool("isRunning", isSprinting);

        // Player is not moving at all
        if (direction.x == 0) {
            controller.SetMotion(new(0,0));
            moveT = 0;
            look.SetWhereToLook(WhereToLook.WhereLooking);
            animator.SetBool("isWalking", false);
            return;
        }

        animator.SetBool("isWalking", true);

        // Sound part
        moveT += Time.deltaTime;
        float currentDelta = isSprinting ? sprintStepDelta : walkStepDelta;
        
        if (moveT > currentDelta) {
            moveT -= currentDelta;
            walkSoundPlayer.Play();
        }

        // Set where model is facing

        // == Moving part == 
        // Rotate where player is going
        look.SetWhereToLook(direction.x > 0 ? WhereToLook.Right : WhereToLook.Left);
        float speed = direction.x * Time.deltaTime;
        
        if (customSpeed == 0) {
            speed *= isSprinting ? runSpeed : walkSpeed;
        } else {
            speed *= customSpeed;
        }

        controller.SetMotion(new(speed,0));
    }
}