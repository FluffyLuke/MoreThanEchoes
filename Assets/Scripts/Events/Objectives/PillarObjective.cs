using UnityEngine;
using UnityEngine.Events;

public class PillarObjective : MonoBehaviour {
    private float pillarCount;
    public string objectiveID;
    public string objectiveContent;
    public UnityEvent objectiveCompleted = new();
    void Start() {
        var pillars = GameObject.FindGameObjectsWithTag(Tags.PillarTag);
        pillarCount = pillars.Length;
        ObjectiveUI.instance.AddObjective(objectiveID, new($"{objectiveContent}{pillarCount}", false), false);

        foreach (GameObject p in pillars) {
            p.GetComponent<UseAction>().interacted.AddListener(pillarChecked);
        }
    }

    public void DisableAllPillars() {
        var pillars = GameObject.FindGameObjectsWithTag(Tags.PillarTag);
        foreach (var p in pillars) {
            p.GetComponent<UseAction>().enabled = false;
            p.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void pillarChecked() {
        pillarCount--;
        ObjectiveUI.instance.ModifyObjective(objectiveID, $"{objectiveContent}{pillarCount}");
        if (pillarCount == 0) {
            ObjectiveUI.instance.MarkCompletion(objectiveID, true);
            objectiveCompleted.Invoke();
        }
    }
}