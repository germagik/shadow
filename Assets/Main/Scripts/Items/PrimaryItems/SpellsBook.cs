
using System;
using System.Collections.Generic;

public class SpellsBook : PrimaryItem
{
    protected List<SpellPage> _pages = new ();

    public virtual void Add(SpellPage spellPage)
    {
        _pages.Add(spellPage);  
    }
}
