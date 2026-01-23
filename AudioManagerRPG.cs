using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio")]
    public AudioSource bgmSource;
    public AudioClip bgmClip;

    private bool hasStarted = false;
    private bool resetOnMenu = false;

    private const string VOLUME_KEY = "GameVolume";
    private const float DEFAULT_VOLUME = 5f;

    private void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // ===============================
        // 🔑 INIT PLAYER PREFS (BUILD FIX)
        // ===============================
        if (!PlayerPrefs.HasKey(VOLUME_KEY))
        {
            PlayerPrefs.SetFloat(VOLUME_KEY, DEFAULT_VOLUME);
            PlayerPrefs.Save();
        }

        // ===============================
        // 🔊 LOAD SAVED VOLUME
        // ===============================
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY);
        bgmSource.volume = savedVolume;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // RESET SAAT MASUK GAMEPLAY1
        if (scene.name == "Gameplay1Scene")
        {
            ResetMusic();
            return;
        }

        // RESET SAAT BALIK KE MENU SETELAH ENDING
        if (scene.name == "MainMenuScene")
        {
            if (resetOnMenu)
            {
                ResetMusic();
                resetOnMenu = false;
            }

            PlayIfNeeded();
            return;
        }

        // Scene lain (Settings, Credit, Failed, Ending)
        PlayIfNeeded();
    }

    void PlayIfNeeded()
    {
        if (!hasStarted)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
            hasStarted = true;
        }
        else if (!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    void ResetMusic()
    {
        bgmSource.Stop();
        bgmSource.time = 0f;
        hasStarted = false;
        PlayIfNeeded();
    }

    // DIPANGGIL DARI ENDING / CREDIT
    public void MarkEndingFinished()
    {
        resetOnMenu = true;
    }

    // DIPANGGIL DARI SETTINGS SLIDER
    public void SetVolume(float volume)
    {
        float v = Mathf.Clamp(volume, 0f, 0.6f);
        bgmSource.volume = v;

        PlayerPrefs.SetFloat(VOLUME_KEY, v);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return bgmSource.volume;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
