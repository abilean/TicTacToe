using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    #region private variables

    private Mark _curTurn;

    private List<Mark> _localPlayers;

    private int _playerCount =0;

    private Mark[,] _board;

    #endregion

    #region Public variables

    public Mark CurTurn
    {
        get { return _curTurn; }
        private set { _curTurn = value; }
    }

    #endregion

    #region events
    /// <summary>
    /// thrown when the turn changes, Mark is the new turns mark
    /// </summary>
    public Action<Mark> OnTurnChanged = delegate { };

    /// <summary>
    /// Thrown when a spot is legally selected, int = spot location, Mark is the mark to put there
    /// </summary>
    public Action<Coordinate, Mark> OnSpotSelected = delegate { };

    /// <summary>
    /// Thrown when the players turn changes, Bool is true when its a local player, otherwise false
    /// </summary>
    public Action<bool> OnLocalPlayerTurn = delegate { };

    #endregion


    #region public methods

    /// <summary>
    /// Checks to see if this placing this mark causes a win (3 in a row)
    /// </summary>
    /// <param name="board">the current board</param>
    /// <param name="newMark">the mark placed</param>
    /// <param name="loc">the location of the mark placed</param>
    /// <returns>true when 3 in a row (win)</returns>
    public static bool CheckWin(Mark[,] board, Mark newMark, Coordinate loc)
    {
        if (loc.x < 0 || loc.x > board.GetLength(0) || newMark == Mark.non ||
            loc.y < 0 || loc.y > board.GetLength(1) )
            return false;

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                //look for the first mark in a row
                if (loc.x + x >= 0 && loc.x + x < board.GetLength(0) &&
                    loc.y + y >= 0 && loc.y + y < board.GetLength(1) &&
                    board[loc.x + x, loc.y + y] == newMark)
                {
                    //look for second mark in the same direction
                    if (loc.x + (2 * x) >= 0 && loc.x + (2 * x) < board.GetLength(0) &&
                        loc.y + (2 * y) >= 0 && loc.y + (2 * y) < board.GetLength(1) &&
                        board[loc.x + (2 * x), loc.y + (2 * y)] == newMark)
                    {
                        return true;
                    }

                    //look for second mark in the oposite direction
                    if (loc.x - x >= 0 && loc.x - x < board.GetLength(0) &&
                        loc.y - y >= 0 && loc.y - y < board.GetLength(1) && 
                        board[loc.x - x, loc.y - y] == newMark)
                    {
                        return true;
                    }
                }
            }
        }


        return false;
    }

    /// <summary>
    /// Looks at the board and checks if there are any available spaces left to play
    /// </summary>
    /// <param name="board">The board to exame</param>
    /// <returns>True if there are no spaces and its a cat game, else false</returns>
    public static bool CatGame(Mark[,] board)
    {

        for(int x = 0; x < board.GetLength(0); x++)
        {
            for(int y = 0; y < board.GetLength(1); y++)
            {
                if (board[x, y] == Mark.non)
                    return false;
            }
        }

        return true;
    }


    /// <summary>
    /// Tries to place the mark of the current player in the selected spot.
    /// </summary>
    /// <param name="spot">location on the board to place mark</param>
    /// <returns>true when the mark is placed, otherwise false</returns>
    public bool SelectSpot(Coordinate spot)
    {
        //make sure the spot is in bounds or the mark is wrong
        if (spot.x < 0 || spot.x >= _board.GetLength(0) || spot.y < 0 || spot.y >= _board.GetLength(1))
            return false;

        //check that the spot is empty
        if(_board[spot.x, spot.y] == Mark.non)
        {
            if(SetMark(spot, _curTurn))
            {
                if (BoardController.CheckWin(_board, _curTurn, spot))
                {
                    GameManager.PlayerWins(_curTurn);
                    if (OnLocalPlayerTurn != null)
                        OnLocalPlayerTurn(false);
                }
                else
                {
                    if (CatGame(_board))
                    {
                        GameManager.PlayerWins(Mark.non);
                        if (OnLocalPlayerTurn != null)
                            OnLocalPlayerTurn(false);
                    }
                    else
                    {
                        ChangeTurn();
                    }
                }
                return true;
            }
        }

        return false;
    }



    /// <summary>
    /// gives the player a mark to represent them.
    /// </summary>
    /// <param name="islocal">Tells the board if this is a local player</param>
    /// <returns>the players mark, if NON. there are too many players</returns>
    public Mark RegisterPlayer(bool islocal)
    {
        if (_playerCount == 0) {
            _playerCount++;
            if (islocal)
                _localPlayers.Add(Mark.X);
            return Mark.X;
        }
        else if (_playerCount == 1)
        {
            _playerCount++;
            if (islocal)
                _localPlayers.Add(Mark.O);
            return Mark.O;
        }
        return Mark.non;
    }

    #endregion

    #region private methods

    private void Awake()
    {
        _board = new Mark[3,3];
        _localPlayers = new List<Mark>();

        OnSpotSelected += HandleSpotSelected;
        GameManager.OnStartGame += HandleStartGame;

    }

    private void Start()
    {
        ResetBoard();


        StartCoroutine(DelayStart());

    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1);
        //X always goes first
        _curTurn = Mark.O;
        ChangeTurn();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefabX"></param>
    /// <param name="prefabO"></param>
    private void HandleStartGame(GameObject prefabX, GameObject prefabO)
    {
        Instantiate(prefabX);
        Instantiate(prefabO);
    }

    /// <summary>
    /// resets the board to have all Mark.non
    /// </summary>
    private void ResetBoard()
    {
        for(int x = 0; x < _board.GetLength(0); x++)
        {
            for(int y = 0; y < _board.GetLength(1); y++)
            SetMark(new Coordinate(x,y), Mark.non);
        }
    }

    /// <summary>
    /// Sets the mark at the location on the board, and sends out the event
    /// </summary>
    /// <param name="loc">The location of the mark, cannot be less than 0</param>
    /// <param name="mark">The mark to place</param>
    /// <returns>true if mark location is allowed and placed, otherwise false</returns>
    private bool SetMark(Coordinate loc, Mark mark)
    {
        if (loc.x < 0 || loc.x >= _board.GetLength(0) || loc.y < 0 || loc.y > _board.GetLength(1))
            return false;
        _board[loc.x,loc.y] = mark;
        if (OnSpotSelected != null)
        {
            OnSpotSelected(loc, mark);
        }
        return true;
    }

    /// <summary>
    /// Changes the turn, throws OnTurnChanged and OnLocalPlayerTurn events
    /// </summary>
    private void ChangeTurn()
    {
        bool isLocalPlayer = false;

        if (_curTurn == Mark.X)
        {
            _curTurn = Mark.O;
            if (_localPlayers.Contains(Mark.O))
                isLocalPlayer = true;
            else
                isLocalPlayer = false;
        }
        else
        {
            _curTurn = Mark.X;
            if (_localPlayers.Contains(Mark.X))
                isLocalPlayer = true;
            else
                isLocalPlayer = false;
        }

        if(OnTurnChanged != null)
        {
            OnTurnChanged(_curTurn);
        }

        //tells all the controls if it's a local player
        if (OnLocalPlayerTurn != null)
            OnLocalPlayerTurn(isLocalPlayer);
    }

    private void HandleSpotSelected(Coordinate loc, Mark newMark)
    {
        
    }

    private void OnDestroy()
    {
        GameManager.OnStartGame -= HandleStartGame;
    }

    #endregion

}


public enum Mark { non, X, O }

[Serializable]
public struct Coordinate
{
    public int x;
    public int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() == typeof(Coordinate))
        {
            if (((Coordinate)obj).x == x && ((Coordinate)obj).y == y)
                return true;
        }
        return false;
    }

    public static bool Equals(Coordinate obj1, Coordinate obj2)
    {
        if (obj1.x == obj2.x)
        {
            if (obj1.y == obj2.y)
                return true;
        }
        return false;
    }
}

