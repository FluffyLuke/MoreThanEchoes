using System;
using UnityEngine;

// TODO: Get rid of moving by physics
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour {
    private Rigidbody2D body;
    //RaycastHit2D[] hits = new RaycastHit2D[32];
    void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    // https://github.com/Brackeys/2D-Character-Controller/blob/master/CharacterController2D.cs
    public void SetMotion(Vector2 motion) {
        if (Time.deltaTime == 0) return;

        Vector2 velocity = motion / Time.deltaTime;
        body.linearVelocity = velocity;
    }

    // public void Move(Vector3 motion) {
    //     Array.Clear(hits, 0, 32);
    //     body.Cast(motion, hits);
        
    //     foreach (RaycastHit2D hit in hits) {
    //         hit.
    //     }
    // }
}
