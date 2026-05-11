using UnityEngine;

public class ObjectivePart : MonoBehaviour {
    [SerializeField] private TMPWrapper text;
    public void SetData(ObjectiveData data) {
        if (data.isCompleted) text.SetText($"<s>{data.text}</s>");
        else text.SetText(data.text);
    }
}

public class ObjectiveData {
    public string text;
    public bool isCompleted;

    public ObjectiveData(string text, bool completed) {
        this.text = text;
        this.isCompleted = completed;
    }
}