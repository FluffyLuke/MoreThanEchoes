using UnityEngine;
using UnityEngine.InputSystem;

public class Skip3D : MonoBehaviour {
    private GameInput input;
    void Start() {
        input = new GameInput();
        input.Debug.Skip3D.performed += Skip;
        input.Debug.Enable();
    }
    public void Skip(InputAction.CallbackContext ctx) {
        PlayerInspect pi = GameObject.FindAnyObjectByType<PlayerInspect>();
        if (pi == null) {
            Debug.LogWarning("Cannot skip minigame.");
        }

        pi.StopInspecting();
    }
}