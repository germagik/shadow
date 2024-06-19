using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/SpellsBook")]
public class SpellsBook : PrimaryItem
{
    protected List<SpellPage> _pages = new ();

    public virtual void Add(SpellPage spellPage)
    {
        _pages.Add(spellPage);  
    }

    public virtual bool HasSpell<T>() where T : Spell
    {
        return _pages.Any(page => page is T);
    }

    public virtual Spell GetSpell<T>() where T : Spell
    {
        return _pages.ElementAtOrDefault(_pages.FindIndex(page => page.Spell is T))?.Spell;
    }
}
