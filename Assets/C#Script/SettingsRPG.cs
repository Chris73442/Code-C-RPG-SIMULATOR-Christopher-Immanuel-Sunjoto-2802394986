using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [Header("UI")]
    public Slider soundSlider;
    public Button backButton;

    void Start()
    {
        // 🔊 Ambil volume LANGSUNG dari AudioManager
        if (AudioManager.instance != null)
        {
            float currentVolume = AudioManager.instance.GetVolume();

            // Set slider TANPA memicu event
            soundSlider.SetValueWithoutNotify(currentVolume);
        }

        // Listener slider
        soundSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Listener tombol back
        backButton.onClick.AddListener(BackToMenu);
    }

    void OnVolumeChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(value);
        }
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
