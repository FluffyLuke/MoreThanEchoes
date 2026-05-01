// using UnityEngine;
// using Newtonsoft.Json;
// using System.Linq;
//
// [System.Serializable]
// public struct CharacterDialogue {
//     [JsonProperty("id")] 
//     public string id;
//     [JsonProperty("speaker_name")] 
//     public string speakerName;
//     [JsonProperty("text")] 
//     public string text;
//     [JsonProperty("Speed")] 
//     public float speed;
//     public float ShowingTime() {
//         return text.Count() / speed;
//     }
// }
//
// [System.Serializable]
// public struct UIText {
//     [JsonProperty("id")]
//     public string id;
//     [JsonProperty("text")]
//     public string text;
//     [JsonProperty("Speed")]
//     public float speed;
//
//     public float ShowingTime() {
//         return text.Count() / speed;
//     }
// }
//
// [System.Serializable]
// public struct Localization {
//     [JsonProperty("file_id")] public string fileID;
//     [JsonProperty("file_lang")] public string lang;
//     [JsonProperty("dialogues")] public CharacterDialogue[] dialogues;
//     [JsonProperty("ui")] public UIText[] ui;
//
//     public void Connect(ref Localization other) {
//         if (other.lang != lang) {
//             Debug.LogError("Cannot connect locales of two languages!");
//             return;
//         }
//
//         dialogues = dialogues.Concat(other.dialogues).ToArray();
//         ui = ui.Concat(other.ui).ToArray();
//     }
// }
//
// // Used in some scripts
// public enum DialogueType {
//     Dialogue,
//     Monologue,
// }