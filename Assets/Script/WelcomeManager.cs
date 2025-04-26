using UnityEngine.SceneManagement;
using UnityEngine;

public class WelcomeManager : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(LoadScene), 2);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("Selection");
    }    
}
