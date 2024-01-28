using System.Collections;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI killCountText;
    private gameMode gameMode;
    public GameObject frozenCircle;
    public GameObject frozenTriangle;
    public GameObject frozenBox;
    public GameObject frozenOval;
    public GameObject frozenRectangle;
    public GameObject frozenTrianglePortal;
    private float nextShapeOffset;

    void Awake()
    {
        gameMode = FindObjectOfType<gameMode>();
        gameMode.stopTimer = true;
        timeText = GameObject.Find("time").GetComponent<TextMeshProUGUI>();

        float minutes = Mathf.FloorToInt(gameMode.time / 60);
        float seconds = Mathf.FloorToInt(gameMode.time % 60);
        float milliseconds = Mathf.FloorToInt((gameMode.time * 1000) % 1000);
        UpdateTimeText(minutes, seconds, milliseconds);

        Vector2 screenPosition = transform.position;
        killCountText = GameObject.Find("KillCount").GetComponent<TextMeshProUGUI>();
        float killCount = gameMode._freezeCounter + 1;
        string timeString = string.Format("You won, but look what you lost: {0:00}", killCount);

        Vector3 position = new Vector3(screenPosition.x + nextShapeOffset, -150, 0);
        killCountText.text = timeString;

        for (int i = 0; i < gameMode.shapes.Count; i++)
        {
            GameObject currentShape;
            switch (gameMode.shapes[i])
            {
                case MagazineOfShapes.Shapes.Circle:
                    currentShape = Instantiate(frozenCircle, position, Quaternion.identity);
                    break;
                case MagazineOfShapes.Shapes.Triangle:
                    currentShape = Instantiate(frozenTriangle, position, Quaternion.identity);
                    break;
                case MagazineOfShapes.Shapes.Box:
                    currentShape = Instantiate(frozenBox, position, Quaternion.identity);
                    break;
                case MagazineOfShapes.Shapes.Oval:
                    currentShape = Instantiate(frozenOval, position, Quaternion.identity);
                    break;
                case MagazineOfShapes.Shapes.Rectangle:
                    currentShape = Instantiate(frozenRectangle, position, Quaternion.identity);
                    break;
                case MagazineOfShapes.Shapes.TrianglePortal:
                    currentShape = Instantiate(frozenTrianglePortal, position, Quaternion.identity);
                    break;
                default:
                    currentShape = null;
                    break;
            }
            if (currentShape != null)
            {
                currentShape.transform.SetParent(transform, false);
            }
            position.x += 100; // Increase X position for the next shape
        }
    }

    void UpdateTimeText(float minutes, float seconds, float milliseconds)
    {
        // Format the minutes, seconds, and milliseconds into a single string
        string timeString = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);

        // Update the TextMeshProUGUI component
        timeText.text = timeString;
    }
}