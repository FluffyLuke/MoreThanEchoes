using System;
using UnityEngine;

public class PlayerEffects : MonoBehaviour {
    [SerializeField] private GameObject stunSprite;
    void Start() {
        stunSprite.SetActive(false);

        PlayerEventBus.stun.AddListener(Stun);
        PlayerEventBus.stunAndMove.AddListener(StunAndMove);
    }

    void OnDestroy() {
        PlayerEventBus.stun.RemoveListener(Stun);
        PlayerEventBus.stunAndMove.RemoveListener(StunAndMove);
    }
    public void Stun(float timeSec) {
        PlayerEventBus.stateCinematic.Invoke();
        stunSprite.SetActive(true);
        StaticUtils.DoSomethingAfter(timeSec, this, () => {
            stunSprite.SetActive(false);
            PlayerEventBus.stateNormal.Invoke();
        });
    }

    public void StunAndMove(float timeSec, MoveDirection direction, float speed) {
        PlayerEventBus.stateCinematic.Invoke();
        stunSprite.SetActive(true);

        PlayerMoveCinematic m_c = PlayerEventBus.GetPlayerComponent<PlayerMoveCinematic>();

        m_c.SetMove(direction, speed);
        
        StaticUtils.DoSomethingAfter(timeSec, this, () => {
            stunSprite.SetActive(false);
            PlayerEventBus.stateNormal.Invoke();
        });
    }
}