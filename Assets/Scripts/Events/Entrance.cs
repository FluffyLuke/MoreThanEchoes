using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Entrance : MonoBehaviour {
    [Header("Init")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Blackness fade;
    public UnityEvent playerEntered = new();
    public string EntranceName = "";
    [Header("Time")]
    public float transitionTimeSecs = 3;
    public float moveForSecs = 3;
    [Header("Direction")]
    public MoveDirection direction;
    public void SpawnPlayer() {
        Vector2 pos2 = transform.position;
        GameObject playerObj = Instantiate(playerPrefab, pos2, Quaternion.identity);

        // Switch to cinematic mode (disable player input)
        PlayerBrain brain = playerObj.GetComponent<PlayerBrain>();
        brain.SwitchMode(PlayerMode.Cinematic);

        // Move player
        PlayerMoveCinematic moveCinematic = playerObj.GetComponent<PlayerMoveCinematic>();
        moveCinematic.SetMove(direction, false);

        // Set transition
        fade.TransitionOut(transitionTimeSecs, Ease.Linear, null);

        // Give player back the control
        StaticUtils.DoSomethingAfter(moveForSecs, this, () => {
            brain.SwitchMode(PlayerMode.Normal);
            playerEntered.Invoke();
        });
    }
}