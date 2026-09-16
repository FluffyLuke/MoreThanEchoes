using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ShadowFigure : MonoBehaviour {
    [Header("Parameters")]
    public float fadeDuration = 1f;
    public float stepInterval = 0.3f;
    public float distance = 5f;
    public string wetStepSoundID = "wet_step";
    public Ease ease = Ease.Linear;
    [Header("References")]
    private AudioSource source;
    public GameObject figure;
    public Animator animator;
    public SpriteRenderer figureSprite;
    public Transform initialPosition;
    public Transform endPosition;
    public Transform endPositionSprint;
    [Header("Events")]
    public UnityEvent onJumpscare = new();
    private bool fading = false;
    private GameInput input;

    void Awake() {
        input = new GameInput();
        input.Player.Enable();
        source = GetComponent<AudioSource>();
    }
    void OnDisable() {
        input.Player.Disable();
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
            Transform end = isSprinting ? endPositionSprint : endPosition;

            onJumpscare.Invoke();
            animator.Play("run");
            StartCoroutine(wetSteps(stepInterval));
            
            transform
                .DOMove(end.position, fadeDuration)
                .SetEase(ease)
                .OnComplete(() => {
                    Destroy(gameObject);
                });
            calculateFade();
        }
    }

    private IEnumerator wetSteps(float interval) {
        while(true) {
            SoundManager.instance.PlayOneShot(wetStepSoundID, gameObject, out SoundHandle _, spartialBlend: 0.5f, source: source);
            yield return new WaitForSeconds(interval);
        }
    }

    private void calculateFade() {
        Color figureColor = figureSprite.color;
        figureColor.a -= fadeDuration * Time.deltaTime;
        figureSprite.color = figureColor;
    }
}