using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Character
{
    protected List<Item> _items = new();
    protected List<PrimaryItem> _primaryItems = new();
    protected bool _grabbed = false;
    public bool IsGrabbed
    {
        get
        {
            return _grabbed;
        }
    }
    protected bool _dead = false;
    public bool IsDead
    {
        get
        {
            return _dead;
        }
    }

    public virtual void CastSpell<T>() where T : Spell
    {
        if (!HasSpell<T>())
        {
            return;
        }
        Spell spell = GetSpell<T>();
        if (spell.CanBeCastedBy(this))
        {
            spell.CastedBy(this);
        }
    }

    public virtual void AddItem(Item item)
    {
       _items.Add(item);
    }
    public virtual void AddPrimaryItem(PrimaryItem primaryItem)
    {
        _primaryItems.Add(primaryItem);
    }

   public virtual void AddMatches(Matches matches)
    {
        foreach(Item item in _items)
        {
            if (item is Matches matchesItem)
            {
                matchesItem.Add(matches);
                return;
            }
        }
        Matches newMatches = ScriptableObject.CreateInstance<Matches>();
        newMatches.Add(matches);
        _items.Add(newMatches);
    }

    public virtual void AddSalt(Salt salt)
    {
        foreach(Item item in _items)
        {
            if (item is Salt saltItem)
            {
                saltItem.Add(salt);
                return;
            }
        }
        Salt newSalt = ScriptableObject.CreateInstance<Salt>();
        newSalt.Add(salt);
        _items.Add(newSalt);
    }

    public virtual void AddPage(SpellPage spellPage)
    {
        foreach (PrimaryItem primaryItem in _primaryItems)
        {
            if(primaryItem is SpellsBook spellsBook)
            {
                spellsBook.Add(spellPage);
                return;
            }
        }
    }

    protected virtual bool HasPrimary<T>() where T : PrimaryItem
    {
        return _primaryItems.Any(item => item is T);
    }
    protected virtual T GetPrimary<T>() where T : PrimaryItem
    {
        return (T)_primaryItems.ElementAtOrDefault(_primaryItems.FindIndex(item => item is T));
    }
    public virtual bool HasSpell<T>() where T : Spell
    {
        if (!HasPrimary<SpellsBook>())
        {
            return false;
        }
        return GetPrimary<SpellsBook>().HasSpell<T>();
    }

    public virtual Spell GetSpell<T>() where T : Spell
    {
        return GetPrimary<SpellsBook>()?.GetSpell<T>();
    }
    public override void OnHear(PerceptionMark mark)
    {
    }

    public override void OnSight(PerceptionMark mark)
    {
    }

    public virtual void GrabbedBy(Enemy enemy)
    {
        if (IsDead || _grabbed)
        {
            return;
        }
        _grabbed = true;
    }

    public virtual bool SuccessfullyGrabbedBy(Enemy enemy)
    {
        transform.forward = enemy.transform.position - transform.position;
        return true;
    }

    public virtual void Killed()
    {
        _dead = true;
        _animator.SetDying();
    }

    public virtual void ToggleLantern()
    {
        _animator.ToggleLantern();
    }
}
