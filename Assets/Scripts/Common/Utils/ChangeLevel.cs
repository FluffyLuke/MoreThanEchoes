using DG.Tweening;
using UnityEngine;

public class ChangeLevel : MonoBehaviour {
    public string levelName = "";
    public void Change() {
        //black.TransitionIn(fade, Ease.Linear, () => StaticUtils.ChangeLevel(levelName, ""));
        StaticUtils.ChangeLevel(levelName, "");
    }

    public void Exit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}