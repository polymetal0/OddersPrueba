using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.XR.CoreUtils;

public class GameController : MonoBehaviour
{
    public static PlayingState state;

    public GameObject menuPanel;
    public GameObject backgroundPanel;
    public GameObject pausePanel;
    public GameObject UI;
    public GameObject playground;

    public Text timeText;
    public Text scoreText;

    [SerializeField] private GameObject[] cubes;
    private int cubeNum;
    private int cubesSpawned = 0;
    //private System.Array dist;
    //private System.Array prefabs;
    private GameObject[] sortedTargets;

    float t = 0f;
    float _score = 0f;
    // Start is called before the first frame update
    void Start()
    {
        state = PlayingState.Menu;
        menuPanel.transform.position = pausePanel.transform.position;

        timeText.text = "00:00";

        cubeNum = 1;// (int)Random.Range(100, 150);

        Debug.Log(cubeNum);

        CreateTargets();

    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime();

        if (transform.childCount == 0 && state == PlayingState.Playing)
        {
            Debug.Log("FINISH");
            Menu();
        }
    }

    private void DisplayTime()
    {
        if (state == PlayingState.Playing)
        {
            t += Time.deltaTime;
            float min = Mathf.FloorToInt(t / 60);
            float s = Mathf.FloorToInt(t % 60);
            timeText.text = string.Format("{0:00}:{1:00}", min, s);
        }   
    }

    public void TargetDestroyed()
    {
        _score += 10;
        scoreText.text = string.Format("{0:00000}", _score);
    }

    public void Play()
    {
        if (state == PlayingState.Menu || cubesSpawned != 0)
        {
            StartCoroutine("SpawnTargets");
        }
        state = PlayingState.Playing;
        backgroundPanel.SetActive(false);
        pausePanel.SetActive(false);
        menuPanel.SetActive(false);

        UI.SetActive(true);
    }

    public void Pause()
    {
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
        if (state == PlayingState.Pause)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        state = PlayingState.Menu;
        //menuPanel.transform.position = pausePanel.transform.position;
        //menuPanel.transform.rotation = pausePanel.transform.rotation;
        menuPanel.SetActive(true);
        menuPanel.GetComponent<AudioSource>().Play();
        backgroundPanel.SetActive(true);
        playground.SetActive(false);
        pausePanel.SetActive(false);
        UI.SetActive(false);
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet b in bullets)
        {
            Destroy(b.gameObject);
        }
        t = 0;

        //timeText.text = string.Format("{0:00}:{1:00}", t, t);

        CreateTargets();
    }

    public void CreateTargets()
    {
        GameObject[] unsortedTargets = new GameObject[cubeNum];
        for (int i = 0; i < cubeNum; i++)
        {

            float x = Random.Range(-4.5f, 4.5f);
            float y = Random.Range(0.5f, 4.5f);
            float z = Random.Range(-4.5f, 4.5f);

            if (x < 2f && x > -2f && y < 2f && y > -2f && z < 2f && z > -2f)
            {
                x = x <= 0f ? -2f : 2f;
                y = y <= 0f ? -2f : 2f;
                z = z <= 0f ? -2f : 2f;
            }

            Vector3 pos = new Vector3(x, y, z);
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0); //Random.rotation;
            float scale = Random.Range(0.75f, 3f);
            GameObject cube = Instantiate(cubes[Random.Range(0, cubes.Length - 1)], gameObject.transform);
            cube.transform.SetPositionAndRotation(pos, rot);
            cube.transform.localScale *= scale;
            cube.SetActive(false);

            unsortedTargets[i] = cube;

            //Debug.Log(i + ": " + unsortedTargets[i].name + "\nd = " + (cube.transform.position - Camera.main.transform.position).sqrMagnitude);
        }

        sortedTargets = unsortedTargets.OrderBy((obj) => (obj.transform.position - FindObjectOfType<XROrigin>().transform.position).sqrMagnitude).ToArray();
        //for (int i = 0; i < sortedTargets.Length; i++)
        //{
        //    Debug.Log(i + ": " + sortedTargets[i].name + "\nd = " + (sortedTargets[i].transform.position - Camera.main.transform.position).sqrMagnitude);
       // }
    }

    public IEnumerator SpawnTargets()
    {
        if (cubesSpawned < cubeNum)
        {
            sortedTargets[cubesSpawned].SetActive(true);
            cubesSpawned++;
            yield return new WaitForSeconds(6f / cubeNum);

            StartCoroutine("SpawnTargets");
        }
        else
        {
            cubesSpawned = 0;
            StopCoroutine("SpawnTargets");
            Debug.Log("ALL CUBES SPAWNED");
        }

    }

    public void Quit()
    {
        Application.Quit();
    }

    public enum PlayingState
    {
        Menu,
        Playing,
        Pause
    }
}
