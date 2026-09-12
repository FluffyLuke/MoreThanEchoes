using System;
using UnityEngine;

[CreateAssetMenu(menuName = "NoteAsset")]
public class NoteAsset : ScriptableObject
{
    public NotePart[] pages;
}

[Serializable]
public struct NotePart
{
    public string imageID;
}