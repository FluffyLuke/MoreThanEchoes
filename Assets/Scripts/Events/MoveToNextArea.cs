using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class MoveToNextArea : MonoBehaviour {
    public float transitionTimeSecs = 3;
    [SerializeField] private string sceneName = "Intro";
    [SerializeField] private string entranceName = "EntranceName";
    [SerializeField] private Blackness fade;
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
        brain.SwitchMode(PlayerMode.Cinematic);

        // Move to the right
        PlayerMoveCinematic move = playerObj.GetComponent<PlayerMoveCinematic>();
        move.SetMove(MoveDirection.Right, false);

        // Set transition
        fade.TransitionIn(transitionTimeSecs, Ease.Linear, () => {
            StaticUtils.ChangeLevel(sceneName, entranceName);
        });
    }
}
