using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpeedDebug : MonoBehaviour {
    private GameInput input;
    void Awake() {
        input = new GameInput();
        input.Debug.MoreSpeed.performed += moreSpeed;
        input.Debug.LessSpeed.performed += lessSpeed;

        input.Debug.Enable();
    }

    private void moreSpeed(InputAction.CallbackContext ctx) {
        PlayerMove move = GameObject.FindGameObjectWithTag(Tags.PlayerTag).GetComponent<PlayerMove>();
        move.walkSpeed *= 2;
        move.runSpeed *= 2;
    }

    private void lessSpeed(InputAction.CallbackContext ctx) {
        PlayerMove move = GameObject.FindGameObjectWithTag(Tags.PlayerTag).GetComponent<PlayerMove>();
        move.walkSpeed /= 2;
        move.runSpeed /= 2;
    }
}