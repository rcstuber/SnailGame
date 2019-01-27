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

    public int numScores {
        get => scoreRows.Length;
    }

    private List<Highscore> highscoreList = new List<Highscore>();

    static string prefsKey = "highscores";


    void Start()
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
                scoreRows[i].text = highscoreList[i].name + " -- " + score.ToString("0.0") + " m";
            } else {
                scoreRows[i].text = "--";
            }
        }
    }

    public bool AddHighscore(float newScore)
    {
        if(newScore < 0.1)
            return false;

        var highscore = new Highscore();
        highscore.name = "Player";
        highscore.score = newScore;

        int newPlace = highscoreList.FindIndex( entry => entry.score < newScore);
        if(newPlace < 0)
        {
            if(highscoreList.Count >= numScores)
                return false; // not a highscore

            highscoreList.Add(highscore);
        } else {
            highscoreList.Insert(newPlace, highscore);
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
