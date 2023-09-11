using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Image savePanel;
    public Button newGame;
    public Button saveGame;
    public Button cancelButton;
    public Button introduction;
    public Button exitGame;

    private bool isOpened;

    private void Awake()
    {
        newGame.onClick.AddListener(OnShowSavePanel);
        saveGame.onClick.AddListener(OnShowSavePanel);
        cancelButton.onClick.AddListener(OnShowSavePanel);
        exitGame.onClick.AddListener(ExitGame);
    }

    private void Update()
    {
        if (isOpened)
        {
            newGame.enabled = false;
            introduction.enabled = false;
            exitGame.enabled = false;
        }
        else
        {
            newGame.enabled = true;
            introduction.enabled = true;
            exitGame.enabled = true;
        }
    }

    private void OnShowSavePanel()
    {
        isOpened = !isOpened;
        savePanel.gameObject.SetActive(isOpened);
        
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("EXIT GAME");
    }
}
