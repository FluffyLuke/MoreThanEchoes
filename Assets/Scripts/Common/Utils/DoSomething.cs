using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DoSomething : MonoBehaviour {
    [SerializeField] private bool runOnStart = true;
    public UnityEvent WhatToDo;
    void Start() {
        if (runOnStart) {
            DoTheThing();
        }
    }

    public void DoTheThing() {
        WhatToDo.Invoke();
    }
}