using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    public float transitionTime = 1;

    private int levelIndex = -1;

    //
    // Summary:
    //     Call start coroutine RunLoadLevel(string)
    //
    // Parameters:
    //   name:
    //     Name of the scene to load
    public void LoadLevel(string name)
    {
        sceneToLoad = name;
        StartCoroutine(RunLoadLevel());
    }

    //
    // Summary:
    //     Call start coroutine RunLoadLevel(int)
    //
    // Parameters:
    //   index:
    //     Index of the scene to load
    public void LoadLevel(int index)
    {
        levelIndex = index;
        StartCoroutine(RunLoadLevel());
    }

    //
    // Summary:
    //     IEnumerator that calls the StarAnim method from the AnimFade object,
    //     and loads the scene
    //
    IEnumerator RunLoadLevel()
    {
        // Start anim fade
        FindObjectOfType<AnimFade>().StartAnim();
        // Wait X amount of time before load scene
        yield return new WaitForSeconds(transitionTime);
        
        // Check if load scene using index or name
        if (levelIndex != -1)
        {
            SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RunLoadLevel());
        }
    }

}
