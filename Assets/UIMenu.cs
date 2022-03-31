using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private void Start()
    {
        scoreText.text = "Score: " + PersistenceManager.Instance.HighscorePlayerName + 
                                 ": " + PersistenceManager.Instance.HighScore;
    }
    
    public void NewGame()
    {
        PersistenceManager.Instance.PlayerName = inputField.text;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}