using UnityEngine;
using UnityEngine.Events;

public enum GameMoment {
    GoingForth,
    GoingBack,
}
public static class GameState {
    public static UnityEvent<GameMoment> onUpdate = new();
    public static GameMoment currentMoment {
        get;
        private set;
    } = GameMoment.GoingForth;

    public static void SetCurrentMoment(GameMoment moment) {
        Debug.Log($"Changing game moment to: {moment}");
        currentMoment = moment;
        onUpdate.Invoke(currentMoment);
    }
}