using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Animator fader;

    void Start()
    {
        fader.Play("FadeIn");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            fader.Play("FadeOut");

            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene("Game");
        }    
    }
}
