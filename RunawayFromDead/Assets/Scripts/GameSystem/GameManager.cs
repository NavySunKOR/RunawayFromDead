using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public string[] keyOwned;
    public bool IsGameCleared
    {
        get
        {
            return isGameCleared;
        }

        set
        {
            isGameCleared = value;
        }
    }

    private bool isGameCleared;
    private int idx;

    private void Start()
    {
        isGameCleared = false;
    }

    public void AddKey(string key)
    {
        keyOwned[idx] = key;
        idx++;
    }

    public bool HasKey(string key)
    {
        foreach(string name in keyOwned)
        {
            if(name.Equals(key))
            {
                return true;
            }
        }

        return false;
    }

    public void GameCleared()
    {
        isGameCleared = true;
    }
}
