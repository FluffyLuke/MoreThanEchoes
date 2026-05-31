using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ObstacleInput : MonoBehaviour {
    [SerializeField] private InputActionReference action;
    [SerializeField] private InputActionReference[] badActions;
    // InputActionReference is a shared asset, so additional check must be used
    private bool gateEnabled = false;
    public UnityEvent onInput;
    public UnityEvent onBadInput;
    void Start() {
        action.action.Enable();
        action.action.performed += onTrigger;

        foreach (var a in badActions) {
            a.action.Enable();
            a.action.performed += onBadTrigger;
        }
    }
    private void onTrigger(InputAction.CallbackContext ctx) {
        if (gateEnabled) onInput.Invoke();
    }
    private void onBadTrigger(InputAction.CallbackContext ctx) {
        if (gateEnabled) onBadInput.Invoke();
    }
    void OnDisable() {
        gateEnabled = false;
    }
    void OnTriggerEnter2D(Collider2D collision) {
        gateEnabled = true;
    }
    void OnTriggerExit2D(Collider2D collision) {
        gateEnabled = false;
    }
}