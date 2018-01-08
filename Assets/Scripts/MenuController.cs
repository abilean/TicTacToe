using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject[] Prefabs;

    private int _prefabX = 0;
    private int _prefabO = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlayerX(Dropdown dd)
    {
        _prefabX = dd.value;
    }

    public void SetPlayerO(Dropdown dd)
    {
        _prefabO = dd.value ;
    }

    public void StartGame()
    {
        GameManager.StartGame(_prefabX < Prefabs.Length ? Prefabs[_prefabX] : null,
                               _prefabO < Prefabs.Length ? Prefabs[_prefabO] : null);
    }
}
