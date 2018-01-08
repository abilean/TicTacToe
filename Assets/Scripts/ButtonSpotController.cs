using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Text))]
public class ButtonSpotController : MonoBehaviour {

    [SerializeField]
    private BoardController _boardControl;

    [SerializeField]
    [Tooltip("The location of this spot on the board ( 0-8)")]
    private Coordinate _locaction;

    private Button _button;
    private Text _text;

    private Mark _myMark;

	// Use this for initialization
	void Start () {
        if(_locaction.x <0 || _locaction.x > 2 || _locaction.y < 0 || _locaction.y > 2)
        {
            throw new UnityException("Location is invalid in spot " + this.gameObject.name);
        }
        _button = this.GetComponent<Button>();
        _text = this.GetComponent<Text>();
        _myMark = Mark.non;

        _text.text = "";

        if (_boardControl == null)
        {
            _boardControl = FindObjectOfType<BoardController>() as BoardController;
        }

        _boardControl.OnLocalPlayerTurn += HandleLocalPlayerTurn;
        _boardControl.OnSpotSelected += HandleSpotSelected;


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleClick()
    {
        if (_boardControl.SelectSpot(_locaction))
        {
            //TODO: maybe change background to green?
        }
        else
        {
            //TODO: display to the user that this is an invalid click
        }
    }

    /// <summary>
    /// Disables the button when the not the local players turn 
    /// </summary>
    /// <param name="local"></param>
    private void HandleLocalPlayerTurn(bool local)
    {
        if(_myMark == Mark.non)
            _button.enabled = local;
    }
    
    /// <summary>
    /// Places the mark if it's for the spot
    /// </summary>
    /// <param name="loc">location to place</param>
    /// <param name="mark">mark to be placed</param>
    private void HandleSpotSelected(Coordinate loc, Mark mark)
    {
        if(loc.Equals(_locaction))
        {
            if(_myMark != Mark.non)
            {
                Debug.LogError("The same spot got chosen twice. loc = " + loc + " , spot = " + this.name);
            }

            _myMark = mark;
            _text.text = _myMark.ToString();
            _button.enabled = false;
        }

    }

    private void OnDestroy()
    {
        if(_boardControl != null)
        {
            _boardControl.OnLocalPlayerTurn -= HandleLocalPlayerTurn;
            _boardControl.OnSpotSelected -= HandleSpotSelected;
        }
    }
}


