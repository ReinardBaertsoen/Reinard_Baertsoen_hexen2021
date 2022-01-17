using DAE.GameSystem.GameStates;
using DAE.StateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.GameSystem
{
    public class EndScreenState : GameStateBase
    {
        private static Canvas _startScreenState;
        private static Canvas _playingState;
        private static Canvas _endScreenState;
        public EndScreenState(StateMachine<GameStateBase> stateMachine, Canvas startScreenState, Canvas playingState, Canvas endScreenState) : base(stateMachine, startScreenState, playingState, endScreenState)
        {
            _startScreenState = startScreenState;
            _playingState = playingState;
            _endScreenState = endScreenState;
        }
        public override void OnEnter()
        {
            _startScreenState.gameObject.SetActive(false);
            _playingState.gameObject.SetActive(false);
            _endScreenState.gameObject.SetActive(true);
        }
    }
}