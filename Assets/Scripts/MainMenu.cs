using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    Button[] menuItems;

    void Start () {
        menuItems[0].onClick.AddListener(btn_onPlay);
        menuItems[1].onClick.AddListener(btn_quit);
    }

    void btn_onPlay() {
        SceneManager.LoadScene("Game");
    }

    void btn_quit() {
        Application.Quit();
    }
}
