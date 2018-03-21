using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    private GameObject currentWorm;
    private List<GameObject> team1, team2;

    [SerializeField]
    private PositionCamera cam;

    [SerializeField]
    private GameObject weaponUI;

    [SerializeField]
    private Text timerText;

    private int currentTeam;
    private float timer;
    private bool showUI;

    [SerializeField]
    float turnTime = 60.9f;

    [SerializeField]
    GameObject gameOverTxt;

    [SerializeField]
    GameObject gameOverFade;

    public enum GameStates { Playing, Panning, GameOver }
    public GameStates gameState;

	void Start () {
        DamageNumberController.initialize();

        gameState = GameStates.Playing;

        showUI = false;
        hideUI();

        timer = turnTime;

        team1 = new List<GameObject>();
        team2 = new List<GameObject>();

        GameObject team1Node = GameObject.Find("Team1Worms");
        GameObject team2Node = GameObject.Find("Team2Worms");

        for (int i = 0; i < team1Node.transform.childCount; i++) {
            team1.Add(team1Node.transform.GetChild(i).gameObject);
            team1[i].GetComponent<WormMovement>().setTextColour(Color.cyan);
        }

        for (int i = 0; i < team2Node.transform.childCount; i++){
            team2.Add(team2Node.transform.GetChild(i).gameObject);
            team2[i].GetComponent<WormMovement>().setTextColour(Color.yellow);
        }            

        currentTeam = Random.Range(1, 3);
        changeWorm();

        gameOverTxt.GetComponent<Text>().enabled = false;
        gameOverFade.GetComponent<Image>().enabled = false;
    }
	
	void Update () {
        switch(gameState) {
            case GameStates.Playing:
                timer -= Time.deltaTime;
                timerText.text = (int)timer + "";

                if (timer < 0.0f) {
                    timer = turnTime;
                    changeWorm();
                }

                if (Input.GetKeyUp(KeyCode.U)) {
                    showUI = !showUI;
                    weaponUI.SetActive(showUI);
                }

                break;

            case GameStates.Panning:
                if (cam.panned) {
                    cam.panned = false;
                    gameState = GameStates.Playing;
                }

                break;

            case GameStates.GameOver:

                break;
        }
	}

    public void changeWorm() {
        currentTeam = (currentTeam % 2) + 1;
        timer = 60.9f;

        if (currentWorm)
            currentWorm.GetComponent<WormMovement>().wormState = WormMovement.WormState.Idle;

        if (currentTeam == 1)
            currentWorm = team1[Random.Range(0, team1.Count)];
        else
            currentWorm = team2[Random.Range(0, team2.Count)];

        currentWorm.GetComponent<WormMovement>().wormState = WormMovement.WormState.Playing;
        gameState = GameStates.Panning;
        cam.pan();
    }

    public List<GameObject> getAllWorms() {
        List<GameObject> allWorms = new List<GameObject>();

        allWorms.AddRange(team1);
        allWorms.AddRange(team2);

        return allWorms;
    }

    public void removeWorm(GameObject worm) {
        if(currentWorm == worm)
            changeWorm();

        if (team1.Contains(worm))
            team1.Remove(worm);
        else if (team2.Contains(worm))
            team2.Remove(worm);

        if (team1.Count <= 0 || team2.Count <= 0) {
            gameState = GameStates.GameOver;
            gameOverTxt.GetComponent<Text>().enabled = true;
            gameOverFade.GetComponent<Image>().enabled = true;

            gameOverTxt.GetComponent<Animation>().Play();
            gameOverFade.GetComponent<Animation>().Play();

            if(team1.Count > 0) {
                foreach (GameObject w in team1)
                    w.GetComponent<WormMovement>().wormState = WormMovement.WormState.Idle;
            } else if (team2.Count > 0) {
                foreach (GameObject w in team2)
                    w.GetComponent<WormMovement>().wormState = WormMovement.WormState.Idle;
            }
        }
    }

    public void hideUI() {
        showUI = false;
        weaponUI.SetActive(showUI);
    }

    public float Timer {
        get {
            return timer;
        }
        set {
            timer = value;
        }
    }

    public GameObject CurrentWorm {
        get {
            return currentWorm;
        }
        set {
            currentWorm = value;
        }
    }
}
