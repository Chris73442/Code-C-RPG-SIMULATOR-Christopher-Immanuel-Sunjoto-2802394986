using UnityEngine;
using UnityEngine.SceneManagement;

public class NoticeController : MonoBehaviour
{
    public string nextSceneName = "BeforeGameplayScene";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Gameplay1Scene");
        }
    }
}
