using System;
using UnityEngine;

public class PlayerEffects : MonoBehaviour {
    [SerializeField] private Animator animator;
    public string stunAnimation = "stun";
    void Start() {
        PlayerEventBus.stun.AddListener(Stun);
        //PlayerEventBus.stunAndMove.AddListener(StunAndMove);
    }

    void OnDestroy() {
        PlayerEventBus.stun.RemoveListener(Stun);
        //PlayerEventBus.stunAndMove.RemoveListener(StunAndMove);
    }
    public void Stun(float timeSec, MoveDirection direction) {
        PlayerEventBus.stateCinematic.Invoke();

        PlayerMoveCinematic m_c = PlayerEventBus.GetPlayerComponent<PlayerMoveCinematic>();
        m_c.SetMove(direction == MoveDirection.Left ? MoveDirection.Right : MoveDirection.Left, 0.0001f);
        
        animator.Play(stunAnimation);
    }

    // This should be called by the animation itself
    public void StunEnd() {
        PlayerEventBus.stateNormal.Invoke();
    }
}