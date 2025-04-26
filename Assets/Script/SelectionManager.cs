using Fusion;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public TMP_InputField nameInput;

    public Button buttonMale;
    public Button buttonFemale;
    void Start()
    {
        buttonMale.onClick.AddListener(() => OnButtonClick("Male"));
        buttonFemale.onClick.AddListener(() => OnButtonClick("Female"));

    }
    void OnButtonClick(string playerClass)
    {
        var playerName = nameInput.text;

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("PlayerClass", playerClass);

        PlayerPrefs.Save();
        SceneManager.LoadScene("SimpleNaturePack_Demo");
    }
}

