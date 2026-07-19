using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CharacterController2D))]
public class Monster : MonoBehaviour {
    public float speed = 18;
    public string clickingSoundID = "monster_clicking";
    private SoundHandle handle;
    private CharacterController2D controller;
    void Start() {
        controller = GetComponent<CharacterController2D>();
        SoundManager.instance.PlayAndLoop(clickingSoundID, gameObject, out handle, 1);
    }

    void Update() {
        controller.SetMotion(new Vector2(-speed, 0) * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag(Tags.PlayerTag)) return;
        StaticUtils.ChangeLevel("GameOver", "");
    }
}