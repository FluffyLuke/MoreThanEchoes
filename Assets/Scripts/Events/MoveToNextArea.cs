using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class MoveToNextArea : MonoBehaviour {
    public float transitionTimeSecs = 3;
    [SerializeField] private string sceneName = "Intro";
    [SerializeField] private string entranceName = "EntranceName";
    [SerializeField] private MoveDirection direction = MoveDirection.Right;
    [SerializeField] private bool running = false;
    void Awake() {
        // var scene = SceneManager.GetSceneByName(sceneName);
        // if (!scene.IsValid()) {
        //     Debug.LogError($"Cannot find scene '{sceneName}'");
        //     gameObject.SetActive(false); // Disable script and whole object, since scene is bad
        // }
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag(Tags.PlayerTag)) return;
        GameObject playerObj = collision.gameObject;

        // Switch to cinematic mode (disable player input)
        PlayerBrain brain = playerObj.GetComponent<PlayerBrain>();
        brain.CinematicMode();

        // Move to the right
        PlayerMoveCinematic move = playerObj.GetComponent<PlayerMoveCinematic>();
        move.SetMove(direction, running);

        // Set transition
        UIEventBus.transitionIn.Invoke(transitionTimeSecs, Ease.Linear, () => {
            StaticUtils.ChangeLevel(sceneName, entranceName);
        });
    }
}
