using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

public enum EnemySide {
    Right  = 0,
    Left   = 1,
    Up     = 2
}

public enum PlayerLookingDirection {
    Right   = 0,
    Left    = 1,
    Up      = 2,
    Main    = 3
}

[Serializable]
public struct EnemyPrefabHolder {
    public GameObject enemyPrefab;
    public EnemySide side;
}

[RequireComponent(typeof(Minigame))]
public class MinigameEnemyBrain: MonoBehaviour {
    private Minigame mg;
    [Header("Spawn")]
    // Left, Up, Right
    [SerializeField] private EnemyPrefabHolder[] enemyPrefabs;
    private MinigameEnemy enemyInstance;
    private EnemySide enemyInstanceSide;
    private PlayerLookingDirection looking = PlayerLookingDirection.Main;
    [Range(1.0f, 100.0f)]
    public float minTimeToSpawn = 10f;
    [Range(1.0f, 100.0f)]
    public float maxTimeToSpawn = 100f;
    [HideInInspector] public bool attackedFlag = false;

    void Awake() {
        mg = GetComponent<Minigame>();
    }


    // This two functions are used by the minigame main component.
    public void StartMiniGame() {
        CountDownToMonsterSpawn(); looking = PlayerLookingDirection.Main;
    }

    public void EndMinigame() {
        StopAllCoroutines();
        if (enemyInstance != null) {
            enemyInstance.onHide.RemoveAllListeners();
            enemyInstance.onAttack.RemoveAllListeners();
            enemyInstance.Hide();
            enemyInstance = null;
        }
    }

    public void CountDownToMonsterSpawn() {
        if (attackedFlag) return;

        // Destroy hide current enemy if present.
        // WARNING: This causes infinite loop
        // if (enemyInstance != null) {
        //     Debug.LogWarning("New enemy should not be spawned before current one doesn't hide.");
        //     enemyInstance.Hide();
        //     enemyInstance = null;
        // }

        float waitFor = UnityEngine.Random.Range(minTimeToSpawn, maxTimeToSpawn);
        Debug.Log($"Counting to a new enemy spawn: {waitFor} seconds...");

        DoSomethingAfter.After(this, waitFor, () => {
            // ==============================
            // === Get the correct prefab ===
            // ==============================
            int randomIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
            EnemyPrefabHolder randomPrefab = enemyPrefabs[randomIndex];

            Debug.Log($"Spawning new enemy on side '{randomPrefab.side}'.");
            
            // If player is looking at that particular side, then change it
            if ((int)looking == (int)randomPrefab.side) {
                Debug.Log($"Must change side, player is looking in direction '{looking}'.");
                
                if (looking == PlayerLookingDirection.Up) randomPrefab = findPrefabFromSide(EnemySide.Right);
                else if (looking == PlayerLookingDirection.Left) randomPrefab = findPrefabFromSide(EnemySide.Right);
                else if (looking == PlayerLookingDirection.Right) randomPrefab = findPrefabFromSide(EnemySide.Left);
                else Debug.LogError("This code should not be reachable?");
            }

            // =======================================
            // === Set parameters for new monsters ===
            // =======================================

            enemyInstanceSide = randomPrefab.side;
            enemyInstance = Instantiate(randomPrefab.enemyPrefab).GetComponent<MinigameEnemy>();

            if (randomPrefab.side == EnemySide.Up) {
                enemyInstance.showPos = mg.activePillar.shownUp.transform;
                enemyInstance.initPos = mg.activePillar.hiddenUp.transform;
            }
            
            switch (randomPrefab.side) {
                case EnemySide.Right:
                    enemyInstance.killPos = mg.activePillar.killRight.transform;
                    break;
                case EnemySide.Left:
                    enemyInstance.killPos = mg.activePillar.killLeft.transform;
                    break;
                case EnemySide.Up:
                    enemyInstance.killPos = mg.activePillar.killUp.transform;
                    break;
            }

            // ===========================
            // === Add event callbacks ===
            // ===========================

            enemyInstance.onHide.AddListener(CountDownToMonsterSpawn);
            enemyInstance.onAttack.AddListener(() => {
                attackedFlag = true;
            });

            enemyInstance.Show();
        });
    }

    private EnemyPrefabHolder findPrefabFromSide(EnemySide side) {
        foreach (var prefab in enemyPrefabs) {
            if (prefab.side == side) return prefab;
        }

        Debug.LogError($"Cannot find enemy from side '{side}'. Returning a default one.");
        return enemyPrefabs[0];
    }

    #region CameraFunctions
    public void OnPlayerLookLeft() {
        looking = PlayerLookingDirection.Left;
        if (attackedFlag == true) return;
        
        var currentEnemy = enemyInstance;
        enemyInstance = null;
        if (enemyInstanceSide == EnemySide.Left) currentEnemy?.Hide();
    }
    public void OnPlayerLookRight() {
        looking = PlayerLookingDirection.Right;
        if (attackedFlag == true) return;

        var currentEnemy = enemyInstance;
        enemyInstance = null;
        if (enemyInstanceSide == EnemySide.Right) currentEnemy?.Hide();
    }
    public void OnPlayerLookUp() {
        looking = PlayerLookingDirection.Up;
        if (attackedFlag == true) return;

        var currentEnemy = enemyInstance;
        enemyInstance = null;
        if (enemyInstanceSide == EnemySide.Up) currentEnemy?.Hide();
    }

    public void OnPlayerLookMain() {
        looking = PlayerLookingDirection.Main;
    }

    #endregion
}