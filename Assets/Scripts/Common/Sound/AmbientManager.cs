using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientManager : MonoBehaviour {
    [Header("Sounds")]
    [SerializeField] private SoundDatabase soundDatabase;
    private Dictionary<string, SoundAsset> lookup;
    [SerializeField] private AudioSource source1, source2;
    private bool usedSource; // false = source1, true = source2
    private Coroutine fadeInCoroutine = null;
    private Coroutine fadeOutCoroutine = null;
    [HideInInspector] public static AmbientManager instance;
    public static string lastAmbientID = null;
    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;

        lookup = soundDatabase.ambients.ToDictionary(s => s.id);
    }
    void Start() {
        if (source1 == null || source2 == null) {
            Debug.Log("Ambient manager has at least one not assigned audio source!");
        }
    }

    public bool PlayAmbient(string id, float fadeDuration) {
        if (!lookup.TryGetValue(id, out SoundAsset sound))
        {
            Debug.LogError($"Cannot found asset of id: \"{id}\"");
            return false;
        }

        Debug.Log($"Changing ambient to: {id}");

        if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
        if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);

        AudioSource newSource = usedSource ? source1 : source2;
        AudioSource currentSource = usedSource ? source2 : source1;

        fadeInCoroutine = StartCoroutine(fadeIn(sound, fadeDuration, newSource));
        fadeOutCoroutine = StartCoroutine(fadeOut(sound, fadeDuration, currentSource));

        usedSource = !usedSource;

        lastAmbientID = id;

        return true;
    }

    public void StopAmbient() {
        Debug.Log($"Stopping ambient");
        if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
        if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);

        source1.clip = null;
        source1.volume = 0;

        source2.clip = null;
        source2.volume = 0;
    }

    public IEnumerator fadeIn(SoundAsset sound, float fadeDuration, AudioSource source) {
        // New Ambient
        AudioClip clip = sound.GetRandomClip();
        source.clip = clip;
        source.pitch = Random.Range(sound.pitchRange.x, sound.pitchRange.y);
        source.volume = 0;
        source.loop = true;
        source.Play();

        // Fade in
        float elapsed = 0f;
        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, sound.volume, elapsed / fadeDuration);
            yield return null;
        }
        source.volume = sound.volume;
        fadeInCoroutine = null;
    }

    public IEnumerator fadeOut(SoundAsset sound, float fadeDuration, AudioSource source) {
        float startValue = source.volume;
        float elapsed = 0f;
  
        // Fade out
        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startValue, 0f, elapsed / fadeDuration);
            yield return null;
        }
        source.volume = 0;

        source.volume = sound.volume;
        source.Stop();

        fadeOutCoroutine = null;
    }
}