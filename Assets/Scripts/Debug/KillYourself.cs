using UnityEngine;
using UnityEngine.InputSystem;

public class KillYourself : MonoBehaviour {
    private GameInput input;
    void Awake() {
        input = new GameInput();
        input.Debug.KYS.performed += KYS;
        input.Debug.Enable();
    }

    // Kill yourself
    public void KYS(InputAction.CallbackContext ctx) {
        PlayerEventBus.GetPlayer().GetComponent<PlayerBrain>().Die();
    }
}