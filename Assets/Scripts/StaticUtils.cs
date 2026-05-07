using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StaticUtils {
    private static string nextEntrance = "";
    public static void ChangeLevel(string sceneName, string entranceName) {
        Debug.Log($"Changing scene to '{sceneName}'");
        Debug.Log($"Next entrance name is '{entranceName}'");
        nextEntrance = entranceName;
        SceneManager.LoadScene(sceneName);
    }
    public static string GetEntranceName() {
        return nextEntrance;
    }

    public static void DoSomethingAfter(float seconds, MonoBehaviour caller, Action action) {
        caller.StartCoroutine(doSomethingAfterCoroutine(seconds, action));
    }

    private static IEnumerator doSomethingAfterCoroutine(float seconds, Action action) {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }
}