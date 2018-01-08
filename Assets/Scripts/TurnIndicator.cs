using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TurnIndicator : MonoBehaviour {

    private BoardController _boardControl;
    
    private Text _text;

	// Use this for initialization
	void Awake () {
        _text = this.GetComponent<Text>();

        if(_boardControl == null)
        {
            _boardControl = FindObjectOfType<BoardController>() as BoardController;
        }

        _boardControl.OnTurnChanged += HandleChangeTurn;
        GameManager.OnPlayerWins += HandlePlayerWin;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void HandleChangeTurn(Mark curMark)
    {
        _text.text = curMark.ToString() + "'s Turn";
    }

    private void HandlePlayerWin(Mark player)
    {
        if (player == Mark.non)
        {
            _text.text = "Cat game! No one wins.";
        }
        else
        {
            _text.text = string.Format("Player {0} is the winner!", player.ToString());
        }
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerWins -= HandlePlayerWin;
    }
}
