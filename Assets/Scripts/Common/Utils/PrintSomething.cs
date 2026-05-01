using UnityEngine;

public class PrintSomething : MonoBehaviour {
    public string whatToPrint = "Something...";

    public void Print() {
        Debug.Log(whatToPrint);
    }
    public void PrintWarning() {
        Debug.LogWarning(whatToPrint);
    }
    public void PrintError() {
        Debug.LogError(whatToPrint);
    }
}