using UnityEngine;

public enum DepthLevel {
    Background_Deepest  = 5,
    Background_Deeper   = 4,
    Background_Deep     = 3,
    Background          = 2,
    MiddleGround_1      = 1,
    MiddleGround_2      = 0,
    MiddleGround_3      = -1,
    ForeGround          = -2,
    ForeGround_Close    = -3,
    ForeGround_Closer   = -4,
    ForeGround_Closest  = -5,
}

public class SetDepth : MonoBehaviour {
    public DepthLevel startingDepth;
    void Start() {
        Set(startingDepth);
    }

    public void Set(DepthLevel depth) {
        Vector3 currentPosition = transform.position;
        currentPosition.z = (int)depth;
        transform.position = currentPosition;
    }
}