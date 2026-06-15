using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public enum WhereToLook {
    Left,
    Right,
    WhereLooking,
}

public class PlayerLook : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject body;
    [SerializeField] private Light2D torch;
    private Camera mainCamera;
    private WhereToLook state = WhereToLook.WhereLooking;
    void Start() {
        mainCamera = GameObject.FindWithTag(Tags.MainCameraTag).GetComponent<Camera>();
    }

    void Update() {
        UpdatePlayerSprite();

        Vector3 direction = GetLookingDirection(transform);
        if (state == WhereToLook.WhereLooking) {
            UpdateTorchAngle();
            SetModelDirection(direction.x > 0);
        } else {
            SetModelDirection(state == WhereToLook.Right);
        }
    }

    public void SetWhereToLook(WhereToLook newState) {
        state = newState;

        // Reset flashlight, used in animations
        if (state != WhereToLook.WhereLooking) {
            torch.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void UpdatePlayerSprite() {
        Vector3 direction = GetLookingDirection(transform);
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        angle = Mathf.Abs(angle);

        // Front is the left side of X axis
        // Up is 90 degrees, so it goes from 0 to 90 upwards
        // Same goes for going downwards, from 0 to -90
        // Range is from 90 (up) to -90 (down)
        float angleAnimator = 90 - angle;
        animator.SetFloat("degrees", angleAnimator);
    }
    
    public void UpdateTorchAngle() {
        Vector3 direction = GetLookingDirection(torch.transform);
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        angle *= -1;

        torch.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public Vector3 GetLookingDirection(Transform trans) {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerPosition = mainCamera.WorldToScreenPoint(trans.position);
        return mousePos - playerPosition;
    }

    public void SetModelDirection(bool right) {
        Vector3 newBodyScale = body.transform.localScale;
        if (right) newBodyScale.x = Mathf.Abs(newBodyScale.x);
        else newBodyScale.x = -Mathf.Abs(newBodyScale.x);
        body.transform.localScale = newBodyScale;
    }
}