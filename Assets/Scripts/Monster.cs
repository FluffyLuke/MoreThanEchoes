using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CharacterController2D))]
public class Monster : MonoBehaviour {
    public float speed = 18;
    public float catchUpSpeed = 25;
    public float catchUpDistanceFull = 5;
    public float catchUpDistanceMin = 2;
    public string clickingSoundID = "monster_clicking";
    private SoundHandle handle;
    private CharacterController2D controller;
    void Start() {
        controller = GetComponent<CharacterController2D>();
        SoundManager.instance.PlayAndLoop(clickingSoundID, gameObject, out handle, 1);
    }

    void Update() {
        GameObject player = PlayerEventBus.GetPlayer();
        if (player == null) return;

        float t = Mathf.InverseLerp(catchUpDistanceMin, catchUpDistanceFull, player.transform.position.x);
        float s = Mathf.Lerp(speed, catchUpSpeed, t);
        controller.SetMotion(new Vector2(-s, 0) * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag(Tags.PlayerTag)) return;
        StaticUtils.ChangeLevel("GameOver", "");
    }
}