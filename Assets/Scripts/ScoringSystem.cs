using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class ScoringSystem : MonoBehaviour
{
    [Serializable]
    public class ScoreData
    {
        public string name = "";
        public int score = 0;
    }

    private Player m_player = null;
    private LevelGenerator m_generator = null;

    [SerializeField] private Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        m_player = FindObjectOfType<Player>();
        m_generator = FindObjectOfType<LevelGenerator>();
        if (text)
            text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
