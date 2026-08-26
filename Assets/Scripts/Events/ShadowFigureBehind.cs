using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class ShadowFigureBehind : MonoBehaviour {
    [Header("Parameters")]
    public float fadeDuration = 1f;
    public float startingDistance = 10f;
    public float killDistance = 3f;
    public float timeToKill = 6f;
    [Header("Eyes")]
    public Light2D[] eyes; 
    public float startGlowingTime = 1f;
    public float maxGlowingTime = 5f;
    [Range(0, 1)]
    public float maxGlowValue = 0.3f;
    [Header("Jumpscare")]
    public string ambientID = "gathering_darkness";
    public string heartBeatID = "heart_beat_fast";
    public float minimumJumpscareTime = 4f;
    public float waitBeforeAmbientReturn = 3f;
    public float ambientReturnTime = 3f;
    public Ease ease = Ease.Linear;
    [Header("References")]
    public GameObject figure;
    public SpriteRenderer figureSprite;
    public Transform endPosition;
    private bool fading = false;
    private GameInput input;
    [HideInInspector] public UnityEvent destroyed = new();
    public UnityEvent onJumpscare = new();

    void Awake() {
        input = new GameInput();
        input.Player.Enable();
    }
    void OnDisable() {
        input.Player.Disable();
    }

    void OnDestroy() {
        input.Player.Disable();
        destroyed.Invoke();
    }

    void Start() {
        PlayerEventBus.GetPlayerComponent<PlayerLook>().newLookingDirection.AddListener(onChangeDirection);
    }
    private float timeElapsed = 0;
    private void onChangeDirection(bool right) {
        PlayerEventBus.GetPlayerComponent<PlayerLook>().newLookingDirection.RemoveListener(onChangeDirection);

        Vector2 playerPos = PlayerEventBus.GetPlayer().transform.position;
        Vector2 spawnPosition = playerPos - new Vector2(startingDistance, 0);
        Vector2 killPosition = playerPos - new Vector2(killDistance, 0);

        fading = true;
        float jumpscareT = timeElapsed/minimumJumpscareTime;

        // Show monster running away only if it is very close.
        // If not, then make it disappear right away.
        if (jumpscareT < 1) {
            Destroy(gameObject);
            return;
        }

        // This part of the code is for jumpscare
        bool isSprinting = input.Player.Sprint.IsInProgress();

        onJumpscare.Invoke();
        AmbientManager.instance.PlayAmbient(AmbientNames.EmptyAmbient, 0.3f);
        SoundManager.instance.PlayOneShot(heartBeatID, gameObject);

        StaticUtils.DoSomethingAfter(waitBeforeAmbientReturn, this, () => {
            AmbientManager.instance.PlayAmbient(ambientID, ambientReturnTime);
        });

        transform
            .DOMove(endPosition.position, fadeDuration)
            .SetEase(ease)
            .OnComplete(() => {
                Destroy(gameObject, 5);
            });
        calculateFade();
    }

    void Update() {
        if (fading) {
            calculateFade();
            return;
        } else {
            timeElapsed += Time.deltaTime;
            Vector2 playerPos = PlayerEventBus.GetPlayer().transform.position;
            Vector2 spawnPosition = playerPos - new Vector2(startingDistance, 0);
            Vector2 killPosition = playerPos - new Vector2(killDistance, 0);

            float t = timeElapsed/timeToKill;
            Vector2 newPos = Vector2.Lerp(spawnPosition, killPosition, t);
            transform.position = newPos;

            // Eye glow
            float eyeT = Mathf.InverseLerp(startGlowingTime, maxGlowingTime, timeElapsed);
            Debug.Log($"== DEBUG eyeT {eyeT}== ");
            float eyeAlpha = maxGlowValue * eyeT;
            Debug.Log($"DEBUG eyeAlpha {eyeAlpha}");

            foreach (Light2D l in eyes) {
                Color newColor = l.color;
                newColor.a = eyeAlpha;
                l.color = newColor;
            }

            if (t >= 1) {
                PlayerEventBus.GetPlayerComponent<PlayerBrain>().Die();
            }
        }
    }

    private void calculateFade() {
        Color figureColor = figureSprite.color;
        figureColor.a -= fadeDuration * Time.deltaTime;
        figureSprite.color = figureColor;

        foreach (Light2D l in eyes) {
            Color newColor = l.color;
            newColor.a = fadeDuration * Time.deltaTime;;
            l.color = newColor;
        }
    }
}