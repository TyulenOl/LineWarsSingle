using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public MainMenuManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public void LoadSingleGame()
    {
        SceneTransition.LoadScene(SceneName.SingleGame);
    }
}
