using UnityEngine;

public class BestiaryManager : MonoBehaviour
{
    public Animator fader;
    public NextCage[] cages;
    public WeaponAnimator vine;

    [System.Serializable]
    public class NextCage
    {
        public GameObject[] activateObjects;
        public GameObject[] deactivateObjects;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController.isActive = true;    

        UIManager.Instance.PauseGame(false);    

        fader.Play("FadeIn", -1, 0);
    }

    int walkIndex;
    public void EnableNextCage()
    {
        foreach (GameObject obj in cages[walkIndex].activateObjects)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in cages[walkIndex].deactivateObjects)
        {
            obj.SetActive(false);
        }

        vine.DisableSelf();

        walkIndex++;
    }
}
