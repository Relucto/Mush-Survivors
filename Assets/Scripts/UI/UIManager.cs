using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IAwaitable
{
    public static UIManager Instance { get; private set; }

    public GameObject pauseMenu;
    public Animator faderAnim;

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
        if (InputManager.Instance.ReadPause())
        {
            EnablePauseMenu(Time.timeScale == 1 ? true : false);
        }
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
        StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            // Play animation
            faderAnim.Play("FadeOut", -1, 0);

            do
            {
                yield return null;
            } while (faderAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

            SceneManager.LoadScene(nextScene);
        }
    }
}
