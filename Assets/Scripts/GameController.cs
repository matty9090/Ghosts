using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    private GameObject currentWorm;
    private List<GameObject> team1, team2;

    [SerializeField]
    private PositionCamera cam;

    [SerializeField]
    private GameObject weaponUI;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject btnSkip;

    [SerializeField]
    private GameObject btnExit;

    [SerializeField]
    private GameObject selectedWeapon;

    [SerializeField]
    private Text timerText;

    private int currentTeam;
    private float timer;
    private bool showWeaponUI;

    [SerializeField]
    float turnTime = 60.9f;

    [SerializeField]
    GameObject gameOverTxt;

    [SerializeField]
    GameObject gameOverFade;

    [SerializeField]
    AudioSource winningMusic;

    [SerializeField]
    AudioSource gameMusic;

    [SerializeField]
    AudioSource clockTick;

    [SerializeField]
    AudioSource deathNoise;

    public enum GameStates { Playing, Panning, GameOver }
    public GameStates gameState;
    public bool canFire;

    private bool showMenu;

    void Start () {
        DamageNumberController.initialize();

        gameState = GameStates.Playing;

        hideUI();

        timer = turnTime;
        showMenu = false;
        menu.SetActive(showMenu);

        team1 = new List<GameObject>();
        team2 = new List<GameObject>();

        GameObject team1Node = GameObject.Find("Team1Worms");
        GameObject team2Node = GameObject.Find("Team2Worms");

        for (int i = 0; i < team1Node.transform.childCount; i++) {
            team1.Add(team1Node.transform.GetChild(i).gameObject);
            team1[i].GetComponent<WormMovement>().setTextColour(Color.cyan);

            pickGhostPosition(1, team1[i]);
        }

        for (int i = 0; i < team2Node.transform.childCount; i++){
            team2.Add(team2Node.transform.GetChild(i).gameObject);
            team2[i].GetComponent<WormMovement>().setTextColour(Color.yellow);

            pickGhostPosition(2, team2[i]);
        }

        currentTeam = Random.Range(1, 3);
        changeWorm();

        gameOverTxt.GetComponent<Text>().enabled = false;
        gameOverFade.GetComponent<Image>().enabled = false;

        btnSkip.GetComponent<Button>().onClick.AddListener(btn_skip);
        btnExit.GetComponent<Button>().onClick.AddListener(btn_exit);
    }

    void btn_skip() {
        changeWorm();
    }

    void btn_exit() {
        SceneManager.LoadScene("MainMenu");
    }

    void pickGhostPosition(int team, GameObject ghost) {
        do {
            float x = (team == 1) ? Random.Range(-7.0f, -1.0f) : Random.Range(1.5f, 7.0f);
            float y = Random.Range(-1.0f, 3.8f);

            Vector2 pos = new Vector2(x, y);
            RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(0.0f, -1.0f), 1.0f);

            if (hit.collider != null) {
                ghost.transform.position = pos;
                return;
            }
        } while(true);
    }

    void Update () {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            showMenu = !showMenu;
            menu.SetActive(showMenu);
        }

        switch (gameState) {
            case GameStates.Playing:
                timer -= Time.deltaTime;
                timerText.text = (int)timer + "";

                if (timer <= 10.9) {
                    timerText.color = new Color(1.0f, 0.24f, 0.24f);

                    if ((int)timer > (int)(timer - Time.deltaTime))
                        clockTick.Play();
                }

                if (timer < 0.0f)
                    changeWorm();

                if (Input.GetKeyUp(KeyCode.U)) {
                    if (showWeaponUI)
                        hideUI();
                    else
                        showUI();
                }

                break;

            case GameStates.Panning:
                if (cam.panned) {
                    cam.panned = false;
                    gameState = GameStates.Playing;

                    timer = turnTime;
                    timerText.color = new Color(0.0f, 1.0f, 0.55f);
                }

                break;

            case GameStates.GameOver:
                gameMusic.Stop();

                break;
        }
	}

    public void changeWorm() {
        canFire = true;
        currentTeam = (currentTeam % 2) + 1;

        if (currentWorm)
            currentWorm.GetComponent<WormMovement>().wormState = WormMovement.WormState.Idle;

        if (currentTeam == 1)
            currentWorm = team1[Random.Range(0, team1.Count)];
        else
            currentWorm = team2[Random.Range(0, team2.Count)];

        weaponUI.GetComponent<WeaponsUI>().setWeapon(currentWorm.GetComponent<WormMovement>().missile);

        currentWorm.GetComponent<WormMovement>().wormState = WormMovement.WormState.Playing;
        currentWorm.GetComponent<WormMovement>().SwapToCrosshair();
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
        deathNoise.Play();
        worm.GetComponent<WormMovement>().takeDamage(100);

        if(currentWorm == worm)
            changeWorm();

        if (team1.Contains(worm))
            team1.Remove(worm);
        else if (team2.Contains(worm))
            team2.Remove(worm);

        if (team1.Count <= 0 || team2.Count <= 0) {
            gameState = GameStates.GameOver;
            winningMusic.Play();

            gameOverTxt.GetComponent<Text>().enabled = true;
            if(team1.Count <= 0)
            {
                gameOverTxt.GetComponent<Text>().color = Color.yellow;
                gameOverTxt.GetComponent<Text>().text = "Yellow Team Wins";
            }
            else
            {
                gameOverTxt.GetComponent<Text>().color = Color.cyan;
                gameOverTxt.GetComponent<Text>().text = "Blue Team Wins";
            }

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

    public void showUI() {
        showWeaponUI = true;
        weaponUI.SetActive(true);
        selectedWeapon.SetActive(false);
    }

    public void hideUI() {
        showWeaponUI = false;
        weaponUI.SetActive(false);
        selectedWeapon.SetActive(true);
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
