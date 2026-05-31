using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeGameState : MonoBehaviour {
    private GameInput input;
    void Awake() {
        input = new GameInput();
        input.Debug.MoveBack.performed += MoveBack;
        input.Debug.Enable();
    }
    public void MoveBack(InputAction.CallbackContext ctx) {
        GameState.SetCurrentMoment(GameMoment.GoingBack);
    }
}