using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour {
    [SerializeField] public GlobalSettings settings;
    [SerializeField] private SoundDatabase soundDatabase;
    private Dictionary<string, SoundAsset> lookup;
    [Header("Buses")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup bus_main;
    [SerializeField] private AudioMixerGroup bus_sfx;
    [SerializeField] private AudioMixerGroup bus_ambient;
    [Header("Volume")]
    [Range(-80, 20)]
    [SerializeField] private float maxVolume_dB;
    [Range(-80, 20)]
    [SerializeField] private float minVolume_dB;
    public static SoundManager instance;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }

        instance = this;

        lookup = soundDatabase.sounds.ToDictionary(s => s.id);
    }

    void Start() {
        if (bus_main == null)
            Debug.LogError("Main audio bus is not assigned!");

        if (bus_sfx == null)
            Debug.LogError("SFX bus is not assigned!");

        if (bus_ambient == null)
            Debug.LogError("Ambient bus is not assigned!");

        UpdateValues();
        settings.SettingsUpdated.AddListener(UpdateValues);
    }

    public bool PlayAndLoop(string id, Vector3 position, out SoundHandle handle) {
        handle = default;
        if (!lookup.TryGetValue(id, out SoundAsset sound))
        {
            Debug.LogError($"Cannot found asset of id: \"{id}\"");
            return false;
        }

        GameObject gameObject = new GameObject("SoundSource");
        gameObject.transform.position = position;
        gameObject.transform.SetParent(this.transform);

        AudioSource source = gameObject.AddComponent<AudioSource>();
        AudioClip clip = sound.GetRandomClip();
        source.resource = clip;
        source.pitch = Random.Range(sound.pitchRange.x, sound.pitchRange.y);
        source.volume = sound.volume;
        source.loop = true;
        
        switch (sound.busID) {
            case AudioBusID.NotDefined:
                source.outputAudioMixerGroup = bus_main;
                break;
            case AudioBusID.Ambient:
                source.outputAudioMixerGroup = bus_ambient;
                break;
            case AudioBusID.SFX:
                source.outputAudioMixerGroup = bus_sfx;
                break;
            default:
                Debug.LogError("wtf?");
                break;
        }

        source.Play();
        handle = new SoundHandle(source);

        return true;
    }

    public bool PlayOneShot(string id, Vector3 position, out SoundHandle handle) {
        if (!lookup.TryGetValue(id, out SoundAsset sound))
        {
            Debug.LogError($"Cannot found asset of id: \"{id}\"");
            handle = new SoundHandle(default);
            return false;
        }

        GameObject gameObject = new GameObject("SoundSource");
        gameObject.transform.position = position;
        gameObject.transform.SetParent(this.transform);

        AudioSource source = gameObject.AddComponent<AudioSource>();
        AudioClip clip = sound.GetRandomClip();
        source.resource = clip;
        source.pitch = Random.Range(sound.pitchRange.x, sound.pitchRange.y);
        source.volume = sound.volume;
        source.loop = false;

        switch (sound.busID) {
            case AudioBusID.NotDefined:
                source.outputAudioMixerGroup = bus_main;
                break;
            case AudioBusID.Ambient:
                source.outputAudioMixerGroup = bus_ambient;
                break;
            case AudioBusID.SFX:
                source.outputAudioMixerGroup = bus_sfx;
                break;
            default:
                Debug.LogError("wtf?");
                break;
        }

        source.PlayOneShot(clip);
        handle = new SoundHandle(source);

        // Destroy(gameObject, clip.length);
        return true;
    }

    public bool PlayOneShot(string id, Vector3 position) {
        bool ifSuccess = PlayOneShot(id, position, out SoundHandle handle);
        if (ifSuccess) {
            // Destroy(handle.source.gameObject, handle.source.clip.length);
        }
        return ifSuccess;
    }

    private void UpdateValues() {
        float volumeDelta = Mathf.Abs(maxVolume_dB - minVolume_dB);

        if (volumeDelta == 0) {
            Debug.LogError("Volume delta = 0. Changes in volume will not be applied");
        }

        float masterVolume = minVolume_dB + volumeDelta * settings.Volume_Main;
        float sfxVolume = minVolume_dB + volumeDelta * settings.Volume_Sfx;
        float ambientVolume = minVolume_dB + volumeDelta * settings.Volume_Ambient;

        masterVolume = settings.Volume_Main == 0 ? -80 : masterVolume;
        sfxVolume = settings.Volume_Sfx == 0 ? -80 : sfxVolume; 
        ambientVolume = settings.Volume_Ambient == 0 ? -80 : ambientVolume;

        mixer.SetFloat("MasterVolume", masterVolume);
        mixer.SetFloat("SFXVolume", sfxVolume);
        mixer.SetFloat("AmbientVolume", ambientVolume);
    }
}