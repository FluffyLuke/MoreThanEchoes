using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class UseAction : MonoBehaviour {
    [SerializeField] private GameObject buttonSprite;
    private GameInput input;
    public UnityEvent interacted = new();
    void Awake() {
        input = new GameInput();
        input.Player.Interact.performed += interact;
    }
    void Start() {
        buttonSprite.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag(Tags.PlayerTag)) return;
        buttonSprite.SetActive(true);
        input.Player.Interact.Enable();
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag(Tags.PlayerTag)) return;
        buttonSprite.SetActive(false);
        input.Player.Interact .Disable();
    }

    void OnDisable() {
        buttonSprite.SetActive(false);
    }

    private void interact(InputAction.CallbackContext ctx) {
        interacted.Invoke();
    }
}