using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPWrapper : MonoBehaviour 
{
    private TextMeshProUGUI textGUI;
    private Coroutine showTextCoroutine;
    private float defaultSpeed = 0; 
    public UnityEvent CharacterInserted = new();

    void Awake() {
        textGUI = GetComponent<TextMeshProUGUI>();
    }

    public void HideText() {
        textGUI.maxVisibleCharacters = 0;
    }

    public void SetText(string text, bool hide = false) {
        textGUI.text = text;
        textGUI.maxVisibleCharacters = hide ? 0 : int.MaxValue;
    }

    // public void SetText(CharacterDialogue dialogue, bool hide = false) {
    //     defaultSpeed = dialogue.speed;
    //     SetText(dialogue.text, hide);
    // }

    // public void SetText(UIText uiText, bool hide = false) {
    //     defaultSpeed = uiText.speed;
    //     SetText(uiText.text, hide);
    // }
    public void ShowText() {
        if(showTextCoroutine != null) {
            Debug.Log("Skipped text showing...");
            StopCoroutine(showTextCoroutine);
        }

        showTextCoroutine = StartCoroutine(showText(defaultSpeed, -1, null));
    }

    public void ShowText(string text, float speed, float clearAfter = -1, Action onComplete = null) {
        SetText(text);
        if(showTextCoroutine != null) {
            Debug.Log("Skipped text showing...");
            StopCoroutine(showTextCoroutine);
        }

        if (speed <= 0) {
            showTextCoroutine = StartCoroutine(showText(defaultSpeed, clearAfter, onComplete));
            return;
        }

        showTextCoroutine = StartCoroutine(showText(speed, clearAfter, onComplete));
    }

    public void ShowText(float speed, float clearAfter = -1, Action onComplete = null) {
        if(showTextCoroutine != null) {
            Debug.Log("Skipped text showing...");
            StopCoroutine(showTextCoroutine);
        }

        if (speed <= 0) {
            showTextCoroutine = StartCoroutine(showText(defaultSpeed, clearAfter, onComplete));
            return;
        }

        showTextCoroutine = StartCoroutine(showText(speed, clearAfter, onComplete));
    }

    public void ShowText(string text, float speed = 10, float clearAfter = -1) {
        defaultSpeed = speed;
        SetText(text);
        ShowText(speed, clearAfter);
    }

    // public void ShowText(CharacterDialogue dialogue, float clearAfter = -1) {
    //     defaultSpeed = dialogue.speed;
    //     SetText(dialogue.text);
    //     ShowText(dialogue.speed, clearAfter);
    // }

    // public void ShowText(UIText uiText, float clearAfter = -1) {
    //     defaultSpeed = uiText.speed;
    //     SetText(uiText.text);
    //     ShowText(uiText.speed, clearAfter);
    // }

    // TODO: Clean this function, since it is a mess
    private IEnumerator showText(float speed, float clearAfter, Action onComplete) {
        float timer = 0f;

        // If speed is set to 0, just wait to clear text after
        if (speed <= 0) {
            textGUI.maxVisibleCharacters = int.MaxValue;
            if (clearAfter <= 0) {
                yield break;
            }

            // We don't want this to be affected by game pause
            while (timer < clearAfter) {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            textGUI.text = "";
            onComplete?.Invoke();
            yield break;
        }

        // Speed is not set to 0

        float cps = 1f / speed;

        textGUI.maxVisibleCharacters = 0;

        // FIX: This is binded to the frame rate
        // Lower framerate may make text appear slower than it should
        float clearTimer = 0;
        foreach(char l in textGUI.text) {
            textGUI.maxVisibleCharacters += 1;
            CharacterInserted.Invoke();
            
            if (l == ' ') {
                continue;
            }
            while (clearTimer < cps) {
                clearTimer += Time.unscaledDeltaTime;
                yield return null;
            }

            clearTimer = 0;
        }

        // Do not clear after
        if (clearAfter <= 0) {
            onComplete?.Invoke();
            yield break;
        }

        // Wait and clear after
        while (timer < clearAfter) {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        textGUI.text = "";
        onComplete?.Invoke();
    }
}