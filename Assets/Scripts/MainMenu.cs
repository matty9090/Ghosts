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
        menuItems[2].onClick.AddListener(btn_about);
    }

    void btn_onPlay() {
        SceneManager.LoadScene("Game");
    }

    void btn_quit() {
        Application.Quit();
    }

    void btn_about() {
        SceneManager.LoadScene("About");
    }
}
