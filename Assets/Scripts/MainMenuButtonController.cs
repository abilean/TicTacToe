using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuButtonController : MonoBehaviour {



    [SerializeField]
    public GameObject MenuButton;

	// Use this for initialization
	void Start () {
        GameManager.OnPlayerWins += HandlePlayerWins;
	}

    private void HandlePlayerWins(Mark player)
    {
        if(MenuButton != null)
        {
            MenuButton.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerWins -= HandlePlayerWins;
    }

    public void ReturnToMainMenu()
    {
        GameManager.ToMainMenu();
    }
}

