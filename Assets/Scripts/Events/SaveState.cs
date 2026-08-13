using UnityEngine;

public class SaveState : MonoBehaviour {
    public float ambientTransitionSecs = 1;
    // A patchfix over the fact, that save can be done before the ambient transition.
    public string ambientID = null;
    void Start() {
        if (FindObjectsByType<SaveState>().Length > 1) {
            Debug.LogError("Cannot load save. There can only be one save state class!");
            return;
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