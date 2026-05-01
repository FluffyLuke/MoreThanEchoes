using UnityEngine;

[CreateAssetMenu(menuName = "Audio/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    public SoundAsset[] sounds;
    public SoundAsset[] ambients;
}