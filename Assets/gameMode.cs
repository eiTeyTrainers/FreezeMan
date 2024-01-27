using System;
using UnityEngine;

public class gameMode : MonoBehaviour
{
    public event Action<float> FreezeCounterChanged; // Event to notify changes in freezeCounter
    public  int _freezeCounter = 0;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int FreezeCounter
    {
        get { return _freezeCounter; }
        set
        {
            if (_freezeCounter != value)
            {
                _freezeCounter = value;
                OnFreezeCounterChanged();
            }
        }
    }


    private void OnFreezeCounterChanged()
    {
        FreezeCounterChanged?.Invoke(_freezeCounter);
    }
}