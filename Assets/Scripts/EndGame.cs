using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Text gameOverText = null;
    [SerializeField] private Text highscoreText = null;
    [SerializeField] private Text endMessageText = null;
    private string gameOverString = "GAME OVER";
    [SerializeField] private float duration = 1f;
    private float timer = 0;
    private int stringNumber = 0;
    private int m_coins = 0;
    private int m_score = 0;
    [SerializeField] private LevelGenerator m_generator = null;
    //[SerializeField] private Player m_player = null;
    private bool m_newHighscore = false;

    private void OnEnable()
    {
        // Clears text messages and reset string counter
        gameOverText.text = "";
        endMessageText.text = "";
        highscoreText.text = "";
        stringNumber = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 10;
        m_generator = FindObjectOfType<LevelGenerator>();
        NullChecker();
        m_newHighscore = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_newHighscore)
            HighscoreInput();
        else
            GameOverMessage();
    }

    private void HighscoreInput()
    {
        highscoreText.text = "NEW HIGHSCORE";
        int distance = (int) m_generator.distance;
        endMessageText.text = distance +"m x 1 = " + distance + "\n";
        endMessageText.text += m_coins + "coins x 50 = " + m_coins * 50 + "\n";
        m_score = distance + m_coins * 50;
        endMessageText.text += "Total score: " + m_score;
        if (timer > 0)
            timer -= Time.unscaledDeltaTime;
        else
        {
            m_newHighscore = false;
            timer = duration;
            highscoreText.text = "";
        }
    }

    private void GameOverMessage()
    {
        if (timer > 0)
            timer -= Time.unscaledDeltaTime;
        else
        {
            // Adds an effect to the game over message by typing one by one
            if (stringNumber < gameOverString.Length)
            {
                gameOverText.text += gameOverString[stringNumber];
                stringNumber++;
                timer = duration;
            }
        }
        // Once the game over message has been typed, adds the details of the player's achievement
        if (stringNumber >= gameOverString.Length && m_generator && endMessageText)
        {
            endMessageText.text = "You Travelled " + m_generator.distance.ToString("0") + "m and collected " + m_coins + " coins";
        }
    }

    // Used by player script to send coin information
    public void CollectedCoins(int amount)
    {
        m_coins = amount;
    }


    // This is used to check for any null reference that could break the game
    private void NullChecker()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            bool errorOccurred = false;

            // Checks if any of the texts needed for this script are not set
            if (gameOverText == null)
            {   
                EditorUtility.DisplayDialog("Error", "Game Over Text has not been set on End Game", "Exit");
                errorOccurred = true;
            }
            if (highscoreText == null)
            {   
                EditorUtility.DisplayDialog("Error", "Highscore Text has not been set on End Game", "Exit");
                errorOccurred = true;
            }
            if (endMessageText == null)
            {
                EditorUtility.DisplayDialog("Error", "End Message Text has not been set on End Game", "Exit");
                errorOccurred = true;
            }

            // Turns off the application if any error occurs
            if (errorOccurred)
                EditorApplication.isPlaying = false;
        }
#endif
    }


}
