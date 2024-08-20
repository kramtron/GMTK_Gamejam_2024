using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject levelSelector;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject pauseMenu;

    private bool isPaused= false;

    private InputMapping controls;

    private void Awake()
    {
        controls = new InputMapping();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Pause.started += OnMenuClicked;
    }

    private void OnDisable()
    {

        controls.Gameplay.Pause.started -= OnMenuClicked;

    }



    private void OnMenuClicked(InputAction.CallbackContext context)
    {
        if (isPaused) 
        { 
            Resume();
        }
        else
        {
            Pause();
        }


    }

    public void Pause()
    {

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }



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
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;

        }
        SceneManager.LoadScene(sceneName);

    }

}
