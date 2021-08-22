﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(AudioManager))]
public class GameController : MonoBehaviour
{
    public Text health;
    public Text domination;

    public GameObject block;
    public Canvas gameOverCanvas;
    public Canvas pause;

    private float fixedDeltaTime;

    void Awake()
    {
        gameOverCanvas.enabled = false;
        pause.enabled = false;
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Start()
    {
        // Receiive the scene
        // Appoint the music
        if(SceneManager.GetActiveScene().name == "Level_1")
        {
            GetComponent<AudioManager>().Play("korridor");
        }

        if(SceneManager.GetActiveScene().name == "Level_2_test")
        {
            GetComponent<AudioManager>().Play("bossfight_intro");
        }


        // Находит нужный текст, если есть
        if (health == null || domination == null)
        {
            try
            {
                health = GameObject.Find("Health").GetComponent<Text>();
                domination = GameObject.Find("Domination").GetComponent<Text>();
            }
            catch
            {
                // Выдает сообщение если нет
                Debug.Log("Не получается обнаружить Health и Domination");
            }
        }
    }
    void Update()
    {
        // Обновляет текстовые показатели
        domination.text = "Domination: " + GameData.current.domination;
        health.text = "Health: " + GameData.current.health;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pause.enabled == false)
            {
                Debug.Log("PAUSE");
                pause.enabled = true;
                StopTime();

            } else if (pause.enabled == true)
            {
                Debug.Log("RESUME");
                pause.enabled = false;
                StopTime();
            }

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameData.current.health = 0;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }

            // Уничтожает препятствие перед лестницей, когда игрок достиг 20 доминации
            if (GameData.current.domination >= 20)
        {
            // Пробует найти препятствие
            try
            {
                Destroy(GameObject.Find("block"));
            }
            catch
            {
                Debug.Log("Не могу найти преграду");
            }
        }

        // Смерть игрока
        if(GameData.current.health <= 0)
        {
            StopTime();
            gameOverCanvas.enabled = true;
            gameOverCanvas.GetComponentInChildren<Button>().onClick.AddListener(RestartScene);
            GameData.current.health = 1;
        }
    }

    void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void StopTime()
    {
        if (Time.timeScale == 1.0f)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1.0f;

        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
}
