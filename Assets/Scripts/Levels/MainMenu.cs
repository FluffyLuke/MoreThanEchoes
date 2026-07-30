using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChangeLevel))]
public class MainMenu : MonoBehaviour
{
    public string bulbExplodeID = "light_explode";
    public Sprite lightExplosion;
    [SerializeField] private Blackness black;
    [SerializeField] private Image background;
    [Header("Speed")]
    public float fadeStartSecs = 3f;
    public float exitTransitionWait = 2f;
    public float lightDisappearWait = 0.2f;
    void Start() {
        black.TransitionIn(0, DG.Tweening.Ease.InExpo, () => {
            DoSomethingAfter.After(this, 0.5f, () => {
                black.TransitionOut(fadeStartSecs, DG.Tweening.Ease.OutExpo, () => {});
            });
        });
    }
    public void StartGame() {
        background.sprite = lightExplosion;
        DoSomethingAfter.After(this, lightDisappearWait, () => {
            black.TransitionIn(0, DG.Tweening.Ease.Linear, () => {});
        });
        AmbientManager.instance.StopAmbient();
        SoundManager.instance.PlayOneShot(bulbExplodeID, gameObject);
        DoSomethingAfter.After(this, exitTransitionWait, () => {
            GetComponent<ChangeLevel>().Change();
        });
    }
    public void ExitGame() {
        GetComponent<ChangeLevel>().Exit();
    }
}
