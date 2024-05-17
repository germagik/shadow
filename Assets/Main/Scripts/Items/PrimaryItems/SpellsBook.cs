using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/SpellsBook")]
public class SpellsBook : PrimaryItem
{
    protected List<SpellPage> _pages = new ();

    public virtual void Add(SpellPage spellPage)
    {
        _pages.Add(spellPage);  
    }
}
