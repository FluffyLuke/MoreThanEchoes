using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
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
    public IntroPanelState state;
}
public class IntroManager : MonoBehaviour {
    public float cps = 15;
    public float fade = 2;
    public float fadeExit = 3;
    private int currentPage = 0; 
    private GameInput input;
    [SerializeField] private Blackness black;
    [SerializeField] private IntroPanel[] panels;
    void Awake() {
        input = new GameInput();
        input.Intro.Next.performed += nextPageInput;
    }

    void Start() {
        input.Intro.Enable();
        nextPage();
    }

    private void nextPageInput(InputAction.CallbackContext ctx) {
        nextPage();
    }

    private void nextPage() {
        switch (currentPage) {
            case 0:
                foreach (var p in panels) {
                    p.panel.SetActive(false);
                }

                currentPage++;
                goto case 1;
            case 1:
                if (activatePanel(panels[0])) {
                    currentPage++;
                    goto case 2;
                }
                break;
            case 2:
                if (activatePanel(panels[1])) {
                    currentPage++;
                    goto case 3;
                }
                break;
            case 3:
                Debug.Log("Exiting...");
                input.Disable();
                black.TransitionIn(fadeExit, onComplete: () => changeLevel());
                AmbientManager.instance.PlayAmbient(LevelNames.EmptyAmbient, fadeExit * 0.8f);
                break;
            default:
                Debug.LogError("This code should not be reachable?");
                break;
        }
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
            panel.state = IntroPanelState.Ended;
            return false;
        } else {
            Debug.Log($"Exiting {panel.panel.name}.");
            return true;
        }
    }

    private void changeLevel() {
        SceneManager.LoadScene(LevelNames.Entrance);
    }
}