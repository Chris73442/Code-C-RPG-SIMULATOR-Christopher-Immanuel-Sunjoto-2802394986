using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScroll : MonoBehaviour
{
    public RectTransform creditTransform;
    public float scrollSpeed = 30f;
    public float endYPosition = 700f;

    private bool reachedEnd = false;

    void Update()
    {
        // Kalau tekan Enter kapan saja, langsung balik ke Main Menu
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }

        if (!reachedEnd)
        {
            creditTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (creditTransform.anchoredPosition.y >= endYPosition)
            {
                reachedEnd = true;
            }
        }
    }
}
