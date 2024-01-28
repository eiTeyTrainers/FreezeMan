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
    private int shapesPerLine = 16; // Number of shapes per line
    private float lineOffset = 0.0f;

    void Awake()
    {
        gameMode = FindObjectOfType<gameMode>();
        gameMode.stopTimer = true;
        timeText = GameObject.Find("time").GetComponent<TextMeshProUGUI>();

        float minutes = Mathf.FloorToInt(gameMode.time / 60);
        float seconds = Mathf.FloorToInt(gameMode.time % 60);
        float milliseconds = Mathf.FloorToInt((gameMode.time * 1000) % 1000);
        UpdateTimeText(minutes, seconds, milliseconds);
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        killCountText = GameObject.Find("KillCount").GetComponent<TextMeshProUGUI>();
        Vector3 shapeSpawnPoint = GameObject.Find("ShapeSpawnPoint").GetComponent<RectTransform>().pivot;
        float killCount = gameMode._freezeCounter + 1;
        string timeString = string.Format("You won, but look what you lost: {0:00}", killCount);
        Vector3 initialPosition = new Vector3(0, 0, 0); // Adjust the initial position as needed
        Vector3 position = initialPosition;

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
                currentShape.transform.SetParent(GameObject.Find("Viewport").transform, false);

                // Adjust the pivot along the x-axis
                RectTransform rectTransform = currentShape.GetComponent<RectTransform>();
                Vector2 newPivot = new Vector2(-1.74f + nextShapeOffset, 1.3f - lineOffset); // Adjust the x-axis and y-axis values as needed
                rectTransform.pivot = newPivot;
                nextShapeOffset -= screenSize.x * 0.002f;

                if ((i + 1) % shapesPerLine == 0)
                {
                    lineOffset -= screenSize.y * 0.003f; // Increase Y offset for a new line
                    nextShapeOffset = 0.0f; // Reset X offset for the next line
                }
            }
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
