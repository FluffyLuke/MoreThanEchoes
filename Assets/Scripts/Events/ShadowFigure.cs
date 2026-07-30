using UnityEngine;
using DG.Tweening;

public class ShadowFigure : MonoBehaviour {
    [Header("Parameters")]
    public float fadeDuration = 1f;
    public float distance = 5f;
    public Ease ease = Ease.Linear;
    [Header("References")]
    public GameObject figure;
    public SpriteRenderer figureSprite;
    public Transform initialPosition;
    public Transform endPosition;
    public Transform endPositionSprint;
    private bool fading = false;
    private GameInput input;

    void Awake() {
        input = new GameInput();
        input.Player.Enable();
    }

    void Update() {
        if (fading) {
            calculateFade();
            return;
        }

        Vector2 playerPosition2D = PlayerEventBus.GetPlayer().transform.position;
        Vector2 initialPosition2D = initialPosition.position;

        if (Vector3.Distance(playerPosition2D, initialPosition2D) <= distance) {
            fading = true;
            bool isSprinting = input.Player.Sprint.IsInProgress();
            Debug.Log($"IS SPRINTING: {isSprinting}");
            Transform end = isSprinting ? endPositionSprint : endPosition;

            transform
                .DOMove(end.position, fadeDuration)
                .SetEase(ease)
                .OnComplete(() => {
                    Destroy(gameObject);
                });
            calculateFade();
        }
    }

    private void calculateFade() {
        Color figureColor = figureSprite.color;
        figureColor.a -= fadeDuration * Time.deltaTime;
        figureSprite.color = figureColor;
    }
}