using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerTorch : MonoBehaviour {
    [SerializeField] private Light2D torchBeam;
    [SerializeField] private Light2D torchAura;
    [SerializeField] private PlaySound soundPlayer;
    private GameInput input;
    private bool state = true;
    private float beamIntensity;
    private float auraIntensity;
    void Awake() {
        input = new GameInput();
        input.Player.Torch.performed += switchTorchCallback;
    }
    void Start() {
        beamIntensity = torchBeam.intensity;
        auraIntensity = torchAura.intensity;
    }
    void OnEnable() {
        input.Player.Enable();
    }
    void OnDisable() {
        input.Player.Disable();
    }
    private void switchTorchCallback(InputAction.CallbackContext ctx) {
        SwitchTorch(!state);
        soundPlayer.Play(transform.position);
    }
    public void SwitchTorch(bool newState) {
        Debug.Log($"Switching flashlight to {newState}");
        torchBeam.intensity = newState ? beamIntensity : 0;
        torchAura.intensity = newState ? auraIntensity : 0;

        state = newState;
    }
}