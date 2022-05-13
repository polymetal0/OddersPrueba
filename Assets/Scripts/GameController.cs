using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.XR.CoreUtils;

public class GameController : MonoBehaviour
{
    public static PlayingState state;
    public enum PlayingState
    {
        Menu,
        Playing,
        Pause
    }

    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject scoreList;

    [Header("UI Texts")]
    [SerializeField] private Text timeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject scoreListElem;

    [Header("Game Room")]
    [SerializeField] private GameObject playground;

    [Header("Game Assets")]
    [SerializeField] private GameObject[] cubes;
    [SerializeField] private Material[] skyboxes;

    private float t = 0f;
    private float _score = 0f;
    private List<Vector2> timesNscores = new List<Vector2>();
    private Vector2[] _timesNScores;

    private int cubeNum;
    private int cubesSpawned = 0;
    private GameObject[] targets;

    #region Unity Methods
    void Start()
    {
        Cursor.visible = false;
        state = PlayingState.Menu;

        //main menu will appear in front of player's initial position,
        //then stay there for the whole game
        menuPanel.transform.position = pausePanel.transform.position;

        //set random skybox from the assets
        RenderSettings.skybox = skyboxes[Random.Range(0, skyboxes.Length)];

        CreateTargets();
    }

    void Update()
    {
        DisplayTime();

        //no targets left; game has finished
        if (transform.childCount == 0 && state == PlayingState.Playing)
        {
            timeText.text = "00:00";
            scoreText.text = "00000";

            //save scores and sort all scores
            timesNscores.Add(new Vector2(_score, t));

            _timesNScores = timesNscores.ToArray();
            _timesNScores = _timesNScores.OrderByDescending(v => v.x).ToArray();

            Menu();
        }
    }

    #endregion

    #region Game Logic

    public void CreateTargets()
    {
        cubesSpawned = 0;
        //randomize number of targets
        cubeNum = Random.Range(100, 151);

        targets = new GameObject[cubeNum];

        for (int i = 0; i < cubeNum; i++)
        {
            float x = Random.Range(-4.5f, 4.5f);
            float y = Random.Range(0.5f, 4.5f);
            float z = Random.Range(-4.5f, 4.5f);

            //in case the targets are too close
            if (x < 2f && x > -2f && y < 2f && y > -2f && z < 2f && z > -2f)
            {
                x = x <= 0f ? -2f : 2f;
                y = y <= 0f ? -2f : 2f;
                z = z <= 0f ? -2f : 2f;
            }

            Vector3 pos = new Vector3(x, y, z);
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 361), 0);
            float scale = Random.Range(0.75f, 3f);

            GameObject cube = Instantiate(cubes[Random.Range(0, cubes.Length)], gameObject.transform);
            cube.transform.SetPositionAndRotation(pos, rot);
            cube.transform.localScale *= scale;
            cube.SetActive(false);

            targets[i] = cube;
        }

        //sort targets from closer to furthest
        targets = targets.OrderBy((obj) => (obj.transform.position - transform.position).sqrMagnitude).ToArray();
    }

    public IEnumerator SpawnTargets()
    {
        //set all cubes active in 6 seconds
        if (cubesSpawned < cubeNum)
        {
            targets[cubesSpawned].SetActive(true);
            cubesSpawned++;
            yield return new WaitForSeconds(6f / cubeNum);

            StartCoroutine("SpawnTargets");
        }
        else
        {
            StopCoroutine("SpawnTargets");
        }
    }

    public void TargetDestroyed(Transform targetTx)
    {
        //the further away and the smaller the target is, the more points it's worth
        _score += (targetTx.position - transform.position).sqrMagnitude + (1f / targetTx.localScale.sqrMagnitude);
        scoreText.text = string.Format("{0:00000}", _score);
    }

    public void Play()
    {
        //in case a new game is started or current was paused during spawning
        if (state == PlayingState.Menu || cubesSpawned != 0)
        {
            StartCoroutine("SpawnTargets");
        }

        state = PlayingState.Playing;

        //hide main menu and show timer and score
        backgroundPanel.SetActive(false);
        pausePanel.SetActive(false);
        menuPanel.SetActive(false);
        UI.SetActive(true);
    }

    public void Pause()
    {
        //only show pause menu when in play mode 
        if (state == PlayingState.Playing)
        {
            state = PlayingState.Pause;
            backgroundPanel.SetActive(true);
            pausePanel.SetActive(true);
            pausePanel.GetComponent<AudioSource>().Play();

            StopCoroutine("SpawnTargets");
        }
    }
    public void Menu()
    {
        //in case we exitted the game from pause menu, destroy remaining targets
        if (state == PlayingState.Pause)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        state = PlayingState.Menu;

        //show main menu and hide all other game elements
        menuPanel.SetActive(true);
        menuPanel.GetComponent<AudioSource>().Play();
        backgroundPanel.SetActive(true);
        playground.SetActive(false);
        pausePanel.SetActive(false);
        UI.SetActive(false);

        //reset local time and score
        t = 0;
        _score = 0;

        UpdateScoreList();

        CreateTargets();
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    #region Time & Score UI

    private void DisplayTime()
    {
        //only increment time when in play mode
        if (state == PlayingState.Playing)
        {
            t += Time.deltaTime;
            float min = Mathf.FloorToInt(t / 60);
            float s = Mathf.FloorToInt(t % 60);
            timeText.text = string.Format("{0:00}:{1:00}", min, s);
        }
    }

    private void UpdateScoreList()
    {
        //empty old list
        for (int i = 0; i < scoreList.transform.childCount; i++)
        {
            Destroy(scoreList.transform.GetChild(i).gameObject);
        }

        //refill score list
        for (int i = 0; i < _timesNScores.Length && i < 10; i++)
        {
            float min = Mathf.FloorToInt(_timesNScores[i].y / 60);
            float s = Mathf.FloorToInt(_timesNScores[i].y % 60);
            Instantiate(scoreListElem, scoreList.transform).GetComponent<Text>().text = 
                string.Format("{0:00000}", _timesNScores[i].x) + "      " + string.Format("{0:00}:{1:00}", min, s);
        }
    }

    #endregion

}
