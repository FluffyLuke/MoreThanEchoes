using UnityEngine;

public enum PlayerMode {
    Cinematic,
    Normal,
    Obstacle,
}
public class PlayerBrain : MonoBehaviour
{
    public GameObject speechBubble;
    private PlayerMove move;
    private PlayerMoveCinematic moveCinematic;
    private PlayerMoveObstacle moveObstacle;
    private PlayerTorch torch;
    private PlayerCheckObjective objective;
    [SerializeField] private PlayerMode startingMode = PlayerMode.Normal;
    void Awake() {
        // Get interactive components
        move = GetComponent<PlayerMove>();
        torch = GetComponent<PlayerTorch>();
        objective = GetComponent<PlayerCheckObjective>();
        moveCinematic = GetComponent<PlayerMoveCinematic>();
        moveObstacle = GetComponent<PlayerMoveObstacle>();

        PlayerEventBus.changeState.AddListener(SwitchMode);

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
        moveObstacle.enabled = false;
        PlayerEventBus.canInteract = false;

        Debug.Log($"Switching player to mode: {mode}");

        switch (mode) {
            case PlayerMode.Normal:
                normalMode();
                break;
            case PlayerMode.Cinematic:
                cinematicMode();
                break;
            case PlayerMode.Obstacle:
                obstacleMode();
                break;
        }
    }
    private void cinematicMode() {
        moveCinematic.enabled = true;
    }
    private void normalMode() {
        PlayerEventBus.canInteract = true;
        move.enabled = true;
        torch.enabled = true;
        objective.enabled = true;
    }
    private void obstacleMode() {
        moveObstacle.enabled = true;
    }

    public static Transform GetSpeechBubbleTransform() {
        return GameObject
            .FindGameObjectWithTag(Tags.PlayerTag)
            .GetComponent<PlayerBrain>()
            .speechBubble
            .transform;
    }
}
