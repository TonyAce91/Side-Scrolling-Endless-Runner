using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    private bool paused = false;
    [SerializeField] private Text m_countdownText = null;
    [SerializeField] private Text m_instructionText = null;
    [SerializeField] private float m_pauseDuration = 3f;
    private float timer = 0;

    private void Start()
    {
        NullChecker();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks whether the game has been unpaused and show timer to player
        if (!paused && timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            m_countdownText.text = timer.ToString("0");
        }

        // Once timer turns out, plays the game
        if (!paused && timer <= 0)
        {
            Time.timeScale = 1;
            m_countdownText.text = "";
            m_instructionText.text = "";
            gameObject.SetActive(false);
        }

    }

    public void PauseGame()
    {
        paused = !paused;

        // Pauses the game and shows the player that the game has been paused
        if (paused)
        {
            m_countdownText.text = "PAUSED";
            Time.timeScale = 0;
        }
        else
        {
            // Sets pause timer the moment it has been unpaused
            timer = m_pauseDuration;
        }
    }

    // Used when the game starts, adds a timer before starting to let the player get ready
    // Adds instruction on how to play the game
    public void StartGame()
    {
        Time.timeScale = 0;
        paused = false;
        timer = m_pauseDuration;

#if (UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL || UNITY_EDITOR)
        m_instructionText.text = "Press space to Jump.You can also double Jump.";
#elif (UNITY_IOS || UNITY_ANDROID)
        m_instructionText.text = "Tap to Jump.You can also double Jump.";
#endif

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // This is used to check for any null reference that could break the game
    private void NullChecker()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            bool errorOccurred = false;

            // Checks if any of the texts needed for this script are not set
            if (m_countdownText == null)
            {
                EditorUtility.DisplayDialog("Error", "Countdown Text has not been set on Menu Manager", "Exit");
                errorOccurred = true;
            }
            if (m_instructionText == null)
            {
                EditorUtility.DisplayDialog("Error", "Instruction Text has not been set on Menu Manager", "Exit");
                errorOccurred = true;
            }

            // Turns off the application if any error occurs
            if (errorOccurred)
                EditorApplication.isPlaying = false;
        }
#endif
    }

}
