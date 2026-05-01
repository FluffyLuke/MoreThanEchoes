using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderWraper : MonoBehaviour 
{
    public bool IsColliding {
        get;
        private set;
    }
    public bool IsTriggering {
        get;
        private set;
    }

    [Header("Tags")]
    public string[] includedTags = new string[] {};
    [Header("Events")]
    public UnityEvent<Collision> OnCollisionEnterEvent = new();
    public UnityEvent<Collision> OnCollisionExitEvent = new();
    public UnityEvent<Collider> OnTriggerEnterEvent = new();
    public UnityEvent<Collider> OnTriggerExitEvent = new();
    void Start() {
        GetComponent<Collider>();
    }
    void OnCollisionEnter(Collision collision) {
        if (!includedTags.Contains(collision.gameObject.tag) && includedTags.Length != 0) return;
        IsColliding = true;
        OnCollisionEnterEvent.Invoke(collision);
    }
    void OnCollisionExit(Collision collision) {
        if (!includedTags.Contains(collision.gameObject.tag) && includedTags.Length != 0) return;
        IsColliding = false;
        OnCollisionExitEvent.Invoke(collision);
    }
    void OnTriggerEnter(Collider other) {
        if (!includedTags.Contains(other.gameObject.tag) && includedTags.Length != 0) return;
        IsTriggering = true;
        OnTriggerEnterEvent.Invoke(other);
    }
    void OnTriggerExit(Collider other) {
        if (!includedTags.Contains(other.gameObject.tag) && includedTags.Length != 0) return;
        IsTriggering = false;
        OnTriggerExitEvent.Invoke(other);
    }
}