using UnityEngine;

public class Fireplace : MonoBehaviour {
    public NoteAsset note;
    public float cooldown = 1f;
    private bool onCooldown = false;

    void Start() {
        PlayerEventBus.hideNote.AddListener(hideNote);
    }

    public void Use() {
        if (onCooldown) return;

        onCooldown = true;
        PlayerEventBus.showNote.Invoke(note);
    }

    private void hideNote() {
        StaticUtils.DoSomethingAfter(cooldown, this, () => {
            onCooldown = false;
        });
    }
}