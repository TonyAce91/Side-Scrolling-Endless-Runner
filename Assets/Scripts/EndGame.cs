using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Text gameOverText = null;
    private string gameOverString = "GAME OVER";
    [SerializeField] private float duration = 1f;
    private float timer = 0;
    private int stringNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        timer = duration;
        gameOverText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            if (stringNumber < gameOverString.Length)
            {
                gameOverText.text += gameOverString[stringNumber];
                stringNumber++;
                timer = duration;
            }
        }
    }
}
