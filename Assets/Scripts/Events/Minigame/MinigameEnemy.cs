using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
public class MinigameEnemy : MonoBehaviour {
    [Header("Sounds")]
    public float volume = 1;
    public string showSoundID;
    public string hideSoundID;
    public string attackSoundID;
    public string attackSound2ID;
    public AudioSource source;
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

        SoundManager.instance.PlayOneShot(showSoundID, gameObject, out SoundHandle handle, source);

        DoSomethingAfter.After(this, timeToKill, () => {
           Attack(); 
        });
    }

    public void Hide() {
        Debug.Log("Hiding enemy...");

        SoundManager.instance.PlayOneShot(hideSoundID, gameObject, out SoundHandle handle, source);

        StopAllCoroutines();
        onHide.Invoke();
        transform
            .DOMove(initPos.position, hideSpeed)
            .OnComplete(() => Destroy(gameObject));
    }
    public void Attack() {
        transform.position = killPos.position;
        SoundManager.instance.PlayOneShot(attackSoundID, gameObject, out SoundHandle handle, source);
        SoundManager.instance.PlayOneShot(attackSound2ID, gameObject, out handle, source);
        onAttack.Invoke();
    }
}