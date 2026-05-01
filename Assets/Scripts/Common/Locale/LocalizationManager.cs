// using System.Collections.Generic;
// using Newtonsoft.Json;
// using UnityEngine;
// public class LocalizationManager : MonoBehaviour 
// {
//     [SerializeField] private string folderName = null;
//     [SerializeField] private GlobalSettings globalSettings;
//     private Localization localization;
//     [HideInInspector] public static LocalizationManager instance = null;
//     void Awake()
//     {
//         if(instance != null) {
//             Debug.LogWarning("Two localization managers detected...");
//             Destroy(gameObject);
//             return;
//         }
//         instance = this;
//
//         LoadLocals();
//     }
//
//     public void LoadLocals(GameLanguage language) {
//         string pathToResource = globalSettings.GetPathToLocals(language);
//         if (folderName != null) {
//             pathToResource = $"{pathToResource}/{folderName}";
//         }
//
//         TextAsset[] jsons = Resources.LoadAll<TextAsset>(pathToResource);
//     
//         if (jsons == null) {
//             Debug.LogError($"Cannot load file(s) with localization: \"{pathToResource}\"");
//             return;
//         }
//
//         if (jsons.Length == 0) {
//             Debug.LogError($"Cannot load file(s) with localization: \"{pathToResource}\"");
//             return;
//         }
//
//         localization = JsonConvert.DeserializeObject<Localization>(jsons[0].text);
//         Debug.Log($"Loaded new file with locales of id: {localization.fileID}");
//
//         for (int i = 1; i < jsons.Length; i++) {
//
//             TextAsset json = jsons[i];
//
//             Localization lPart = JsonConvert.DeserializeObject<Localization>(json.text);
//
//             Debug.Log($"Loaded new file with locales of id: {lPart.fileID}");
//             localization.Connect(ref lPart);
//             // foreach(CharacterDialogue a in lPart.dialogues) {
//             //     Debug.Log($"Loaded dialogue \"{a.id}\"");
//             // }
//         }
//     }
//
//     public void LoadLocals() {
//         LoadLocals(globalSettings.currentLanguage);
//     }
//
//     public CharacterDialogue? GetDialogue(string id) {
//         foreach(var d in localization.dialogues) {
//             if (d.id == id) {
//                 return d;
//             }
//         }
//         Debug.LogWarning($"Cannot find dialogue of id: \"{id}\"");
//         return null;
//     }
//
//     public UIText? GetUIText(string id) {
//         foreach(var t in localization.ui) {
//             if (t.id == id) {
//                 return t;
//             }
//         }
//         Debug.LogWarning($"Cannot find ui text of id: \"{id}\"");
//         return null;
//     }
// }