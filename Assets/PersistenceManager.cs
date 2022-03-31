using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    //make singleton
    private static PersistenceManager _instance;

    public static PersistenceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("PersistenceManager").AddComponent<PersistenceManager>();
            }

            return _instance;
        }
        private set => _instance = value;
    }

    public int HighScore { get; private set; }
    public string HighscorePlayerName { get; private set; }

    public string PlayerName { get; set; }

    private static HighscoreList HList { get; set; }

    private void Awake()
    {
        print(Application.persistentDataPath);
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        PlayerName = "Unknown";
        DontDestroyOnLoad(gameObject);

        if (HList == null)
        {
            var path = Application.persistentDataPath + "/highscorelist.json";
            if (!File.Exists(path))
            {
                HList = new HighscoreList();
            }
            else
            {
                var json = File.ReadAllText(path);
                HList = JsonUtility.FromJson<HighscoreList>(json);
            }
        }

        Load();
    }

    public void Save()
    {
        var saveData = new Highscore(HighscorePlayerName, HighScore);
        HList.Add(saveData);
        var json = JsonUtility.ToJson(saveData);
        print("Data saved: " + json);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);


        json = JsonUtility.ToJson(HList);
        File.WriteAllText(Application.persistentDataPath + "/highscorelist.json", json);
    }

    public void Load()
    {
        var path = Application.persistentDataPath + "/savefile.json";
        print("load");
        if (!File.Exists(path)) return;
        var json = File.ReadAllText(path);
        var saveData = JsonUtility.FromJson<Highscore>(json);
        print("Data read from file: " + saveData.playerName + " " + saveData.score);
        HighScore = saveData.score;
        HighscorePlayerName = saveData.playerName;
        foreach (var hListHighscore in HList.highscores)
        {
            print(hListHighscore.playerName + " " + hListHighscore.score);
        }
    }

    [Serializable]
    private sealed class Highscore : IComparable<Highscore>
    {
        public int score;
        public string playerName;

        public Highscore(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }

        public int CompareTo(Highscore other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return score.CompareTo(other.score);
        }
    }

    [Serializable]
    private sealed class HighscoreList
    {
        public List<Highscore> highscores;

        public HighscoreList()
        {
            highscores = new List<Highscore>(6);
        }

        public void Add(Highscore highscore)
        {
            highscores.Add(highscore);
            highscores.Sort();
            highscores.Reverse();
            if (highscores.Count > 5)
            {
                highscores.RemoveAt(5);
            }
        }
    }

    public void SaveHighscore(int mPoints)
    {
        HighscorePlayerName = PlayerName;
        HighScore = mPoints;
        Save();
    }
}