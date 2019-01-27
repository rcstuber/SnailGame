using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Highscore {
    public string name;
    public float score;
}

public class HighscoreManager : MonoBehaviour
{
    [Serializable]
    public class HighscoreStore {
        public List<Highscore> scores;
    }

    public Text[] scoreRows;

    public Color highlightColor;


    public int numScores {
        get => scoreRows.Length;
    }

    private List<Highscore> highscoreList = new List<Highscore>();

    private int latestPlace = -1;

    static string prefsKey = "highscores";


    void Awake()
    {
        string json = PlayerPrefs.GetString(prefsKey);
        if(null != json)
        {
            var store = JsonUtility.FromJson<HighscoreStore>(json);
            if(null != store)
            {
                highscoreList = store.scores;
            }
        }

        UpdateList();
    }

    void UpdateList()
    {
        for(int i=0; i< scoreRows.Length; i++)
        {
            if(i < highscoreList.Count) {
                float score = highscoreList[i].score;
                scoreRows[i].text = score.ToString("N2") + " m";

                scoreRows[i].color = (i == latestPlace) ? highlightColor : Color.white;
            } else {
                scoreRows[i].text = "--";
            }
        }
    }

    public bool AddHighscore(float newScore)
    {
        if(newScore < 0.1) {
            latestPlace = -1;
            return false;
        }

        var highscore = new Highscore();
        highscore.name = "Player";
        highscore.score = newScore;

        int newPlace = highscoreList.FindIndex( entry => entry.score < newScore);
        if(newPlace < 0)
        {
            if(highscoreList.Count >= numScores) {
                latestPlace = -1;
                return false; // not a highscore
            }

            highscoreList.Add(highscore);
            latestPlace = 0;
        } else {
            highscoreList.Insert(newPlace, highscore);
            latestPlace = newPlace;
        }

        if(highscoreList.Count > numScores)
            highscoreList.RemoveAt(highscoreList.Count - 1); // drop last score
        
        // Strore list
        var store = new HighscoreStore();
        store.scores = highscoreList;

        string jsonList = JsonUtility.ToJson(store);

        PlayerPrefs.SetString(prefsKey, jsonList);

        UpdateList();

        return true;
    }

    public void ClearHighscores()
    {
        latestPlace = -1;

        highscoreList.Clear();
        var store = new HighscoreStore();
        store.scores = highscoreList;
        string jsonList = JsonUtility.ToJson(store);

        PlayerPrefs.SetString(prefsKey, jsonList);

        UpdateList();
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
