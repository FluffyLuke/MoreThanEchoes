using UnityEngine;

public class SaveState : MonoBehaviour {
    public float ambientTransitionSecs = 1;
    // A patchfix over the fact, that save can be done before the ambient transition.
    public string ambientID = null;
    void Start() {
        var objectives = FindObjectsByType<SaveState>();
        if (objectives.Length > 1) {
            Debug.LogWarning("Multiple save classes detected.");
        }

        if (!SaveManager.loadedSave) return;

        string ambientID = SaveManager.GetSavedAmbient();

        if (ambientID != null && ambientID != "") AmbientManager.instance.PlayAmbient(ambientID, ambientTransitionSecs);

        SaveManager.loadedSave = false;
    }
    public void SaveGame() {
        SaveManager.Save(ambientID);
    }
    public void LoadSave() {
        SaveManager.RestartFromSave();
    }
}