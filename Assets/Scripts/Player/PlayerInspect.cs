using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInspect : MonoBehaviour {
    private GameInput input;
    private Minigame mg = null;
    public float transitionTime = 0.3f;
    public Ease transitionEase = Ease.Linear;
    private bool exitFlag = false;
    void Awake() {
        input = new GameInput();

        var minigameObject = GameObject.FindGameObjectWithTag(Tags.MinigameTag);

        if (minigameObject == null) {
            Debug.LogWarning("Cannot find minigame prefab in the scene. This may be intentional. Turning off this player feature.");
            return;
        }

        mg = minigameObject.GetComponent<Minigame>();
            

        input.PlayerInspect.LookDown.performed += lookDownInput;
        input.PlayerInspect.LookUp.performed += lookUpInput;
        input.PlayerInspect.LookLeft.performed += lookLeftInput;
        input.PlayerInspect.LookRight.performed += lookRightInput;
        input.PlayerInspect.ExitDebug.performed += exitDebug;
    }
    public void EnterState(int pillarNumber) {
        ObjectiveUI.instance.HideCompletely();
        if (mg == null) return;

        exitFlag = false;
        enabled = true;

        mg.StartMinigame(pillarNumber);
        mg.onGameOver.AddListener(onLoose);

        input.PlayerInspect.Enable();
    }
    void Update() {
        if (mg == null) return;
        if (mg.progres >= 1 && exitFlag == false) {
            exitFlag = true;
            StopInspecting();
        }
    }
    public void StopInspecting() {
        exitFlag = true;
        UIEventBus.transitionIn.Invoke(transitionTime, transitionEase, () => {
            PlayerEventBus.stateNormal.Invoke();
            UIEventBus.transitionOut.Invoke(transitionTime, transitionEase, null);
            ObjectiveUI.instance.ShowCompletely();
        });
    }
    public void ExitState() {
        if (mg == null) return;
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