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
        start.onClick.AddListener(PlayGame);
        Exit.onClick.AddListener(ExitGame);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("Demo");
        Cursor.visible = false;
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
