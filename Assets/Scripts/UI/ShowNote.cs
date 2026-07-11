using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerShowNote : MonoBehaviour {
    [SerializeField] private GameObject[] pages;
    [Header("Sound")]
    [SerializeField] private PlaySound pickUpSound;
    [SerializeField] private PlaySound turnPageSound;
    [Header("Fade")]
    [SerializeField] private CanvasGroup group;
    [SerializeField] private float fadeTimeSec = 1f;
    private int pageIndex = 0;
    private GameInput input;
    void Awake() {
        input = new GameInput();
        PlayerEventBus.showNote.AddListener(show);
        input.Player.MovePages.performed += movePagesCallback;
        input.Player.HidePages.performed += hide;
    }
    void OnDisable() {
        input.Player.Disable();
    }

    private void show() {
        PlayerEventBus.stateCinematic.Invoke();
        input.Player.Enable();

        pickUpSound.Play();
        group.DOFade(1, fadeTimeSec);

        movePages(new(0,0));
    }

    private void hide(InputAction.CallbackContext ctx) {
        PlayerEventBus.stateNormal.Invoke();

        pickUpSound.Play();
        group.DOFade(0, fadeTimeSec);

        input.Player.Disable();

        PlayerEventBus.hideNote.Invoke();
    }

    private void movePagesCallback(InputAction.CallbackContext ctx) {
        Vector2 direction = ctx.ReadValue<Vector2>();
        movePages(direction);
    }

    private void movePages(Vector2 direction) {
        if (direction.x > 0) {
            pageIndex++;
        } else if (direction.x < 0) {
            pageIndex--;
        }

        if (pageIndex < 0) {
            pageIndex = 0;
            return;
        }

        if (pageIndex > pages.Length - 1) {
            pageIndex = pages.Length - 1;
            return;
        }

        // Play sound only if page was changed
        if (direction.x != 0) {
            turnPageSound.Play();
        }

        foreach (var p in pages) {
            p.SetActive(false);
        }

        GameObject currentPage = pages[pageIndex];
        currentPage.SetActive(true);
    }
}