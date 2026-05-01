using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

// Button can change color of it's background, but not text.
// This utility class sloves the issue.

[RequireComponent(typeof(Button))]
public class TextColorChanger : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI targetText;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;
    public bool isSelected = false;
    private Button button;
    void Start() {
        button = GetComponent<Button>();
    }

    public void UpdateColor() {
        if (isSelected)
            targetText.color = selectedColor;
        else
            targetText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isSelected && EventSystem.current.currentSelectedGameObject != gameObject) {
            button.Select();
        }
    }

    public void OnSelect(BaseEventData eventData) {
        isSelected = true;
        UpdateColor();
    }

    public void OnDeselect(BaseEventData eventData) {
        isSelected = false;
        UpdateColor();
    }
}
