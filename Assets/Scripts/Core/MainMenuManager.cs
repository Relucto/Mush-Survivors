using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioSystem;

public class MainMenuManager : MonoBehaviour
{
    public Animator fader;
    public AudioPair hoverSound, clickSound;
    public AudioChannel sfx;

    void Start()
    {
        fader.Play("FadeIn");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayClick()
    {
        sfx.Play(clickSound.clip, clickSound.volume, clickSound.pitchVariance);
    }

    public void PlayHover()
    {
        sfx.Play(hoverSound.clip, hoverSound.volume, hoverSound.pitchVariance);
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

    public void QuitGame()
    {
        StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            fader.Play("FadeOut");

            yield return new WaitForSeconds(1.5f);

            Application.Quit();
            print("Quit game");
        }
    }
}
