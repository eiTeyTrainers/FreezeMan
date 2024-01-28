using System.Collections.Generic;
using UnityEngine;

public class MagazineOfShapes : MonoBehaviour
{
    private GameObject panelObject;
    public enum Shapes
    {
        Circle,
        Triangle,
        Box,
        Oval,
        Rectangle,
        TrianglePortal,
    }
    public Shapes[] shapes;
    private List<GameObject> shapeGameObjects;
    private float NextShapeOffsetPercentage = 0.11f; // Percentage of the screen width for the next shape offset
    private const float OffsetBetweenShapes = 0.06f;
    private const float OffsetAfterBrackets = 0.12f;  
    private const float SecondBracketOffset = 0.02f;
    private const float DestroyedShapeOffset = 0.12f;
    private const float ShapeOffsetIncrement = 0.023f;
    [SerializeField] private GameObject bracketPrefab;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private GameObject playerCirclePrefab;
    [SerializeField] private GameObject trianglePrefab;
    [SerializeField] private GameObject playerTrianglePrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject playerBoxPrefab;
    [SerializeField] private GameObject ovalPrefab;
    [SerializeField] private GameObject playerOvalPrefab;
    [SerializeField] private GameObject rectanglePrefab;
    [SerializeField] private GameObject playerRectanglePrefab;
    [SerializeField] private GameObject trianglePortalPrefab;
    [SerializeField] private GameObject playerTrianglePortalPrefab;
    
    
    
    private List<GameObject> PlayershapeGameObjects;
    private GameObject _playerStart;
    private int sortlayer;

    private void Awake()
    {
        panelObject = GameObject.Find("MagPanel");
        shapeGameObjects = new List<GameObject>();
        PlayershapeGameObjects = new List<GameObject>();
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        float nextShapeOffset = screenSize.x * NextShapeOffsetPercentage;

        GameObject firstBrackets = Instantiate(bracketPrefab, screenPosition + new Vector3(nextShapeOffset, -150, 0), Quaternion.identity);
        firstBrackets.transform.SetParent(panelObject.transform, false);
        firstBrackets.transform.localScale = new Vector3(1f, 1f, 1f);
        nextShapeOffset += screenSize.x * OffsetAfterBrackets; // Adjust this factor based on your preference
        
        for (int i = 0; i < shapes.Length; i++)
        {
            Vector3 position = screenPosition + new Vector3(nextShapeOffset, -150, 0);

            GameObject currentShape;
            switch (shapes[i])
            {
                case Shapes.Circle:
                    currentShape = Instantiate(circlePrefab, position, Quaternion.identity);
                    PlayershapeGameObjects.Add(playerCirclePrefab);
                    break;
                case Shapes.Triangle:
                    currentShape = Instantiate(trianglePrefab, position, Quaternion.identity);
                    PlayershapeGameObjects.Add(playerTrianglePrefab);
                    break;
                case Shapes.Box:
                    currentShape = Instantiate(boxPrefab, position, Quaternion.identity);
                    PlayershapeGameObjects.Add(playerBoxPrefab);
                    break;
                case Shapes.Oval:
                    currentShape = Instantiate(ovalPrefab, position, Quaternion.identity);
                    PlayershapeGameObjects.Add(playerOvalPrefab);
                    break;
                case Shapes.Rectangle:
                    currentShape = Instantiate(rectanglePrefab, position, Quaternion.identity);
                    PlayershapeGameObjects.Add(playerRectanglePrefab);
                    break;
                case Shapes.TrianglePortal:
                    currentShape = Instantiate(trianglePortalPrefab, position, Quaternion.identity);
                    PlayershapeGameObjects.Add(playerTrianglePortalPrefab);
                    break;
                default:
                    currentShape = null;
                    break;
            }
            if (currentShape != null)
            {
                currentShape.transform.SetParent(panelObject.transform, false);
                currentShape.transform.localScale = new Vector3(1f, 1f, 1f);
                shapeGameObjects.Add(currentShape);
            }
            
            nextShapeOffset += screenSize.x * (i != 0? OffsetBetweenShapes: 0.12f); // Adjust this factor based on your preference
        }

        nextShapeOffset += screenSize.x * SecondBracketOffset; // Adjust this factor based on your preference
        Vector3 secondBracketsPosition = screenPosition + new Vector3(nextShapeOffset, -150, 0);
        GameObject secondBrackets = Instantiate(bracketPrefab, secondBracketsPosition, Quaternion.Euler(0, 0, 180f));
        secondBrackets.transform.SetParent(panelObject.transform, false);
        secondBrackets.transform.localScale = new Vector3(1f, 1f, 1f);
        shapeGameObjects.Add(secondBrackets);

        _playerStart = GameObject.Find("PlayerStart");
        Instantiate(resetUI(), _playerStart.transform.position, Quaternion.identity);
    }

    public GameObject resetUI()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        if (shapeGameObjects.Count > 1)
        {
            addToFreezeGameObjectList(shapes[0]);
            DestroyFirstShape();
            Destroy(shapeGameObjects[0]);
            shapeGameObjects.RemoveAt(0);

            GameObject toBeSpawnedShape = PlayershapeGameObjects[0]; 
            PlayershapeGameObjects.RemoveAt(0);

            float firstShapeOffset = screenSize.x * 0.1f;
            Transform firstShapeTransform = shapeGameObjects[0].transform;
            Vector3 firstShapePosition = firstShapeTransform.position;
            shapeGameObjects[0].transform.SetPositionAndRotation(new Vector3(firstShapePosition.x - firstShapeOffset, firstShapePosition.y, firstShapePosition.z), firstShapeTransform.rotation);

            // // Increment by 0.06f for subsequent shapes
            for (int i = 1; i < shapeGameObjects.Count; i++)
            {
                float offset = screenSize.x * 0.0385f;
                Transform shapeTransform = shapeGameObjects[i].transform;
                Vector3 shapePosition = shapeTransform.position;
                shapeGameObjects[i].transform.SetPositionAndRotation(new Vector3(shapePosition.x - offset, shapePosition.y, shapePosition.z), shapeTransform.rotation);
            }

            return toBeSpawnedShape;
        }
        else
        {
            return null;
            // reset;
        }
    }

    private void DestroyFirstShape()
    {
        if (shapes.Length > 0)
        {
            // Remove the first shape from the array
            List<Shapes> tempList = new List<Shapes>(shapes);
            tempList.RemoveAt(0);
            shapes = tempList.ToArray();
        }
    }

    private static void addToFreezeGameObjectList(Shapes playerShape)
    {
        // Assuming gameMode is a MonoBehaviour class in your project
        gameMode gameMode = FindObjectOfType<gameMode>();

        if (gameMode != null)
        {
            gameMode.AddPlayerShape(playerShape);
        }
    }
}
