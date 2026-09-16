using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CharacterController2D))]
public class Monster : MonoBehaviour {
    public float speed = 18;
    public float catchUpSpeed = 25;
    public float catchUpDistanceFull = 5;
    public float catchUpDistanceMin = 2;
    public string clickingSoundID = "monster_clicking";
    public string wetStepSoundID = "wet_step";
    public float stepInterval;
    public Animator animator;
    private SoundHandle handle;
    private CharacterController2D controller;
    void Start() {
        controller = GetComponent<CharacterController2D>();
        SoundManager.instance.PlayAndLoop(clickingSoundID, gameObject, out handle, 1);
        animator.Play("run");
        StartCoroutine(setSpeedT());
    }

    private float bonusSpeed = 0;
    private float speedT = 0;

    void Update() {
        GameObject player = PlayerEventBus.GetPlayer();
        if (player == null) return;
        
        float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
        float t = Mathf.InverseLerp(catchUpDistanceMin, catchUpDistanceFull, distance);
        float s = Mathf.Lerp(speed, catchUpSpeed, t);

        bonusSpeed += speedT * Time.deltaTime;

        // Keep in in <-2;2>
        bonusSpeed = Mathf.Min(bonusSpeed, 2);
        bonusSpeed = Mathf.Max(bonusSpeed, -2);

        controller.SetMotion(new Vector2(-(s+bonusSpeed), 0) * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag(Tags.PlayerTag)) return;
        Destroy(gameObject);
        StopAllCoroutines();
        PlayerEventBus.GetPlayerComponent<PlayerBrain>().Die(false);
    }

    private IEnumerator setSpeedT() {
        while (true) {
            speedT = UnityEngine.Random.Range(-0.5f, 0.5f);
            yield return new WaitForSeconds(2);
        }
    }
}