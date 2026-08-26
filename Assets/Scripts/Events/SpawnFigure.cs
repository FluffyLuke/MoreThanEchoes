using System.Collections;
using UnityEngine;

public class SpawnFigure : MonoBehaviour {
    [Header("Speed")]
    public float behindMinimumTime = 5f;
    public float behindMaximumTime = 15f;
    [Header("References")]
    public GameObject figureBehindPrefab;
    private bool lookingRight = true;
    void Start() {
        StartCoroutine(spawnMonsterCooldown(getRandomTime()));
        PlayerEventBus.playerSpawned.AddListener(() => {
            PlayerEventBus.GetPlayerComponent<PlayerLook>().newLookingDirection.AddListener((bool right) => {
                lookingRight = right; 
            });
        });
    }

    private IEnumerator spawnMonsterCooldown(float timeToSpawn) {
        yield return new WaitForSeconds(timeToSpawn);
        Debug.Log("Trying to spawn figure from behind...");
        while(true) {
            if (!lookingRight) {
                yield return new WaitForSeconds(2f);
                continue;
            }
            
            Debug.Log("Figure spawned from behind.");
            SpawnMonsterBehind();
            break;
        }
    }

    public void SpawnMonsterBehind() {
        GameObject newFigure = Instantiate(figureBehindPrefab);
        newFigure.GetComponent<ShadowFigureBehind>().destroyed.AddListener(() => {
           StartCoroutine(spawnMonsterCooldown(getRandomTime())); 
        });
    }

    private float getRandomTime() {
        return UnityEngine.Random.Range(behindMinimumTime, behindMaximumTime);
    }
}