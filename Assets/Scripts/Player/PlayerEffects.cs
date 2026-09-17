using System;
using UnityEngine;

public class PlayerEffects : MonoBehaviour {
    [SerializeField] private Animator animator;
    public float stunDuration = 0.75f;
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

        DoSomethingAfter.After(this, stunDuration, () => {
            PlayerEventBus.stateNormal.Invoke();
        });
    }
}