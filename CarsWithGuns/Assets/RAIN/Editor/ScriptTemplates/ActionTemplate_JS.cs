using RAIN.Action;
using RAIN.Core;
using System.Collections;

[System.Serializable]
[RAINAction]
public class ActionTemplate_JS : RAIN.Action.RAINAction
{
    public virtual void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public virtual ActionResult Execute(RAIN.Core.AI ai)
    {
        return ActionResult.SUCCESS;
    }

    public virtual void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

}