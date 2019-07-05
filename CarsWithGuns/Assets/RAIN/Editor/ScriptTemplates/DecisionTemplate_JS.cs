using RAIN.Action;
using RAIN.Core;
using System.Collections;

[System.Serializable]
[RAINDecision]
public class DecisionTemplate_JS : RAIN.Action.RAINDecision
{
    private int _lastRunning;
    public virtual void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        this._lastRunning = 0;
    }

    public virtual ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.SUCCESS;
        while (this._lastRunning < _children.Count)
        {
            tResult = _children[this._lastRunning].Run(ai);
            if (tResult != ActionResult.SUCCESS)
            {
                break;
            }
            this._lastRunning++;
        }
        return tResult;
    }

    public virtual void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

}