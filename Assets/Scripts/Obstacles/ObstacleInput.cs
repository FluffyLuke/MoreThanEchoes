using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ObstacleInput : MonoBehaviour {
    private InputActionReference currentGoodAction;
    [SerializeField] private InputActionReference[] allActions;
    // InputActionReference is a shared asset, so additional check must be used
    private bool gateEnabled = false;
    public UnityEvent onInput;
    public UnityEvent onBadInput;
    void Start() {
        SetActionIndex(0);
    }
    public void SetActionIndex(int index) {
        // Remove all current triggers
        foreach (var a in allActions) {
            // a.action.Disable();
            a.action.performed -= onBadTrigger;
            a.action.performed -= onTrigger;
        }

        // Assign good action
        currentGoodAction = allActions[index];
        currentGoodAction.action.performed += onTrigger;
        // currentGoodAction.action.Enable();

        // Assign bad actions
        foreach (var a in allActions) {
            if (a == currentGoodAction) continue;

            a.action.performed += onBadTrigger;
            // a.action.Enable();
        }
    }
    public void TurnOn() {
        foreach(var a in allActions) {
            a.action.Enable();
        }
    }

    public void TurnOff() {
        foreach(var a in allActions) {
            a.action.Disable();
        }
    }
    private void onTrigger(InputAction.CallbackContext ctx) {
        Debug.Log($"Player pressed right button! {name} {transform?.parent.name}");
        if (gateEnabled) onInput.Invoke();
    }
    private void onBadTrigger(InputAction.CallbackContext ctx) {
        Debug.Log($"Player pressed wrong button! {name} {transform?.parent.name}");
        if (gateEnabled) onBadInput.Invoke();
    }
    void OnDisable() {
        gateEnabled = false;
    }
    void OnTriggerEnter2D(Collider2D collision) {
        // Debug.LogWarning($"DEBUG: Entered collider: {gameObject.name}");
        gateEnabled = true;
    }
    void OnTriggerExit2D(Collider2D collision) {
        // Debug.LogWarning($"DEBUG: Exited collider: {gameObject.name}");
        gateEnabled = false;
    }
}