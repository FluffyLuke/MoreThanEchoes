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
        PlayerEventBus.changeState.Invoke(PlayerMode.Cinematic);
        stunSprite.SetActive(true);
        StaticUtils.DoSomethingAfter(timeSec, this, () => {
            stunSprite.SetActive(false);
            PlayerEventBus.changeState.Invoke(PlayerMode.Normal);
        });
    }

    public void StunAndMove(float timeSec, MoveDirection direction, float speed) {
        PlayerEventBus.changeState.Invoke(PlayerMode.Cinematic);
        stunSprite.SetActive(true);

        PlayerMoveCinematic m_c = PlayerEventBus.GetPlayerComponent<PlayerMoveCinematic>();

        m_c.SetMove(direction, speed);
        
        StaticUtils.DoSomethingAfter(timeSec, this, () => {
            stunSprite.SetActive(false);
            PlayerEventBus.changeState.Invoke(PlayerMode.Normal);
        });
    }
}