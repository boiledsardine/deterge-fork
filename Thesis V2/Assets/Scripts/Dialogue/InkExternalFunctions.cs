using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    public void Bind(Story story)
    {
        story.BindExternalFunction("administerMeds", (string mainVirus) =>
        {
            SyringeBehaviour.instance.resetValues();
            MinigameManager.instance.playerHud.SetActive(false);
            MinigameManager.instance.syringeGame.SetActive(true);
            AssigningBottleWithMeds.instance.setBottleNames(mainVirus);
        });
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("administerMeds");
    }
}