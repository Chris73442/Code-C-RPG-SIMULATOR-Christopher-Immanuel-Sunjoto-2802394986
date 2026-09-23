using UnityEngine;
using UnityEngine.SceneManagement;

public class Failure : MonoBehaviour
{
    public string nextSceneName = "BeforeGameplayScene";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MainmenuScene");
        }
    }
}

