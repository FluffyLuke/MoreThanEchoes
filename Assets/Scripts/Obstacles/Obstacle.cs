using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour {
    public GameObject keySprite;
    public Collider2D obstacleCollider;
    [Header("Parameters")]
    public float speed = 5;
    public float cooldownSecs = 1;
    public float stunDurationSecs = 1;
    public float stunSpeed = 5;
    public Ease ease;
    [Header("Move points")]
    public bool modifyY = false;
    public ObstaclePath entryLeft, entryRight;
    void Start() {
        keySprite.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other) {
        keySprite.SetActive(true);
    }
    void OnTriggerExit2D(Collider2D other) {
        keySprite.SetActive(false);
    }
    private bool onCooldown = false;
    public void MoveLeftToRight() {
        if (onCooldown) return;

        PlayerEventBus.stateObstacle.Invoke();
        Debug.Log($"Player is moving through obstacle {name} from left to right.");

        keySprite.SetActive(false);
        obstacleCollider.enabled = false;
        onCooldown = true;

        var m_o = PlayerEventBus.GetPlayer().GetComponent<PlayerMoveObstacle>();

        Vector3 entryPosition = entryLeft.entry.position;
        Vector3 exitPosition = entryLeft.exit.position;

        if (!modifyY) {
            entryPosition.y = m_o.gameObject.transform.position.y;
            exitPosition.y = m_o.gameObject.transform.position.y;
        }

        m_o.MoveThrough(entryPosition, exitPosition, ease, speed, () => {
            obstacleCollider.enabled = true;
            PlayerEventBus.stateNormal.Invoke();
            StaticUtils.DoSomethingAfter(cooldownSecs, this, () => {
                onCooldown = false;
            });
        });
        
    }
    public void MoveRightToLeft() {
        if (onCooldown) return;

        PlayerEventBus.stateObstacle.Invoke();

        Debug.Log($"Player is moving through obstacle {name} from right to left.");

        keySprite.SetActive(false);
        obstacleCollider.enabled = false;
        onCooldown = true;

        var m_o = PlayerEventBus.GetPlayer().GetComponent<PlayerMoveObstacle>();

        Vector3 entryPosition = entryRight.entry.position;
        Vector3 exitPosition = entryRight.exit.position;

        if (!modifyY) {
            entryPosition.y = m_o.gameObject.transform.position.y;
            exitPosition.y = m_o.gameObject.transform.position.y;
        }

        m_o.MoveThrough(entryPosition, exitPosition, ease, speed, () => {
            obstacleCollider.enabled = true;
            PlayerEventBus.stateNormal.Invoke();
            StaticUtils.DoSomethingAfter(cooldownSecs, this, () => {
                onCooldown = false;
            });
        });
    }
    public void StunLeft() {
        PlayerEventBus.stunAndMove.Invoke(stunDurationSecs, MoveDirection.Left, stunSpeed);
    }
    public void StunRight() {
        PlayerEventBus.stunAndMove.Invoke(stunDurationSecs, MoveDirection.Right, stunSpeed);
    }
}

[Serializable]
public struct ObstaclePath {
    public Transform entry;
    public Transform exit;
}
