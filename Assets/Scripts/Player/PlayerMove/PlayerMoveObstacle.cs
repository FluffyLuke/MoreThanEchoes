using System;
using UnityEngine;
using DG.Tweening;

public class PlayerMoveObstacle : MonoBehaviour {
    [SerializeField] private GameObject body;
    [SerializeField] private string obstacleMoveAnimation = "pass";
    [SerializeField] private string defaultAnimationTrigger = "ExitObstacle";
    [SerializeField] private Animator animator;
    private Tween currentTween;

    void OnDisable() {
        currentTween?.Kill();
    }
    public void MoveThrough(Vector2 start, Vector2 finish, Ease ease, float speedSec, Action onComplete) {
        Vector2 direction = finish - start;
        float distance = Vector2.Distance(start, finish);
        PlayerLook look = GetComponent<PlayerLook>();
        
        look.SetWhereToLook(direction.x > 0 ? WhereToLook.Right : WhereToLook.Left);

        // Must set position on rigidbody to update physics engine to stop jitter
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.position = start;
        // transform.position = start;
        animator.Play(obstacleMoveAnimation);
        currentTween = rb
            .DOMove(finish, distance / speedSec)
            .SetEase(ease)
            .SetUpdate(UpdateType.Late)
            .OnComplete(() => {
                animator.SetTrigger(defaultAnimationTrigger);
                onComplete.Invoke();
            });
    }
}
