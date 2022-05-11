using System;
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

    float t = 0f;
    float _score = 0f;
    // Start is called before the first frame update
    void Start()
    {
        state = PlayingState.Menu;
        menu.transform.position = pause.transform.position;

        time.text = "00:00"; 
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

    public enum PlayingState
    {
        Menu,
        Playing,
        Pause
    }
}
