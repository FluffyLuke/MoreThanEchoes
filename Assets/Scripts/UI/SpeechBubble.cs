using UnityEngine;
public class SpeechBubble : MonoBehaviour {
    [SerializeField] private TMPWrapper text;
    public Transform point;
    public void ShowText(string whatToShow, float speed) {
        text.ShowText($"<color=black>{whatToShow}</color>", speed, 3, () => {
            Destroy(gameObject);
        });
    }

    void Update() {
        transform.position = point.position;
    }
}
