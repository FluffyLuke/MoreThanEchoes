using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class UseAction : MonoBehaviour {
    [SerializeField] private GameObject buttonSprite;
    private GameInput input;
    public UnityEvent interacted = new();
    void Awake() {
        buttonSprite.SetActive(false);
        input = new GameInput();
        input.Player.Interact.performed += interact;
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag(Tags.PlayerTag)) return;
        if (!enabled) return;

        buttonSprite.SetActive(true);
        input.Player.Interact.Enable();
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag(Tags.PlayerTag)) return;
        if (!enabled) return;

        buttonSprite.SetActive(false);
        input.Player.Interact.Disable();
    }

    void OnDisable() {
        buttonSprite.SetActive(false);
        input.Player.Interact.Disable();
    }

    private void interact(InputAction.CallbackContext ctx) {
        interacted.Invoke();
    }
}