using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button buttonPlaySingle;
    public Button buttonPlayMulti;

    private void Awake()
    {
        buttonPlaySingle.onClick.AddListener(PlaySingle);
        buttonPlayMulti.onClick.AddListener(PlayMulti);

    }

    private void PlaySingle()
    {
        SceneManager.LoadScene(1);
    }

    private void PlayMulti()
    {
        SceneManager.LoadScene(2);
    }
}
