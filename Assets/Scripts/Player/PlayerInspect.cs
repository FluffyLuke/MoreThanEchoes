using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInspect : MonoBehaviour {
    private GameInput input;
    private Minigame mg;
    public float transitionTime = 0.3f;
    public Ease transitionEase = Ease.Linear;
    private bool exitFlag = false;
    void Awake() {
        input = new GameInput();

        mg = GameObject
            .FindGameObjectWithTag(Tags.MinigameTag)
            .GetComponent<Minigame>();

        input.PlayerInspect.LookDown.performed += lookDownInput;
        input.PlayerInspect.LookUp.performed += lookUpInput;
        input.PlayerInspect.LookLeft.performed += lookLeftInput;
        input.PlayerInspect.LookRight.performed += lookRightInput;
        input.PlayerInspect.ExitDebug.performed += exitDebug;
    }
    public void EnterState(int pillarNumber) {
        exitFlag = false;
        enabled = true;

        mg.StartMinigame(pillarNumber);
        mg.onGameOver.AddListener(onLoose);

        input.PlayerInspect.Enable();
    }
    void Update() {
        if (mg.progres >= 1 && exitFlag == false) {
            exitFlag = true;
            UIEventBus.transitionIn.Invoke(transitionTime, transitionEase, () => {
                PlayerEventBus.stateNormal.Invoke();
                UIEventBus.transitionOut.Invoke(transitionTime, transitionEase, null);
            });
            return;
        }
    }
    public void ExitState() {
        input?.PlayerInspect.Disable();

        mg.DisableMinigame();
        PlayerEventBus.finishInspecting.Invoke();
        mg.onGameOver.RemoveListener(onLoose);
    }
    private void onLoose() {
        // Simply take away controls
        input.PlayerInspect.Disable();
    }
    public void LookUp() {
        mg.activePillar.ChangeCurrentCamera(mg.activePillar.cameraUp);
    }
    public void LookDown() {
        mg.activePillar.ChangeCurrentCamera(mg.activePillar.cameraMain);
    }
    public void LookLeft() {
        mg.activePillar.ChangeCurrentCamera(mg.activePillar.cameraLeft);
    }
    public void LookRight() {
        mg.activePillar.ChangeCurrentCamera(mg.activePillar.cameraRight);
    }

    #region Input

    private void lookUpInput(InputAction.CallbackContext ctx) {
        LookUp();
    }
    private void lookDownInput(InputAction.CallbackContext ctx) {
        LookDown();
    }
    private void lookLeftInput(InputAction.CallbackContext ctx) {
        LookLeft();
    }
    private void lookRightInput(InputAction.CallbackContext ctx) {
        LookRight();
    }

    private void exitDebug(InputAction.CallbackContext ctx) {
        Debug.Log($"Exiting inspect mode in debug style.");
        PlayerEventBus.stateNormal.Invoke();
    }

    #endregion
}