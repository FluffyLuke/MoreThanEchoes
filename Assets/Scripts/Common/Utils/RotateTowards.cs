using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RotateTowards : MonoBehaviour {
    public GameObject point;
    void Update() {
        transform.LookAt(point.transform);
    }
}