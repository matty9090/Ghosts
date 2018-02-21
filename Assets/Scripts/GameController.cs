using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    private GameObject currentWorm;
    private List<GameObject> team1, team2;

    [SerializeField]
    private Text timerText;

    private int currentTeam;
    private float timer;

    [SerializeField]
    float turnTime = 60.9f;

	void Start () {
        timer = turnTime;

        team1 = new List<GameObject>();
        team2 = new List<GameObject>();

        GameObject team1Node = GameObject.Find("Team1Worms");
        GameObject team2Node = GameObject.Find("Team2Worms");

        for (int i = 0; i < team1Node.transform.childCount; i++)
            team1.Add(team1Node.transform.GetChild(i).gameObject);

        for (int i = 0; i < team2Node.transform.childCount; i++)
            team2.Add(team2Node.transform.GetChild(i).gameObject);

        currentTeam = Random.Range(1, 3);
        changeWorm(currentTeam);
	}
	
	void Update () {
        timer -= Time.deltaTime;
        timerText.text = (int)timer + "";

        if (timer < 0.0f) {
            timer = turnTime;
            currentTeam = (currentTeam % 2) + 1;
            changeWorm(currentTeam);
        }
	}

    void changeWorm(int team) {
        if (currentWorm) {
            currentWorm.GetComponent<WormMovement>().isTurn = false;
        }

        if (team == 1)
            currentWorm = team1[Random.Range(0, team1.Count)];
        else
            currentWorm = team2[Random.Range(0, team2.Count)];

        currentWorm.GetComponent<WormMovement>().isTurn = true;
    }

    public float Timer {
        get {
            return timer;
        } set {
            timer = value;
        }
    }
}
