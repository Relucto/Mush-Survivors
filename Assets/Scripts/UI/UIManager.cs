using System.Collections;
using AudioSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IAwaitable
{
    public static UIManager Instance { get; private set; }

    public AudioChannel sfx;
    public AudioPair clickSound, hoverSound;
    public GameObject pauseMenu, upgradeScreen;
    public Animator faderAnim;
    public AudioChannel musicChannel;

    bool isReady;
    public bool IsReady() => isReady;

    void Awake()
    {
        if (Instance == null)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }

    void Start()
    {
        isReady = true;
    }

    void Update()
    {
        // Toggle pause when pressing pause button
        if (InputManager.Instance.ReadPause() && upgradeScreen.activeInHierarchy == false)
        {
            EnablePauseMenu(Time.timeScale == 1 ? true : false);
        }
    }

    public void PlayClick()
    {
        sfx.Play(clickSound.clip, clickSound.volume, clickSound.pitchVariance, GameObject.FindGameObjectWithTag("Player").transform);
    }

    public void PlayHover()
    {
        sfx.Play(hoverSound.clip, hoverSound.volume, hoverSound.pitchVariance, GameObject.FindGameObjectWithTag("Player").transform);
    }

    public void EnablePauseMenu(bool value)
    {
        pauseMenu.SetActive(value);
        PauseGame(value);
    }

    public void PauseGame(bool value)
    {
        Time.timeScale = value == true ? 0 : 1;

        Cursor.visible = value;

        if (value == true)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void ChangeScene(string nextScene)
    {
        if (musicChannel != null)
            musicChannel.Stop();
        
        StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            // Play animation
            faderAnim.Play("FadeOut", -1, 0);

            do
            {
                yield return null;
            } while (faderAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

            Time.timeScale = 1;
            SceneManager.LoadScene(nextScene);
        }
    }
}
