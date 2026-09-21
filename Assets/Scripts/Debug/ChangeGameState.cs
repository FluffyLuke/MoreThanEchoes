using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeGameState : MonoBehaviour {
    private GameInput input;
    void Awake() {
        #if DEVELOPMENT_BUILD
        input = new GameInput();
        input.Debug.ChangeGameMoment.performed += Change;
        input.Debug.Enable();
        #endif
    }
    public void Change(InputAction.CallbackContext ctx) {
        if (GameState.currentMoment == GameMoment.GoingForth) {
            Debug.Log("DEBUG: Changing moment to 'GOING BACK'");
            GameState.SetCurrentMoment(GameMoment.GoingBack);
        } else {
            Debug.Log("DEBUG: Changing moment to 'GOING FORTH'");
            GameState.SetCurrentMoment(GameMoment.GoingForth);
        }
    }
}