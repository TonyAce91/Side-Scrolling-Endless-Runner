using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Text gameOverText = null;
    [SerializeField] private Text endMessageText = null;
    private string gameOverString = "GAME OVER";
    [SerializeField] private float duration = 1f;
    private float timer = 0;
    private int stringNumber = 0;
    private int m_coins = 0;
    [SerializeField] private LevelGenerator m_generator = null;
    [SerializeField] private Player m_player = null;

    private void OnEnable()
    {
        // Clears text messages and reset string counter
        gameOverText.text = "";
        endMessageText.text = "";
        stringNumber = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = duration;
        m_generator = FindObjectOfType<LevelGenerator>();
    }

    // Update is called once per frame
    void Update()
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
}
