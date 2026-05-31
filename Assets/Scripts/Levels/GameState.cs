using UnityEngine;

public enum GameMoment {
    GoingForth,
    GoingBack,
}
public static class GameState {
    public static GameMoment currentMoment {
        get;
        private set;
    }

    public static void SetCurrentMoment(GameMoment moment) {
        Debug.Log($"Changing game moment to: {moment}");
        currentMoment = moment;
    }
}