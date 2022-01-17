using DAE.HexSystem;
using DAE.StateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.GameSystem.GameStates
{
    public class GameStateBase : IState<GameStateBase>
    {
        public StateMachine<GameStateBase> StateMachine { get; }
        public Canvas StartScreenState { get; set; }
        public Canvas PlayingState { get; set; }
        public Canvas EndScreenState { get; set; }

        public GameStateBase(StateMachine<GameStateBase> stateMachine, Canvas startScreenState, Canvas playingState, Canvas endScreenState)
        {
            StateMachine = stateMachine;
            StartScreenState = startScreenState;
            PlayingState = playingState;
            EndScreenState = endScreenState;
        }

        public virtual void OnEnter()
        {
        }

        public void OnExit()
        {
            
        }
    }
}
