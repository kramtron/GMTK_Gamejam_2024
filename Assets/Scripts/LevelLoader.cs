using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    public Animator transition;
    public float transitionTime = 1;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(sceneToLoad));
    }

    IEnumerator LoadLevel(string name)
    {
        // Play animation, calling the start trigger
        transition.SetTrigger("Start");
        // Wait X amount of time
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(name, LoadSceneMode.Single);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }

}
