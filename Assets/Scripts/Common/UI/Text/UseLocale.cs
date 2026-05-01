// using UnityEngine;

// [RequireComponent(typeof(TMPWrapper))]
// public class UseLocale : MonoBehaviour
// {
//     private enum Type {
//         Dialogue,
//         UIText,
//     };
//     public string id;
//     [SerializeField] private Type type;
//     [SerializeField] private bool showOnStart = true;
//     private TMPWrapper textGUI;
//     void Start() {
//         textGUI = GetComponent<TMPWrapper>();

//         if(id == "") return;

//         LocalizationManager localeManager = LocalizationManager.instance;
//         if (localeManager == null) {
//             Debug.LogError("Cannot find localization manager");
//             return;
//         }

//         switch (type) {
//             case Type.Dialogue: {
//                 CharacterDialogue? dialogue = localeManager.GetDialogue(id);

//                 if (dialogue == null) {
//                     Debug.LogError($"Cannot find dialogue of id: \"{id}\"");
//                     return;
//                 }
//                 textGUI.SetText((CharacterDialogue)dialogue, !showOnStart);
//                 break;
//             }
//             case Type.UIText: {
//                 UIText? uiText = localeManager.GetUIText(id);

//                 if (uiText == null) {
//                     Debug.LogError($"Cannot find ui text of id: \"{id}\"");
//                     return;
//                 }
//                 textGUI.SetText((UIText)uiText, !showOnStart);
//                 break;
//             }
//         }
//     }
// }