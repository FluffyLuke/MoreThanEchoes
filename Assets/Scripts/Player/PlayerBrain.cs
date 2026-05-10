using UnityEngine;

public enum PlayerMode {
    Cinematic,
    Normal,
}
public class PlayerBrain : MonoBehaviour
{
    public GameObject speechBubble;
    private PlayerMove move;
    private PlayerTorch torch;
    private PlayerCheckObjective objective;
    private PlayerMoveCinematic moveCinematic;
    [SerializeField] private PlayerMode startingMode = PlayerMode.Normal;
    void Awake() {
        // Get interactive components
        move = GetComponent<PlayerMove>();
        torch = GetComponent<PlayerTorch>();
        objective = GetComponent<PlayerCheckObjective>();
        moveCinematic = GetComponent<PlayerMoveCinematic>();

        // Some of the games code spawns player and imidiatelly does something to it's state
        // This call MUST BE in awake
        SwitchMode(startingMode);
    }
    public void SwitchMode(PlayerMode mode) {
        // Disable all interactive components
        // They still can be used, but input variables should be disabled
        move.enabled = false;
        torch.enabled = false;
        moveCinematic.enabled = false;
        objective.enabled = false;

        Debug.Log($"Switching player to mode: {mode}");

        if (mode == PlayerMode.Cinematic) {
            cinematicMode();
        } else {
            normalMode();          
        }
    }
    private void cinematicMode() {
        moveCinematic.enabled = true;
    }
    private void normalMode() {
        move.enabled = true;
        torch.enabled = true;
        objective.enabled = true;
    }

    public static Transform GetSpeechBubbleTransform() {
        return GameObject
            .FindGameObjectWithTag(Tags.PlayerTag)
            .GetComponent<PlayerBrain>()
            .speechBubble
            .transform;
    }
}
