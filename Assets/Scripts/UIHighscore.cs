using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHighscore : MonoBehaviour
{
    [SerializeField] private GameObject highscorePanel;
    [SerializeField] private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        print("Start Highscore");
        int counter = 0;
        List<PersistenceManager.Highscore> list = PersistenceManager.GetHighscores();
        foreach (PersistenceManager.Highscore highscore in list)
        {
            TextMeshProUGUI panel = Instantiate(highscorePanel, canvas.transform).GetComponent<TextMeshProUGUI>();
            RectTransform rectTransform = panel.GetComponent<RectTransform>();
            panel.SetText($"{highscore.Score} \t - \t {highscore.Name}");
            rectTransform.localPosition = new Vector3(0,-80 * counter++,0);
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
