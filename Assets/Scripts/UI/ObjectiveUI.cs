using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ObjectiveUI : MonoBehaviour {
    private Dictionary<string, ObjectiveData> objectives = new();
    [SerializeField] private GameObject objectivePartPrefab;
    [SerializeField] private GameObject objectiveParent;
    private CanvasGroup canvasGroup;
    public static ObjectiveUI instance = null;
    void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        // Hide objective panel
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void AddObjective(string id, ObjectiveData objective, bool showNew = true) {
        objectives.Add(id, objective);
        Rebuild();
        if (showNew) Show();
    }

    public void RemoveObjective(string id, bool showNew = true) {
        objectives.Remove(id);
        Rebuild();
        if (showNew) Show();
    }

    public void MarkCompletion(string id, bool completed, bool showNew = true) {
        if (!objectives.ContainsKey(id)) {
            Debug.LogWarning($"Cannot find objective of id '{id}'");
            return;
        }

        objectives[id].isCompleted = completed;
        Rebuild();
        if (showNew) Show();
    }

    public void ModifyObjective(string id, string newContents, bool showNew = true) {
        if (!objectives.ContainsKey(id)) {
            Debug.LogWarning($"Cannot find objective of id '{id}'");
            return;
        }

        objectives[id].text = newContents;
        Rebuild();
        if (showNew) Show();
    }

    public void Reset() {
        foreach(Transform child in objectiveParent.transform) {
            Destroy(child.gameObject);
        }
        Show();
    }

    // Rebuild the UI
    public void Rebuild() {
        foreach(Transform child in objectiveParent.transform) {
            Destroy(child.gameObject);
        }

        foreach(var o in objectives) {
            ObjectivePart objectivePartInstance = Instantiate(objectivePartPrefab, objectiveParent.transform).GetComponent<ObjectivePart>();
            objectivePartInstance.SetData(o.Value);
        }
    }

    private Coroutine fadeCoroutine = null;
    [Header("Transition")]
    public float fadeInSecs = 0.5f;
    public float fadeOutCooldownSecs = 3f;
    public float fadeOutSecs = 2f;
    public void Show() {
        if (fadeCoroutine != null) {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(show(fadeInSecs, fadeOutCooldownSecs, fadeOutSecs));
    }
    private IEnumerator show(float fadeIn, float wait, float fadeOut) {
        while (canvasGroup.alpha < 1) {
            float updateBy = 1 / fadeIn * Time.deltaTime;

            canvasGroup.alpha += updateBy;

            if (canvasGroup.alpha >= 1) {
                canvasGroup.alpha = 1;
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(wait);
        
        while (canvasGroup.alpha > 0) {
            float updateBy = 1 / fadeOut * Time.deltaTime; 

            canvasGroup.alpha -= updateBy;

            if (canvasGroup.alpha <= 0) {
                canvasGroup.alpha = 0;
                break;
            }

            yield return null;
        }

        fadeCoroutine = null;
    }
}