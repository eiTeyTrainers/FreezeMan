using TMPro;
using UnityEngine;

public class TextTime : MonoBehaviour
{
    private TextMeshProUGUI timeText; 
    private gameMode gameMode;

    void Awake()
    {
        gameMode = FindObjectOfType<gameMode>();
    }

    void Update()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        float minutes = Mathf.FloorToInt(gameMode.time / 60);
        float seconds = Mathf.FloorToInt(gameMode.time % 60);
        float milliseconds = Mathf.FloorToInt((gameMode.time * 1000) % 1000);
        UpdateTimeText(minutes, seconds, milliseconds);
    }

    void UpdateTimeText(float minutes, float seconds, float milliseconds)
    {
        // Format the minutes, seconds, and milliseconds into a single string
        string timeString = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);

        // Update the TextMeshProUGUI component
        timeText.text = timeString;
    }
}