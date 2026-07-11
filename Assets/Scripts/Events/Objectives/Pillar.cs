using UnityEngine;

public class Pillar : MonoBehaviour {
    [Range(1,3)]
    public int number;
    public void PlayMinigame() {
        PlayerEventBus.stateInspect.Invoke(number);
    }
}