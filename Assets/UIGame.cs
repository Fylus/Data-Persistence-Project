using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private TextMeshPro highScoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PersistenceManager.Instance != null)
        {
            name  = PersistenceManager.Instance.PlayerName;
        }
    }
}
