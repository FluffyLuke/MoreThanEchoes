using System;
using UnityEngine;
using DG.Tweening;

public class PlayerMoveObstacle : MonoBehaviour {
    [SerializeField] private GameObject body;
    public void MoveThrough(Vector2 start, Vector2 finish, Ease ease, float speedSec, Action onComplete) {
        Vector2 direction = finish - start;
        float distance = Vector2.Distance(start, finish);
        
        if (direction.x > 0) {
            Vector3 newBodyScale = body.transform.localScale;
            if (direction.x > 0) newBodyScale.x = Mathf.Abs(newBodyScale.x);
            else if (direction.x < 0) newBodyScale.x = -Math.Abs(newBodyScale.x);
        }

        // Must set position on rigidbody to update physics engine to stop jitter
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.position = start;
        // transform.position = start;

        rb
            .DOMove(finish, distance / speedSec)
            .SetEase(ease)
            .SetUpdate(UpdateType.Late)
            .OnComplete(() => {
                onComplete.Invoke();
            });
    }
}
