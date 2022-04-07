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

    public Highscore BestHighscore => HList.Highscores[0];
    public int HighScoreScore => BestHighscore.Score;
    public string HighscorePlayerName => BestHighscore.Name;

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
        Load();
        LoadColors();
    }

    public void Save(int mPoints)
    {
        var saveData = new Highscore(PlayerName, mPoints);
        HList.Add(saveData);
        var json =  JsonUtility.ToJson(HList);
        print("json: " + json);
        File.WriteAllText(Application.persistentDataPath + "/highscorelist.json", json);
        print("Saved" + HList.Highscores.Count);
    }

    public void Load()
    {
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
    }

    [Serializable]
    public class Highscore : IComparable<Highscore>
    {
        [SerializeField] private int score;
        [SerializeField] private string name;
        public int Score => score;
        public string Name => name;

        public Highscore(string playerName, int score)
        {
            this.name = playerName;
            this.score = score;
        }

        public int CompareTo(Highscore other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Score.CompareTo(other.Score);
        }
        
        public override string ToString()
        {
            return $"{Name} - {Score}";
        }
        
       
    }

    [Serializable]
    private sealed class HighscoreList
    {
        [SerializeField] private List<Highscore> highscores;
        public List<Highscore> Highscores => highscores;    

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
            print("added "  + highscore);
        }
    }

    public static List<Highscore> GetHighscores => HList.Highscores;

    public void SaveHighscore(int mPoints)
    {
        Save(mPoints);
    }

    public Color[] Colors { get; private set; }

    private void LoadColors()
    {
        var path = Application.persistentDataPath + "/colordata.json";
        if (!File.Exists(path))
        {
           Colors = new Color[2];
           Colors[0] = Color.white;
           Colors[1] = Color.white;
        }
        else
        {
            var json = File.ReadAllText(path);
            ColorData data = JsonUtility.FromJson<ColorData>(json);
            Colors = data.Colors;
        }
        
    }

    public void SetColors(Color[] colors)
    {
        Colors = colors;
        ColorData data = new ColorData(Colors);
        var json =  JsonUtility.ToJson(data);
        print("json: " + json);
        File.WriteAllText(Application.persistentDataPath + "/colordata.json", json);
    }

    [Serializable]
    private sealed class ColorData
    {
        [SerializeField] private Color[] colors;

        public Color[] Colors
        {
            get => colors;
            set => colors = value;
        }

        public ColorData(Color[] colors)
        {
            this.colors = colors;
        }
        
    }
}