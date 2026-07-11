using Unity.Cinemachine;
using UnityEngine;

public class MinigamePillar : MonoBehaviour {
    [Header("Positions")]
    public GameObject shownUp;
    public GameObject hiddenUp;
    [Header("KillPositions")]
    public GameObject killUp;
    public GameObject killRight;
    public GameObject killLeft;

    [Header("Cameras")]
    public CinemachineCamera cameraUp;
    public CinemachineCamera cameraMain;
    public CinemachineCamera cameraLeft;
    public CinemachineCamera cameraRight;
    private CinemachineCamera currentCamera;

    public void DisableCameras() {
        cameraLeft.Priority = 0;
        cameraRight.Priority = 0;
        cameraUp.Priority = 0;
        cameraMain.Priority = 0;
    }

    public void ChangeCurrentCamera(CinemachineCamera current) {
        Debug.Log($"Switching to camera: '{current}'");

        DisableCameras();

        currentCamera = current;

        currentCamera.Priority = 2;
    }
}