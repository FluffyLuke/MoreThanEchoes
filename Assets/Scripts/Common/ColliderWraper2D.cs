using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ColliderWraper2D : MonoBehaviour 
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
    public UnityEvent<Collision2D> OnCollisionEnterEvent = new();
    public UnityEvent<Collision2D> OnCollisionExitEvent = new();
    public UnityEvent<Collider2D> OnTriggerEnterEvent = new();
    public UnityEvent<Collider2D> OnTriggerExitEvent = new();
    void Start() {
        GetComponent<Collider>();
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (!includedTags.Contains(collision.gameObject.tag) && includedTags.Length != 0) return;
        IsColliding = true;
        OnCollisionEnterEvent.Invoke(collision);
    }
    void OnCollisionExit2D(Collision2D collision) {
        if (!includedTags.Contains(collision.gameObject.tag) && includedTags.Length != 0) return;
        IsColliding = false;
        OnCollisionExitEvent.Invoke(collision);
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (!includedTags.Contains(other.gameObject.tag) && includedTags.Length != 0) return;
        IsTriggering = true;
        OnTriggerEnterEvent.Invoke(other);
    }
    void OnTriggerExit2D(Collider2D other) {
        if (!includedTags.Contains(other.gameObject.tag) && includedTags.Length != 0) return;
        IsTriggering = false;
        OnTriggerExitEvent.Invoke(other);
    }
}