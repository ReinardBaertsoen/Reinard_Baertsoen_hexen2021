using DAE.BoardSystem;
using DAE.GameSystem.GameStates;
using DAE.HexSystem;
using DAE.StateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.GameSystem
{
    public class StartScreenState : GameStateBase
    {
        private static Canvas _startScreenState;
        private static Canvas _playingState;
        private static Canvas _endScreenState;
        public StartScreenState(StateMachine<GameStateBase> stateMachine, Canvas startScreenState, Canvas playingState, Canvas endScreenState) : base(stateMachine, startScreenState, playingState, endScreenState)
        {
            _startScreenState = startScreenState;
            _playingState = playingState;
            _endScreenState = endScreenState;
        }

        public override void OnEnter()
        {
            _startScreenState.gameObject.SetActive(true);
            _playingState.gameObject.SetActive(false);
            _endScreenState.gameObject.SetActive(false);
        }
    }
}