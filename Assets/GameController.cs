using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayingState state;

    public GameObject menu;
    public GameObject background;
    public GameObject pause;

    public Text time;
    public Text score;

    [SerializeField] private GameObject[] cubes;
    private int cubeNum;
    private int cubesSpawned;

    float t = 0f;
    float _score = 0f;
    // Start is called before the first frame update
    void Start()
    {
        state = PlayingState.Menu;
        menu.transform.position = pause.transform.position;

        time.text = "00:00";

        cubeNum = (int)Random.Range(10, 15);
        cubesSpawned = cubeNum;

        Debug.Log(cubeNum);

    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime();
    }

    private void DisplayTime()
    {
        if (state == PlayingState.Playing)
        {
            t += Time.deltaTime;
            float min = Mathf.FloorToInt(t / 60);
            float s = Mathf.FloorToInt(t % 60);
            time.text = string.Format("{0:00}:{1:00}", min, s);
        }   
    }

    public void TargetDestroyed()
    {
        _score += 10;
        score.text = _score.ToString();
    }

    public void Play()
    {
        if (state == PlayingState.Menu || cubesSpawned != 0)
        {
            StartCoroutine("SpawnTargets");
        }
        state = PlayingState.Playing;
        background.SetActive(false);
        pause.SetActive(false);
        menu.SetActive(false);
    }

    public void Pause()
    {
        if (state == PlayingState.Playing)
        {
            state = PlayingState.Pause;
            background.SetActive(true);
            pause.SetActive(true);

            StopCoroutine("SpawnTargets");
        }
    }
    
    public void Menu()
    {
        if (state == PlayingState.Playing)
        {
            state = PlayingState.Menu;
            menu.transform.position = pause.transform.position;
            menu.transform.rotation = pause.transform.rotation;
        }
    }

    public IEnumerator SpawnTargets()
    {
        if (cubesSpawned > 0)
        {
            cubesSpawned -= 1;

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
            float scale = Random.Range(1.0f, 2.0f);
            GameObject cube = Instantiate(cubes[Random.Range(0, cubes.Length - 1)], gameObject.transform);
            cube.transform.SetPositionAndRotation(pos, rot);
            cube.transform.localScale *= scale;
            yield return new WaitForSeconds(6f / cubeNum);

            StartCoroutine("SpawnTargets");
        }
        else
        {
            StopCoroutine("SpawnTargets");
            Debug.Log("ALL CUBES SPAWNED");
        }

    }

    public enum PlayingState
    {
        Menu,
        Playing,
        Pause
    }
}
