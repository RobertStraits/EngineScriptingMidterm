using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void StartGameButtonClicked()
    {
        SceneManager.LoadScene("Scene");
    }
    public void CreditsButtonClicked()
    {
        SceneManager.LoadScene("CreditScene");
    }
}