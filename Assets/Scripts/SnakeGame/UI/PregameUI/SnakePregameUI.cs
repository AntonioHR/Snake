using JammerTools.StateMachines;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SnakeGame
{
    public partial class SnakePregameUI : MonoBehaviour
    {
        public TextMeshProUGUI[] keyTexts;
        public SnakeTypeDisplay typeDisplay;

        private List<PlayerActor.Setup> players = new List<PlayerActor.Setup>();
        private SnakeActor.Setup baseSnake => baseSetup.snakeSetup;

        private StateMachine stateMachine;

        private GameConfigs gameConfigs;
        private Action<PlayerActor.Setup[]> FinishCallback;
        private KeyCode pressedKey;

        private PlayerActor.Setup baseSetup => gameConfigs.basePlayerSetup;
        private SnakeTypeAsset[] types => gameConfigs.snakeTypes;
        private int maxPlayers => gameConfigs.maxPlayers;

        private void Awake()
        {
            typeDisplay.gameObject.SetActive(false);
            stateMachine = new StateMachine();
            stateMachine.Begin(this);
        }
        private void Update()
        {
            stateMachine.CurrentState.Update();
        }
        private void OnGUI()
        {
            Event e = Event.current;
            if (e != null && e.isKey)
            {
                pressedKey = e.keyCode;
            }
            else
            {
                pressedKey = KeyCode.None;
            }
        }
        public void BeginSelection(GameConfigs gameConfigs, Action<PlayerActor.Setup[]> OnResult)
        {
            this.gameConfigs = gameConfigs;
            FinishCallback = OnResult;
            stateMachine.CurrentState.BeginSelection();
        }


        private PlayerActor.Setup BuildPlayer(SnakeTypeAsset currentType, List<KeyCode> chosenKeyCodes)
        {
            var snakeSetup = new SnakeActor.Setup()
            {
                color = gameConfigs.playerColors[players.Count],
                configsAsset = baseSetup.snakeSetup.configsAsset,
                startDirection = baseSnake.startDirection,
                startPosition = baseSnake.startPosition + gameConfigs.playerSpacing * players.Count,
                snakeTypeAsset = currentType
            };

            return new PlayerActor.Setup()
            {
                leftKey = chosenKeyCodes[0],
                rightKey = chosenKeyCodes[1],
                snakeSetup = snakeSetup
            };
        }


        private class StateMachine : StateMachine<SnakePregameUI, State>
        {
            public override State DefaultState => new NotStartedState();
        }

        private abstract class State : State<SnakePregameUI, State>
        {
            public virtual void Update() { }
            public virtual void BeginSelection()
            {
                throw new InvalidOperationException();
            }
        }

    }
}