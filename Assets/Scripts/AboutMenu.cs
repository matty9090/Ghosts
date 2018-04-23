using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AboutMenu : MonoBehaviour {
    [SerializeField]
    Button btnBack;

    void Start() {
        btnBack.onClick.AddListener(btn_back);
    } 

    void btn_back() {
        SceneManager.LoadScene("MainMenu");
    }
}
