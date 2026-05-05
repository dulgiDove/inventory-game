using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundClip
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Audio/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    [Header("제작 시스템 사운드")]
    public SoundClip[] craftingSounds;

    [Header("UI 사운드")]
    public SoundClip[] uiSounds;

    [Header("인벤토리 사운드")]
    public SoundClip[] inventorySounds;

    [Header("게임플레이 사운드")]
    public SoundClip[] gameplaySounds;

    private Dictionary<string, SoundClip> soundDictionary;

    void OnEnable()
    {
        BuildDictionary();
    }

    void BuildDictionary()
    {
        soundDictionary = new Dictionary<string, SoundClip>();

        AddToDictionary(craftingSounds);
        AddToDictionary(uiSounds);
        AddToDictionary(inventorySounds);
        AddToDictionary(gameplaySounds);
    }

    void AddToDictionary(SoundClip[] clips)
    {
        if (clips == null)
            return;

        foreach (var sound in clips)
        {
            if (sound == null || string.IsNullOrEmpty(sound.name) || sound.clip == null)
                continue;

            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary[sound.name] = sound;
            }
            else
            {
                Debug.LogWarning($"중복된 사운드 이름: {sound.name}");
            }
        }
    }

    public AudioClip GetClip(string soundName)
    {
        if (soundDictionary == null || soundDictionary.Count == 0)
            BuildDictionary();

        if (soundDictionary.TryGetValue(soundName, out SoundClip sound))
        {
            return sound.clip;
        }

        return null;
    }

    public float GetVolume(string soundName)
    {
        if (soundDictionary == null || soundDictionary.Count == 0)
            BuildDictionary();

        if (soundDictionary.TryGetValue(soundName, out SoundClip sound))
        {
            return sound.volume;
        }

        return 1f;
    }
}