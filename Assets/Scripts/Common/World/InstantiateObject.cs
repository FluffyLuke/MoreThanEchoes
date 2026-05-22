using UnityEngine;

public class InstantiateObject : MonoBehaviour {
    public Transform parent = null;
    public GameObject whatToSpawnPrefab;
    public void Spawn() {
        GameObject obj;
        if (parent != null) {
            obj = Instantiate(whatToSpawnPrefab, parent.transform);
        } else {
            obj = Instantiate(whatToSpawnPrefab);
        }
        obj.transform.position = transform.position;
    }
}