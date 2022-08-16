using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public partial class SnakePregameUI
    {
        private class ChoosingTypeState : State
        {
            private List<KeyCode> chosenKeyCodes;
            private int index = 0;
            private SnakeTypeAsset currentType => Context.types[index];

            public ChoosingTypeState(List<KeyCode> chosenKeyCodes)
            {
                this.chosenKeyCodes = chosenKeyCodes;
            }
            protected override void Begin()
            {
                Context.typeDisplay.gameObject.SetActive(true);
                OnTypeRefresh();
            }
            protected override void End()
            {
                Context.typeDisplay.gameObject.SetActive(false);
            }
            public override void Update()
            {
                if(Input.GetKeyDown(chosenKeyCodes[0]))
                {
                    index = (Context.types.Length + index - 1) % Context.types.Length;
                    OnTypeRefresh();
                } else if(Input.GetKeyDown(chosenKeyCodes[1]))
                {

                    index = (index + 1) % Context.types.Length;
                    OnTypeRefresh();
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    Context.players.Add(Context.BuildPlayer(currentType, chosenKeyCodes));
                    if(Context.players.Count >= Context.maxPlayers)
                    {
                        ExitTo(new FinishState());
                    } else
                    {
                        ExitTo(new ChoosingKeysState());
                    }
                }
            }

            private void OnTypeRefresh()
            {
                Context.typeDisplay.ShowType(currentType);
            }

        }
    }
}