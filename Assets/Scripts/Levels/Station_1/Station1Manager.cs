using UnityEngine;
using UnityEngine.Events;

public class Station1Manager : MonoBehaviour {
    private GameObject[] entrances;
    void Start() {
        entrances = GameObject.FindGameObjectsWithTag(Tags.EntranceTag);
        if (entrances.Length == 0) {
            Debug.LogError("No entrances were given. Player cannot be spawned...");
            return;
        }


        string currentEntranceName = StaticUtils.GetEntranceName();

        Entrance currentEntrance = null;

        foreach (GameObject e in entrances) {
            if (e.TryGetComponent(out Entrance comp)) {
                if (comp.EntranceName == currentEntranceName) {
                    currentEntrance = comp;
                    break;
                }
            } else {
                Debug.LogWarning($"Object marked as entrance has no Entrance component attached: '{e.name}'");
            }
        }

        if (currentEntrance == null) {
            Debug.LogWarning($"Could not find entrance '{currentEntranceName}'. Using default...");
            currentEntrance = entrances[0].GetComponent<Entrance>();
        }

        currentEntrance.SpawnPlayer();
    }
}