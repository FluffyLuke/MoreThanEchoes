using UnityEngine;

public class Fireplace : MonoBehaviour {
    public float cooldown = 1f;
    private bool onCooldown = false;

    void Start() {
        PlayerEventBus.hideNote.AddListener(hideNote);
    }

    public void Use() {
        if (onCooldown) return;

        onCooldown = true;
        PlayerEventBus.showNote.Invoke();
    }

    private void hideNote() {
        StaticUtils.DoSomethingAfter(cooldown, this, () => {
            onCooldown = false;
        });
    }
}