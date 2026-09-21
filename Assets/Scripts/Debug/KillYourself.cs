using UnityEngine;
using UnityEngine.InputSystem;

public class KillYourself : MonoBehaviour {
    private GameInput input;
    void Awake() {
        #if DEVELOPMENT_BUILD
        input = new GameInput();
        input.Debug.KYS.performed += KYS;
        input.Debug.Enable();
        #endif
    }

    // Kill yourself
    public void KYS(InputAction.CallbackContext ctx) {
        PlayerEventBus.GetPlayer().GetComponent<PlayerBrain>().DieInstant();
    }
}