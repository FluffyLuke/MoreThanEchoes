using UnityEngine;
using UnityEngine.InputSystem;

public enum WhereToLook {
    Left,
    Right,
    WhereLooking,
}

public class PlayerLook : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject body;
    private Camera mainCamera;
    private WhereToLook state = WhereToLook.WhereLooking;
    void Start() {
        mainCamera = GameObject.FindWithTag(Tags.MainCameraTag).GetComponent<Camera>();
    }
    void Update() {
        Vector3 direction = GetLookingDirection();
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        angle = Mathf.Abs(angle);
        angle = 90 - angle;
        animator.SetFloat("degrees", angle);

        if (state == WhereToLook.WhereLooking) {
            SetModelDirection(direction.x > 0);
        } else {
            SetModelDirection(state == WhereToLook.Right);
        }
    }

    public void SetWhereToLook(WhereToLook newState) {
        state = newState;
    }

    public Vector3 GetLookingDirection() {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerPosition = mainCamera.WorldToScreenPoint(transform.position);
        return mousePos - playerPosition;
    }

    public void SetModelDirection(bool right) {
        Vector3 newBodyScale = body.transform.localScale;
        if (right) newBodyScale.x = Mathf.Abs(newBodyScale.x);
        else newBodyScale.x = -Mathf.Abs(newBodyScale.x);
        body.transform.localScale = newBodyScale;
    }
}