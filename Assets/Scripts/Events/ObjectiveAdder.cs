using System;
using UnityEngine;

public enum ObjectiveAction {
    Add,
    Complete,
    Modify,
    Delete,
    Show,
    Clear,
}
public class ObjectiveAdder : MonoBehaviour {
    public ObjectiveAction whatToDo = ObjectiveAction.Add;
    public string id = "";
    public string content = "";

    // No idea how to name this function
    public void Run() {
        switch (whatToDo) {
            case ObjectiveAction.Add:
                ObjectiveUI.instance.AddObjective(id, new(content, false));
                break;
            case ObjectiveAction.Delete:
                ObjectiveUI.instance.RemoveObjective(id);
                break;
            case ObjectiveAction.Modify:
                ObjectiveUI.instance.ModifyObjective(id, content);
                break;
            case ObjectiveAction.Complete:
                ObjectiveUI.instance.MarkCompletion(id, true);
                break;
            case ObjectiveAction.Show:
                ObjectiveUI.instance.Show();
                break;
            case ObjectiveAction.Clear:
                ObjectiveUI.instance.Reset();
                break;
        }
    } 
}