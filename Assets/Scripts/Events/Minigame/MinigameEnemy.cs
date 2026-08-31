using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UIElements;
public class MinigameEnemy : MonoBehaviour {
    [Header("Sounds")]
    public float volume = 1;
    public string showSoundID;
    public string hideSoundID;
    public string attackSoundID;
    public string attackSound2ID;
    public AudioSource source;
    [Header("Speed")]
    public float waitBeforeHideTop = 1f;
    public float showSpeed = 1;
    public float hideSpeed = 2;
    public float timeToKill = 2;
    public float timePenalty = 1;
    [Header("Animation")]
    public bool crawler = false;
    public Animator animator;
    public Transform initPos;
    public Transform showPos;
    public Transform killPos;
    [Header("Events")]
    public UnityEvent onAttack = new();
    public UnityEvent onHide = new();

    private float timeLeft;

    public void Show() {
        animator.SetBool("Crawler", crawler);
        if (crawler) {
            animator.Play("Monster_IdleCrawl");
        } else {
            animator.Play("Monster_Walk");
        }

        Debug.Log("Showing enemy...");
        transform.position = initPos.position;
        transform.rotation = showPos.rotation;
        transform.DOMove(showPos.position, showSpeed);
        SoundManager.instance.PlayOneShot(showSoundID, gameObject, out SoundHandle handle, source);

        timeLeft = timeToKill;

        StartCoroutine(attackCoroutine());
    }

    public void CheckedWrongSpot() {
        SoundManager.instance.PlayOneShot(showSoundID, gameObject, out SoundHandle handle, source);
        handle.source.volume *= 1.5f;
        timeToKill -= timePenalty;
    }

    private IEnumerator attackCoroutine() {
        while(true) {
            if (timeLeft <= 0) {
                Attack();
                yield break;
            } else {
                timeLeft -= Time.deltaTime;
                yield return null;
            }
        }
    }

    public void Hide() {
        if (crawler) {
            StartCoroutine(crawlBack(waitBeforeHideTop));
            return;
        } else {
            SoundManager.instance.PlayOneShot(hideSoundID, gameObject, out SoundHandle handle, source);
            animator.Play("Monster_Walk_Backwards");
        }
        Debug.Log("Hiding enemy...");

        StopAllCoroutines();
        onHide.Invoke();
        transform
            .DOMove(initPos.position, hideSpeed)
            .OnComplete(() => Destroy(gameObject));
    }
    private IEnumerator crawlBack(float timeSecs) {
        yield return new WaitForSeconds(timeSecs);
        SoundManager.instance.PlayOneShot(hideSoundID, gameObject, out SoundHandle handle, source);
        
        animator.SetTrigger("CrawlAway");
        Debug.Log("Hiding enemy...");

        StopAllCoroutines();
        onHide.Invoke();
        transform
            .DOMove(initPos.position, hideSpeed)
            .OnComplete(() => Destroy(gameObject));
    }
    public void Attack() {
        transform.rotation = killPos.rotation;
        transform.position = killPos.position;

        if (crawler) {
            animator.SetTrigger("Jump");
            transform.DOScale(2, 1);
        } else {
            // No attack animation
        }

        SoundManager.instance.PlayOneShot(attackSoundID, gameObject, out SoundHandle handle, source);
        SoundManager.instance.PlayOneShot(attackSound2ID, gameObject, out handle, source);
        onAttack.Invoke();
    }
}