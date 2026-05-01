using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChanceAmbient : MonoBehaviour {
    public Collider collider1, collider2;
    public string Ambient1_ID, Ambient2_ID;
    public float FadeDuration = 1f;
    void Start() {
        if (collider1.enabled == false && collider2.enabled == false) {
            Debug.LogWarning("Both colliders are turned off on Ambient changer.");
        }

        if (collider1.enabled == true && collider2.enabled == true) {
            collider2.enabled = false;
            Debug.LogWarning("Both colliders are turned on on Ambient changer. Turning one off.");
        }
    }
    public void PlayAmbient1() {
        Debug.Log("A");
        collider1.enabled = false;
        collider2.enabled = true;
        playAmbient(Ambient1_ID);
    }
    public void PlayAmbient2() {
        Debug.Log("B");
        collider1.enabled = true;
        collider2.enabled = false;
        playAmbient(Ambient2_ID);
    }

    private void playAmbient(string id) {
        AmbientManager.instance.PlayAmbient(id, FadeDuration);
    }
}