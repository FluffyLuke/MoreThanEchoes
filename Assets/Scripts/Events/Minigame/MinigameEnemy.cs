using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
public class MinigameEnemy : MonoBehaviour {
    [Header("Sounds")]
    public string showSoundID;
    public string hideSoundID;
    public string attackSoundID;
    [Header("Speed")]
    public float showSpeed = 2;
    public float hideSpeed = 1;
    public float timeToKill = 2;
    [Header("Positions")]
    public Transform initPos;
    public Transform showPos;
    public Transform killPos;
    [Header("Events")]
    public UnityEvent onAttack = new();
    public UnityEvent onHide = new();

    public void Show() {
        Debug.Log("Showing enemy...");
        transform.position = initPos.position;
        transform.DOMove(showPos.position, showSpeed);

        SoundManager.instance.PlayOneShot(showSoundID, gameObject);

        DoSomethingAfter.After(this, timeToKill, () => {
           Attack(); 
        });
    }

    public void Hide() {
        Debug.Log("Hiding enemy...");

        SoundManager.instance.PlayOneShot(hideSoundID, gameObject);

        StopAllCoroutines();
        onHide.Invoke();
        transform
            .DOMove(initPos.position, hideSpeed)
            .OnComplete(() => Destroy(gameObject));
    }
    public void Attack() {
        transform.position = killPos.position;
        SoundManager.instance.PlayOneShot(attackSoundID, gameObject);
        onAttack.Invoke();
    }
}