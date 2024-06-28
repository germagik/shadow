
using System;

public class ConfineWeakness : Weakness<Confine>
{
    public override void ActionatedBy(Player player, Action<string> Callback)
    {
        player.CastSpell<Confine>(Callback + OnActionEvent);
    }

    protected virtual void OnActionEvent(string eventName)
    {
        switch (eventName)
        {
            case "Success":
                _enemy.SetState(Bounded.Instance);
                break;
        }
    }
}
