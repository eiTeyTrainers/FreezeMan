using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineOfShapes : MonoBehaviour
{
    private GameObject canvasObject;
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
    private float  NextShapeOffset = 100;
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
        canvasObject =  GameObject.Find("MagCanvas");
        shapeGameObjects = new List<GameObject>(); 
        PlayershapeGameObjects = new List<GameObject>();
        Vector2 screenPosition = Camera.main.transform.position;
        Vector3 position = new Vector3(screenPosition.x + NextShapeOffset, -150, 0);
        GameObject firstBrackets = Instantiate(bracketPrefab, position, Quaternion.identity);
        firstBrackets.transform.SetParent(canvasObject.transform, false);
        firstBrackets.transform.localScale = new Vector3(1f,1f,1f);
        NextShapeOffset += 120;
        
        for (int i = 0; i < shapes.Length; i++)
        {
            position = new Vector3(screenPosition.x + NextShapeOffset, -150, 0);
            
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
                    currentShape = Instantiate(boxPrefab,position , Quaternion.identity);
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
                currentShape.transform.SetParent(canvasObject.transform, false);
                currentShape.transform.localScale = new Vector3(1f,1f,1f);
                shapeGameObjects.Add(currentShape);
                RectTransform shapeRectTransform = currentShape.GetComponent<RectTransform>();
                shapeRectTransform.SetSiblingIndex(sortlayer);
            }
             NextShapeOffset += 60;
            sortlayer += 1;
        }

        NextShapeOffset += 40;
        position = new Vector3(screenPosition.x + NextShapeOffset, -150, 0);
        GameObject SecondBrackets = Instantiate(bracketPrefab, position, Quaternion.Euler(0, 0, 180f));
        SecondBrackets.transform.SetParent(canvasObject.transform, false);
        SecondBrackets.transform.localScale = new Vector3(1f, 1f, 1f);
        shapeGameObjects.Add(SecondBrackets);
        
        _playerStart = GameObject.Find("PlayerStart");
        Instantiate(resetUI(), _playerStart.transform.position, Quaternion.identity);
    }
     public GameObject resetUI()
     {
         
         if (shapeGameObjects.Count > 1)
         {
             DestroyFirstShape();
             Destroy(shapeGameObjects[0]);
             shapeGameObjects.RemoveAt(0);
             GameObject toBeSpawnedShape =  PlayershapeGameObjects[0];
             PlayershapeGameObjects.RemoveAt(0);
             for (int i = 0; i < shapeGameObjects.Count; i++)
             {
                 int firstShapeOffset = i == 0 ? 100 : 60;  
                 Transform shapeTransform = shapeGameObjects[i].transform;
                 Vector3 shapePosition = shapeTransform.position;
                 shapeGameObjects[i].transform.SetPositionAndRotation(new Vector3(shapePosition.x - firstShapeOffset, shapePosition.y,shapePosition.z),shapeTransform.rotation); ;
             }
             return toBeSpawnedShape;
         }
         else
         {
             return null;
             //reset;
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
     
}