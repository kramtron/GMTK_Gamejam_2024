using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{

    [SerializeField] GameObject blood;
    [SerializeField] float waitTime;

    public void Kill()
    {
        Instantiate(blood, transform.position, Quaternion.identity);
        StartCoroutine(WaitDeath());
        
    }


    private IEnumerator WaitDeath() {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
