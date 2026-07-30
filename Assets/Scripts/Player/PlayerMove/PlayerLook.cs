using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public enum WhereToLook {
    Left,
    Right,
    WhereLooking,
}

public class PlayerLook : MonoBehaviour {
    [Header("Torch and animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject body;
    [SerializeField] private Light2D torch;
    public float minimumTorchDistance = 0.5f;
    private Camera mainCamera;
    private WhereToLook state = WhereToLook.WhereLooking;
    [Header("View")]
    [SerializeField] private GameObject cameraHolder;
    public Vector2 axisSpeed = new (1, 1);
    public Vector2 moveBy = new (0, 1);
    public float maxDistance = 0.5f;
    public float smoothTime = 0.2f;
    [Header("Events")]
    public UnityEvent<bool> newLookingDirection = new();
    void Start() {
        mainCamera = GameObject.FindWithTag(Tags.MainCameraTag).GetComponent<Camera>();
    }

    void Update() {
        Vector3 direction = GetLookingDirection(transform);
        if (state == WhereToLook.WhereLooking) {
            UpdatePlayerView();

            // Stop player rotation when mouse is too close
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);
            mouseWorldPos.z = 0;
            float distance = Vector3.Distance(mouseWorldPos, transform.position);

            if (distance <= minimumTorchDistance) return;

            UpdateTorchAngle();
            UpdatePlayerSprite();
            SetModelDirection(direction.x > 0);
        } else {
            UpdatePlayerView(state == WhereToLook.Right);
            UpdatePlayerSprite();
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

    private Vector2 currentVelocity = Vector2.zero;
    public void UpdatePlayerView() {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 screenSize = new(Screen.width, Screen.height);
        Vector2 middlePoint = new(Screen.width / 2, Screen.height / 2);
        
        Vector2 offset = mousePosition - middlePoint;
        offset = new (offset.x / middlePoint.x, offset.y / middlePoint.y);
        
        offset.x *= axisSpeed.x;
        offset.y *= axisSpeed.y;

        if (offset.magnitude > maxDistance) {
            offset = Vector2.ClampMagnitude(offset, maxDistance);
        }

        offset.x += moveBy.x;
        offset.y += moveBy.y;

        offset = Vector2.SmoothDamp(cameraHolder.transform.localPosition, offset, ref currentVelocity, smoothTime);
        
        cameraHolder.transform.localPosition = new Vector3(offset.x, offset.y, cameraHolder.transform.localPosition.z);
    }

    public void UpdatePlayerView(bool right) {
        Vector2 offset = right ? new (1, 0) : new (-1, 0);
        
        offset.x *= axisSpeed.x;
        offset.y *= axisSpeed.y;

        if (offset.magnitude > maxDistance) {
            offset = Vector2.ClampMagnitude(offset, maxDistance);
        }

        offset.x += moveBy.x;
        offset.y += moveBy.y;

        offset = Vector2.SmoothDamp(cameraHolder.transform.localPosition, offset, ref currentVelocity, smoothTime);
        
        cameraHolder.transform.localPosition = new Vector3(offset.x, offset.y, cameraHolder.transform.localPosition.z);
    }

    public Vector3 GetLookingDirection(Transform trans) {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerPosition = mainCamera.WorldToScreenPoint(trans.position);
        return mousePos - playerPosition;
    }

    public void SetModelDirection(bool right) {
        float previousValue = body.transform.localScale.x;

        Vector3 newBodyScale = body.transform.localScale;
        if (right) newBodyScale.x = Mathf.Abs(newBodyScale.x);
        else newBodyScale.x = -Mathf.Abs(newBodyScale.x);

        if (previousValue != newBodyScale.x) newLookingDirection.Invoke(right);
        body.transform.localScale = newBodyScale;
    }
}