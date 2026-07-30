using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class ShadowFigureBehind : MonoBehaviour {
    [Header("Parameters")]
    public float fadeDuration = 1f;
    public float distance = 10f;
    public Ease ease = Ease.Linear;
    [Header("References")]
    public GameObject figure;
    public SpriteRenderer figureSprite;
    public Transform endPosition;
    private bool fading = false;
    private GameInput input;
    [HideInInspector] public UnityEvent destroyed = new();

    void Awake() {
        input = new GameInput();
        input.Player.Enable();
    }

    void OnDestroy() {
        input.Player.Disable();
    }

    void Start() {
        PlayerEventBus.GetPlayerComponent<PlayerLook>().newLookingDirection.AddListener(onChangeDirection);
    }

    private void onChangeDirection(bool right) {
        PlayerEventBus.GetPlayerComponent<PlayerLook>().newLookingDirection.RemoveListener(onChangeDirection);

        Vector2 playerPosition2D = PlayerEventBus.GetPlayer().transform.position;
        Vector2 initialPosition2D = playerPosition2D;
        initialPosition2D.x -= distance;

        fading = true;
        bool isSprinting = input.Player.Sprint.IsInProgress();

        transform
            .DOMove(endPosition.position, fadeDuration)
            .SetEase(ease)
            .OnComplete(() => {
                destroyed.Invoke();
                Destroy(gameObject);
            });
        calculateFade();
    }

    void Update() {
        if (fading) {
            calculateFade();
            return;
        } else {
            Vector2 newPos = PlayerEventBus.GetPlayer().transform.position;
            newPos.x -= distance;
            transform.position = newPos;
        }
    }

    private void calculateFade() {
        Color figureColor = figureSprite.color;
        figureColor.a -= fadeDuration * Time.deltaTime;
        figureSprite.color = figureColor;
    }
}