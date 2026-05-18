using UnityEngine;

[ExecuteAlways]
public class PlayerShader : MonoBehaviour {
    private static readonly int playerPosID = Shader.PropertyToID("_PlayerPos");
    void Update() {
        Shader.SetGlobalVector(playerPosID, new Vector2(transform.position.x, transform.position.y));
    }
} 