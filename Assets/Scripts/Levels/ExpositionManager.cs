using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum IntroPanelState {
    Idle,
    Started,
    Ended,
}

[Serializable]
public class IntroPanel {
    public GameObject panel;
    public TMPWrapper exposition;
    [HideInInspector] public IntroPanelState state;
}
public class ExpositionManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Blackness black;
    [SerializeField] private IntroPanel[] panels;
    [Header("Values")]
    public float cps = 15;
    public float fade = 2;
    private int currentPage = 0; 
    [Header("Event")]
    public UnityEvent ended;
    private GameInput input;
    void Awake() {
        input = new GameInput();
        input.Intro.Next.performed += nextPageInput;
    }

    void Start() {
        input.Intro.Enable();
        foreach (var p in panels) {
            p.panel.SetActive(false);
        }
        nextPage();
    }

    private void nextPageInput(InputAction.CallbackContext ctx) {
        nextPage();
    }

    private void nextPage() {
        if (activatePanel(panels[currentPage])) {
            currentPage++;
        } else {
            return;
        }
        
        if (currentPage >= panels.Length) {
            Debug.Log("Exiting...");
            input.Disable();
            ended.Invoke();
            return;
        }

        activatePanel(panels[currentPage]);
    }

    private bool activatePanel(IntroPanel panel) {
        if (panel.state == IntroPanelState.Idle) {
            Debug.Log($"Switching to {panel.panel.name} panel.");
            panel.exposition.HideText();
            panel.panel.SetActive(true);

            panel.state = IntroPanelState.Started;

            panel.exposition.ShowText(cps, onComplete: () => {
                panel.state = IntroPanelState.Ended;
            });
            black.TransitionOut(fade);
            return false;
        } else if (panel.state == IntroPanelState.Started) {
            Debug.Log($"Skipping {panel.panel.name} panel.");
            panel.exposition.ShowText();
            black.StopTransition();

            Debug.Log($"Finished panel {panel.panel.name} early.");
            panel.state = IntroPanelState.Ended;
            return false;
        } else {
            Debug.Log($"Exiting {panel.panel.name}.");
            return true;
        }
    }
}