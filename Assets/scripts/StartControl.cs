using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartControl : MonoBehaviour
{
    public Button start;
    public Button Exit;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        start.onClick.AddListener(PlayGame);
        Exit.onClick.AddListener(ExitGame);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            PlayGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ExitGame();
        }
    }
    private void PlayGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("Demo");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
