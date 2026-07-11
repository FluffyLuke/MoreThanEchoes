using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RotateTowards : MonoBehaviour {
    public string tagToFind;
    public GameObject point;
    public bool lockXAxis, lockYAxis, lockZAxis;
    void Update() {
        if (point == null) {
            point = GameObject.FindWithTag(tagToFind);
            if (point == null) {
                return;
            }
        }

        Vector3 currentPos = transform.position;
        Vector3 pos = point.transform.position;

        pos.x = lockXAxis ? currentPos.x : pos.x;
        pos.y = lockYAxis ? currentPos.y : pos.y;
        pos.z = lockZAxis ? currentPos.z : pos.z;

        transform.LookAt(pos);
    }
}