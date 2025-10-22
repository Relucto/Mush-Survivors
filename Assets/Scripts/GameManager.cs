using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.HideCursor();
        
        StartCoroutine(ReadyUp());
    }

    IEnumerator ReadyUp()
    {
        InputManager.Instance.enabled = true;

        while (!InputManager.Instance.IsReady())
            yield return null;
        print("InputManager ready");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
