using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData {
    public GameMoment moment;
    public string level;
    public string entrance;
    public string ambient;
}

public static class SaveManager {
    private static SaveData save = null;
    // This is used by the "SaveState" class to determine if save was loaded.
    // It is responsible for setting this to "false" once it is done with it.
    // This is necessary, since some things need to be done AFTER the scene loads, like playing ambient.
    public static bool loadedSave = false; 
    public static void RestartFromSave() {
        if (save == null) {
            Debug.LogError("No save found.");
            return;
        }

        loadedSave = true;
        GameState.SetCurrentMoment(save.moment);
        StaticUtils.ChangeLevel(save.level, save.entrance);
    }

    public static string GetSavedAmbient() {
        if (save == null) {
            Debug.LogError("No save found.");
            return null;
        }

        return save.ambient;
    }

    public static void Save(string ambient = null) {
        save = new SaveData();
        save.moment = GameState.currentMoment;
        save.level = SceneManager.GetActiveScene().name;
        save.entrance = StaticUtils.GetEntranceName();
        save.ambient = ambient != null ? ambient : AmbientManager.lastAmbientID;
        Debug.Log("Saved game!");
        Debug.Log($"Moment: '{save.moment}'");
        Debug.Log($"Level name: '{save.level}'");
        Debug.Log($"Entrance id: '{save.entrance}'");
        Debug.Log($"Ambient id: '{save.ambient}'");
    }
}