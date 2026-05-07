using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameLanguage {
    English,
}

[CreateAssetMenu(menuName = "Misc/GlobalSettings")]
public class GlobalSettings : ScriptableObject {
    public UnityEvent SettingsUpdated = new();
    [Serializable]
    private struct GameLanguagePath {
        public GameLanguage lang;
        public string path;
    }
    [SerializeField] private List<GameLanguagePath> pathToLocals = new();
    public GameLanguage currentLanguage = GameLanguage.English;
    public GameLanguage defaultLanguage = GameLanguage.English;
    public string GetPathToLocals(GameLanguage language) {
        foreach(GameLanguagePath l in pathToLocals) {
            if (l.lang == language) {
                return l.path;
            }
        }
        return null;
    }
    public string GetPathToLocals() {
        return GetPathToLocals(currentLanguage);
    }

    [Header("Volume")]
    [Range(0, 1)]
    [SerializeField] private float _volume_main;
    public float Volume_Main {
        get => _volume_main;
        set {
            value = Mathf.Max(0, value);
            value = Mathf.Min(1, value);
            _volume_main = value;
            SettingsUpdated.Invoke();
        }
    }
    [Range(0, 1)]
    [SerializeField] private float _volume_sfx;
    public float Volume_Sfx {
        get => _volume_sfx;
        set {
            value = Mathf.Max(0, value);
            value = Mathf.Min(1, value);
            _volume_sfx = value;
            SettingsUpdated.Invoke();
        }
    }
    [Range(0, 1)]
    [SerializeField] private float _volume_ambient;
    public float Volume_Ambient {
        get => _volume_ambient;
        set {
            value = Mathf.Max(0, value);
            value = Mathf.Min(1, value);
            _volume_ambient = value;
            SettingsUpdated.Invoke();
        }
    }
}