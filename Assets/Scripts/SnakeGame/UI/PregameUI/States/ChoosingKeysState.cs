using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public partial class SnakePregameUI
    {
        private class ChoosingKeysState : State
        {
            private List<KeyCode> chosenKeyCodes = new List<KeyCode>();
            protected override void Begin()
            {
                RefreshKeyText();
            }
            public override void Update()
            {
                
                var key = Context.pressedKey;
                if(IsAlphaNum(key) && !chosenKeyCodes.Contains(key))
                {
                    chosenKeyCodes.Add(key);
                    RefreshKeyText();
                    if (chosenKeyCodes.Count == 2)
                        ExitTo(new ChoosingTypeState(chosenKeyCodes));
                } else if(Input.GetKeyDown(KeyCode.Return) && Context.players.Count > 0)
                {
                    ExitTo(new FinishState());
                }
            }

            private void RefreshKeyText()
            {
                for (int i = 0; i < 2; i++)
                {
                    if(chosenKeyCodes.Count > i)
                    {
                        Context.keyTexts[i].text = $"Key : {chosenKeyCodes[i]}";
                    } else
                    {
                        Context.keyTexts[i].text = $"Press next key";
                    }

                }
            }

            private bool IsAlphaNum(KeyCode keyCode)
            {
                return ((keyCode >= KeyCode.A) && (keyCode <= KeyCode.Z))
                     || ((keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9));
            }
        }

    }
}