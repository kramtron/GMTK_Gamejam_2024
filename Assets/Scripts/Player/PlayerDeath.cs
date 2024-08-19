using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] float waitTime;

    public void PlayerDie(GameObject player) {
        Destroy(player);
        StartCoroutine(WaitDeath());
    }

    private IEnumerator WaitDeath() {
        yield return new WaitForSeconds(waitTime);
        FindObjectOfType<LevelLoader>().LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

}
