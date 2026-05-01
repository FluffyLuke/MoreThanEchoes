using System;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextFlash : MonoBehaviour
{
    public string[] Frames;
    public float TimeBetweenFrames = 1;
    private TextMeshProUGUI text;
    private float lastTime;
    private int currentFrame = 0;
    void Start() {
        text = GetComponent<TextMeshProUGUI>();
        text.text = Frames[0];

        lastTime = Time.time;
    }
    void Update() {
        if(lastTime + TimeBetweenFrames <= Time.time) {
            lastTime = Time.time;

            currentFrame++;
            if(currentFrame >= Frames.Length) {
                currentFrame = 0;
            }

            text.text = Frames[currentFrame];
        }
    }
}
