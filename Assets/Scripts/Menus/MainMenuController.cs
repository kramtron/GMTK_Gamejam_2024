using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject levelSelector;
    [SerializeField] GameObject optionsMenu;
    public void PlayButton()
    {
        mainMenu.SetActive(false);
        levelSelector.SetActive(true);

    }

    public void ReturnButton(GameObject button)
    {

        button.SetActive(false);
        mainMenu.SetActive(true);

    }

    public void OptionsButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void LevelSelector(string sceneName)
    {

        SceneManager.LoadScene(sceneName);

    }

}
