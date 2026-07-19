using UnityEngine;
using UnityEngine.Events;

// This is a generic component that should set the scene based on current GameMoment
// So for example, when player is going forward (first part of the game) it blocks paths behind him
// During the chase it's the opposite - it unlocks paths that lead to the entrance, and so on...
public class DoOnGameState : MonoBehaviour {
    public GameMoment doOnWhatMoment = GameMoment.GoingForth;
    public UnityEvent doOnAwake = new();
public UnityEvent doOnStart = new();
    public UnityEvent doOnStateUpdate = new();
    void Awake() {
        GameState.onUpdate.AddListener(updateState);

        if (doOnWhatMoment == GameState.currentMoment) doOnAwake.Invoke();;
    }

    void Start() {
        if (doOnWhatMoment == GameState.currentMoment) doOnStart.Invoke();;
    }

    private void updateState(GameMoment moment) {
        if (moment == doOnWhatMoment) {
            doOnStateUpdate.Invoke();
        }
    }
}