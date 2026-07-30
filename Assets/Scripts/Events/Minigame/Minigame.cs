using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Minigame : MonoBehaviour {
    public UnityEvent onGameOver = new();
    [Header("UI")]
    public Canvas inspectCanvas;
    public Slider progresBar;
    [Header("References")]
    public Light torch;
    public MinigamePillar[] pillars;
    [HideInInspector] public MinigamePillar activePillar;
    [Header("Values")]
    public float speed = 10;
    public float timeBeforeEndScreen = 2;
    [HideInInspector] public float progres = 0;
    private bool focused = true;
    void Start() {
        inspectCanvas.gameObject.SetActive(false);
    }
    void Update() {
        torch.transform.position = Camera.main.transform.position;
        torch.transform.rotation = Camera.main.transform.rotation;
        if (focused) {
            progres += Time.deltaTime * speed / 50;
            progresBar.value = progres;
        }
    }

    public void SetFocus(bool state) {
        focused = state;
        inspectCanvas.gameObject.SetActive(state);
    }

    #region PlayerControls

    public void StartMinigame(int pillarNumber) {
        int pillarIndex = pillarNumber - 1;

        if (pillarIndex < 0) {
            Debug.LogError($"Index too small. Rounding to 0 ({pillarNumber} originally).");
            activePillar = pillars[0];
        } else if (pillarIndex >= pillars.Length) {
            Debug.LogError($"Index too big. Rounding to {pillars.Length - 1} ({pillarNumber} originally).");
            activePillar = pillars[pillars.Length - 1];
        } else {
            activePillar = pillars[pillarIndex];
        }

        progres = 0;
        progresBar.value = progres;
        inspectCanvas.gameObject.SetActive(true);
        focused = true;
        Camera.main.orthographic = false;
        Camera.main.GetUniversalAdditionalCameraData().SetRenderer(Renderers._3D);
        activePillar.ChangeCurrentCamera(activePillar.cameraMain);
    
        GetComponent<MinigameEnemyBrain>().StartMiniGame();
    }

    public void DisableMinigame() {
        Debug.Log($"Disabling minigame.");

        activePillar?.DisableCameras();
        Camera.main.orthographic = true;
        Camera.main.GetUniversalAdditionalCameraData().SetRenderer(Renderers._2D);
        inspectCanvas.gameObject.SetActive(false);
    
        GetComponent<MinigameEnemyBrain>().EndMinigame();
    }

    #endregion

    #region Jumpscare

    public void LooseMinigameLeft() {
        onGameOver.Invoke();
        activePillar.ChangeCurrentCamera(activePillar.cameraLeft);
        StartCoroutine(looseMinigame(timeBeforeEndScreen));
    }

    public void LooseMinigameUp() {
        onGameOver.Invoke();
        activePillar.ChangeCurrentCamera(activePillar.cameraUp);
        StartCoroutine(looseMinigame(timeBeforeEndScreen));
    }

    public void LooseMinigameRight() {
        onGameOver.Invoke();
        activePillar.ChangeCurrentCamera(activePillar.cameraRight);
        StartCoroutine(looseMinigame(timeBeforeEndScreen));
    }

    private IEnumerator looseMinigame(float cooldown) {
        yield return new WaitForSeconds(cooldown);
        PlayerEventBus.GetPlayerComponent<PlayerBrain>().Die();
    }

    #endregion
}