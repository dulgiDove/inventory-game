using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    [Header("오디오 소스")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private int maxSFXSources = 10;

    [Header("사운드 라이브러리")]
    [SerializeField] private SoundLibrary soundLibrary;

    [Header("볼륨 설정")]
    [SerializeField][Range(0f, 1f)] private float masterVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float sfxVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;

    [Header("시스템 참조 (자동 연결)")]
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private Inventory inventory;

    private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
    private List<AudioSource> activeSFX = new List<AudioSource>();
    private Transform sfxParent;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSFXPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SubscribeToEvents();
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    void SubscribeToEvents()
    {
        if (craftingSystem != null)
        {
            craftingSystem.OnCraftingStarted += OnCraftingStarted;
            craftingSystem.OnCraftingCompleted += OnCraftingCompleted;
            craftingSystem.OnCraftingUIOpened += OnCraftingUIOpened;
            craftingSystem.OnCraftingUIClosed += OnCraftingUIClosed;
        }

        if (inventory != null)
        {
            inventory.OnInventoryChanged += OnInventoryChanged;
            inventory.OnItemUsed += OnItemUsed;
        }
    }

    void UnsubscribeFromEvents()
    {
        if (craftingSystem != null)
        {
            craftingSystem.OnCraftingStarted -= OnCraftingStarted;
            craftingSystem.OnCraftingCompleted -= OnCraftingCompleted;
            craftingSystem.OnCraftingUIOpened -= OnCraftingUIOpened;
            craftingSystem.OnCraftingUIClosed -= OnCraftingUIClosed;
        }

        if (inventory != null)
        {
            inventory.OnInventoryChanged -= OnInventoryChanged;
            inventory.OnItemUsed -= OnItemUsed;
        }
    }

    void OnCraftingStarted(RecipeData recipe)
    {
        PlaySFX("CraftStart");
        Debug.Log("제작 시작 사운드 재생");
    }

    void OnCraftingCompleted(RecipeData recipe, bool success)
    {
        StartCoroutine(PlayCraftCompleteSoundDelayed(recipe, success));
    }

    IEnumerator PlayCraftCompleteSoundDelayed(RecipeData recipe, bool success)
    {
        AudioClip craftStartClip = soundLibrary.GetClip("CraftStart");

        if (craftStartClip != null)
        {
            yield return new WaitForSecondsRealtime(craftStartClip.length);
        }

        if (success)
        {
            PlaySFX("CraftSuccess");
            Debug.Log($"{recipe.RecipeName} 제작 성공 사운드 재생!");
        }
        else
        {
            PlaySFX("CraftFail");
            Debug.Log("제작 실패 사운드 재생");
        }
    }

    void OnCraftingUIOpened(CrafterType crafterType)
    {
        PlaySFX("UIOpen");
        Debug.Log($"{crafterType} UI 열림 사운드");
    }

    void OnCraftingUIClosed()
    {
        PlaySFX("UIClose");
        Debug.Log("UI 닫힘 사운드");
    }

    void OnInventoryChanged()
    {
    }

    void OnItemUsed(ItemCategory category)
    {
        string soundName = GetItemUseSoundName(category);
        PlaySFX(soundName);
        Debug.Log($"{category} 아이템 사용 사운드: {soundName}");
    }

    string GetItemUseSoundName(ItemCategory category)
    {
        switch (category)
        {
            case ItemCategory.Consumable:
                return "UsePotion";

            case ItemCategory.Weapon:
            case ItemCategory.Armor:
            case ItemCategory.Accessory:
                return "EquipItem";

            case ItemCategory.Material:
                return "UseItem";

            case ItemCategory.QuestItem:
            case ItemCategory.KeyItem:
                return "UseSpecial";

            case ItemCategory.Currency:
                return "UseCoin";

            case ItemCategory.Misc:
            default:
                return "UseItem";
        }
    }

    void InitializeSFXPool()
    {
        GameObject parent = new GameObject("SFX Pool");
        parent.transform.SetParent(transform);
        sfxParent = parent.transform;

        for (int i = 0; i < maxSFXSources; i++)
        {
            CreateSFXSource();
        }
    }

    AudioSource CreateSFXSource()
    {
        GameObject obj = new GameObject("SFX Source");
        obj.transform.SetParent(sfxParent);

        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.gameObject.SetActive(false);

        sfxPool.Enqueue(source);
        return source;
    }

    public void PlaySFX(string soundName, float volumeScale = 1f)
    {
        if (soundLibrary == null)
        {
            Debug.LogWarning("SoundLibrary가 없습니다!");
            return;
        }

        AudioClip clip = soundLibrary.GetClip(soundName);

        if (clip != null)
        {
            PlaySFXClip(clip, volumeScale);
        }
        else
        {
            Debug.LogWarning($"사운드를 찾을 수 없습니다: {soundName}");
        }
    }

    public void PlaySFXClip(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null)
            return;

        AudioSource source = GetAvailableSFXSource();

        if (source != null)
        {
            source.gameObject.SetActive(true);
            source.clip = clip;
            source.volume = sfxVolume * volumeScale * masterVolume;
            source.Play();

            activeSFX.Add(source);
            StartCoroutine(ReturnToPoolWhenFinished(source, clip.length));
        }
    }

    AudioSource GetAvailableSFXSource()
    {
        if (sfxPool.Count > 0)
        {
            return sfxPool.Dequeue();
        }

        if (activeSFX.Count > 0)
        {
            AudioSource oldest = activeSFX[0];
            activeSFX.RemoveAt(0);
            oldest.Stop();
            return oldest;
        }

        return CreateSFXSource();
    }

    System.Collections.IEnumerator ReturnToPoolWhenFinished(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (source != null)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);

            activeSFX.Remove(source);
            sfxPool.Enqueue(source);
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.Play();
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }
}