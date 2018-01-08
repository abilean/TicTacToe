using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "AI/Action/ChooseMoveAction")]
public class ChooseMoveAction : Action
{

    public override void Act(StateController controller)
    {
        Dictionary<Coordinate, float> positions = new Dictionary<Coordinate, float>();

        for (int x = 0; x < controller.board.GetLength(0); x++)
        {
            for (int y = 0; y < controller.board.GetLength(0); y++)
            {
                if(controller.board[x,y] == Mark.non)
                {
                    Coordinate tempCoor = new Coordinate(x, y);
                    positions.Add(tempCoor,
                        Calc(controller.board, controller.MyMark, tempCoor, controller.MyMark, 0));
                }
            }
        }

        if (positions.Count == 0)
            controller.gameOver = true;

        int totalPossibilities = (int)controller.AiBrains;


        var choices = positions.OrderByDescending(l => l.Value).Select(l => l.Key).ToList<Coordinate>();
        if(choices.Count > 0)
        {
            int chosen = (int)Mathf.Round( Random.Range(0,
                                            Mathf.Min(totalPossibilities, choices.Count)));
            controller.ChooseMove( choices[chosen]);
            return;
        }

        Debug.Log("there is something wrong in the AI. went through all the options and didn't choose one");
    }

    private int Calc(Mark[,] board, Mark curMark, Coordinate pos, Mark myMark, int depth)
    {
        //check for winning move
        int winValue = 0;
        
        if (BoardController.CheckWin(board, curMark, pos))
        {
            if (myMark == curMark)
                winValue = 10;
            else
                winValue = -10;

            return winValue;
        }
            

        //make a copy of the board to manipulate
        Mark[,] myboard = Clone(board);
        //place my mark
        myboard[pos.x, pos.y] = curMark;

        
        Mark nextMark;

        if (curMark == Mark.X)
            nextMark = Mark.O;
        else
            nextMark = Mark.X;

        for(int x = 0; x < myboard.GetLength(0); x++)
        {
            for(int y = 0; y < myboard.GetLength(0); y++)
            {
                if(myboard[x,y] == Mark.non)
                {
                    winValue += Calc(myboard, nextMark, new Coordinate(x, y), myMark, depth++ );
                }
            }
        }

        return winValue;
    }

    private Mark[,] Clone(Mark[,] oldBoard)
    {
        Mark[,] board = new Mark[oldBoard.GetLength(0), oldBoard.GetLength(1)];

        for(int x = 0; x < oldBoard.GetLength(0); x++)
        {
            for(int y = 0; y < oldBoard.GetLength(1); y++)
            {
                board[x, y] = oldBoard[x, y];
            }
        }
        return board;
    }
}
