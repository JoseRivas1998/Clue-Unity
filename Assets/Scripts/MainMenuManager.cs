using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public Button playButton;
    public Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(() => { SceneManager.LoadScene("SelectCharacter"); });
    }
}
