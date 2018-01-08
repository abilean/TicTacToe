using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : BasePlayer {

    public enum Brains { genious =1, adult=2, teen=3 ,  child=4 , dumb =5 }

    [SerializeField]
    protected State currentState;

    [SerializeField]
    public Brains AiBrains;


    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public Mark[,] board;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public Mark curTurn;
    [HideInInspector] public bool playedMove = false;
    [HideInInspector] public bool aiActive;

    
    void Start()
    {
        board = new Mark[3, 3];
        Initialize();
        aiActive = true;
    }


    protected override Mark RegisterWithBoard()
    {
        return _boardControl.RegisterPlayer(false);
    }

    protected override void HandlePlayerWins(Mark player)
    {
        gameOver = true;
    }

    protected override void HandleTurnChange(Mark curMark)
    {
        curTurn = curMark;
        base.HandleTurnChange(curMark);
    }

    protected override void HandleSpotSelected(Coordinate spot, Mark curMark)
    {
        board[spot.x, spot.y] = curMark;
    }

    public void ChooseMove(Coordinate spot)
    {
        _boardControl.SelectSpot(spot);
        playedMove = true;
    }

    public void SetupAI(bool aiActivation)
    {
        
        aiActive = aiActivation;
        if (aiActive)
        {
            
        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update () {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    void OnDrawGizmos()
    {
        //if (currentState != null && eyes != null)
        //{
        //    Gizmos.color = currentState.sceneGizmoColor;
        //    Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
        //}
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != null)
        {
            
            OnExitState();
            currentState = nextState;
            currentState.InitializeState(this);
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    protected virtual void OnExitState()
    {
        currentState.ExitState(this);
        stateTimeElapsed = 0;
        
    }


}
