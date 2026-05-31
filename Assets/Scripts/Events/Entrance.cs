using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Entrance : MonoBehaviour {
    [Header("Init")]
    [SerializeField] private GameObject playerPrefab;
    public string EntranceName = "";
    [Header("Speed")]
    public float transitionTimeSecs = 3;
    public float walkingSecs = 3;
    public float runningSecs = 1.5f;
    [Header("Where")]
    public MoveDirection direction;
    public bool isRunning = false;
    [Header("Events")]
    public UnityEvent playerStartedEntering = new();
    public UnityEvent playerEntered = new();
    public void SpawnPlayer() {
        playerStartedEntering.Invoke();

        Vector2 pos2 = transform.position;
        GameObject playerObj = Instantiate(playerPrefab, pos2, Quaternion.identity);

        // Switch to cinematic mode (disable player input)
        PlayerBrain brain = playerObj.GetComponent<PlayerBrain>();
        brain.SwitchMode(PlayerMode.Cinematic);

        // Move player
        PlayerMoveCinematic moveCinematic = playerObj.GetComponent<PlayerMoveCinematic>();
        moveCinematic.SetMove(direction, isRunning);

        // Set transition
        UIEventBus.transitionOut.Invoke(transitionTimeSecs, Ease.Linear, null);

        // Give player back the control
        StaticUtils.DoSomethingAfter(isRunning ? runningSecs : walkingSecs, this, () => {
            brain.SwitchMode(PlayerMode.Normal);
            playerEntered.Invoke();
        });
    }
}