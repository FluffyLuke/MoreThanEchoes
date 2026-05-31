using UnityEngine;
using UnityEngine.Events;

// This is a generic component that should set the scene based on current GameMoment
// So for example, when player is going forward (first part of the game) it blocks paths behind him
// During the chase it's the opposite - it unlocks paths that lead to the entrance, and so on...
public class DoOnGameState : MonoBehaviour {
    public GameMoment doOnWhatMoment = GameMoment.GoingForth;
    public UnityEvent doOnAwake = new();
    void Awake() {
        if (doOnWhatMoment != GameState.currentMoment) return;
        
        Debug.Log($"Current game state: {GameState.currentMoment}");
        doOnAwake.Invoke();
    }
}