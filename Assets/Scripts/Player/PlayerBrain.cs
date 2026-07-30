using UnityEngine;

public enum PlayerMode {
    Cinematic,
    Normal,
    Obstacle,
    Inspect,
}
public class PlayerBrain : MonoBehaviour
{
    public GameObject speechBubble;
    private PlayerMove move;
    private PlayerMoveCinematic moveCinematic;
    private PlayerMoveObstacle moveObstacle;
    private PlayerInspect inspect;
    private PlayerTorch torch;
    private PlayerLook look;
    private PlayerCheckObjective objective;
    private MonoBehaviour[] allComponents;
    void Awake() {
        // Get interactive components
        move = GetComponent<PlayerMove>();
        torch = GetComponent<PlayerTorch>();
        objective = GetComponent<PlayerCheckObjective>();
        moveCinematic = GetComponent<PlayerMoveCinematic>();
        moveObstacle = GetComponent<PlayerMoveObstacle>();
        inspect = GetComponent<PlayerInspect>();
        look = GetComponent<PlayerLook>();

        // Get all player components to a list
        allComponents = new MonoBehaviour[] {
            move,
            torch,
            objective,
            moveCinematic,
            moveObstacle,
            inspect,
            look,
        };

        PlayerEventBus.stateNormal.AddListener(NormalMode);
        PlayerEventBus.stateCinematic.AddListener(CinematicMode);
        PlayerEventBus.stateObstacle.AddListener(ObstacleMode);
        PlayerEventBus.stateInspect.AddListener(InspectMode);


        // Some of the games code spawns player and imidiatelly does something to it's state
        // This call MUST BE in awake
        NormalMode();
    }

    void Start() {
        PlayerEventBus.playerSpawned.Invoke();
    }

    private void turnOff() {
        // Disable all interactive components
        // They still can be used, but input variables should be disabled
        if (inspect.enabled) {
            // FIX: This is a quick patch. All player components need to have a proper interface
            // and stop rely on build-in methods like "OnEnable" and "OnDisable"
            inspect.ExitState();
        }
        
        foreach (var c in allComponents) {
            c.enabled = false;
        }
        PlayerEventBus.canInteract = false;
    }
    public void CinematicMode() {
        Debug.Log($"Switching player to mode: Cinematic");
        turnOff();

        moveCinematic.enabled = true;
    }
    public void NormalMode() {
        Debug.Log($"Switching player to mode: Normal");
        turnOff();

        PlayerEventBus.canInteract = true;
        move.enabled = true;
        torch.enabled = true;
        objective.enabled = true;
        look.enabled = true;
    }
    public void ObstacleMode() {
        Debug.Log($"Switching player to mode: Obstacle");
        turnOff();

        moveObstacle.enabled = true;
    }

    public void InspectMode(int number) {
        Debug.Log($"Switching player to mode: Inspect");
        turnOff();

        inspect.enabled = true;

        UIEventBus.transitionIn.Invoke(inspect.transitionTime, inspect.transitionEase, () => {
            inspect.EnterState(number);
            UIEventBus.transitionOut.Invoke(inspect.transitionTime, inspect.transitionEase, null);
        });
    }

    public void Die() {
        Debug.Log("Player died.");
        StaticUtils.ChangeLevel(LevelNames.GameOver, "");
    }

    public static Transform GetSpeechBubbleTransform() {
        return GameObject
            .FindGameObjectWithTag(Tags.PlayerTag)
            .GetComponent<PlayerBrain>()
            .speechBubble
            .transform;
    }
}
