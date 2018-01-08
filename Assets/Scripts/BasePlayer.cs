using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour {

    private Mark _myMark;

    protected BoardController _boardControl;

    public Mark MyMark
    {
        get { return _myMark; }
        protected set { _myMark = value; }
    }

	// Use this for initialization
	void Start () {
        Initialize();
	}

    protected void Initialize()
    {
        if (_boardControl == null)
        {
            _boardControl = FindObjectOfType<BoardController>() as BoardController;
        }

        _myMark = RegisterWithBoard();
        if (_myMark == Mark.non)
        {
            this.gameObject.SetActive(false);
        }

        GameManager.OnPlayerWins += HandlePlayerWins;
        _boardControl.OnTurnChanged += HandleTurnChange;
        _boardControl.OnSpotSelected += HandleSpotSelected;
    }

    /// <summary>
    /// Registers with the board, telling if it is a local player (defualt = true)
    /// </summary>
    /// <returns>The mark for this player</returns>
    protected virtual Mark RegisterWithBoard()
    {
        if (_myMark != Mark.non)
            return _myMark;

        return _boardControl.RegisterPlayer(true);
    }
	
    /// <summary>
    /// Disable anything that needs to be disabled when the game is over
    /// </summary>
    /// <param name="playerName"></param>
    protected virtual void HandlePlayerWins(Mark player)
    {

    }

    protected virtual void HandleTurnChange(Mark curMark)
    {

    }

    protected virtual void HandleSpotSelected(Coordinate spot, Mark curMark)
    {
        
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void OnDestroy()
    {
        GameManager.OnPlayerWins -= HandlePlayerWins;
    }
}
