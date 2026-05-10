using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCheckObjective : MonoBehaviour {
    private GameInput input;
    void Awake() {
        input = new GameInput();
        input.Player.CheckObjective.performed += checkObjectives;
    }
    void OnEnable() {
        input.Player.Enable();
    }
    void OnDisable() {
        input.Player.Disable();
    }

    private void checkObjectives(InputAction.CallbackContext ctx) {
        ObjectiveUI.instance.Show();
    }
}