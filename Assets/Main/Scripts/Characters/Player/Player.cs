using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class Player : Character
{
    [SerializeField] protected float _maxResistance = 100f;
    [SerializeField] protected GameObject _lantern;
    protected float _resistance;
    protected List<Item> _items = new();
    protected List<PrimaryItem> _primaryItems = new();
    protected bool _equippedLantern = false;
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

    protected virtual void Update() 
    {
        LanternUpdate();
    }
    protected virtual void LanternUpdate()
    {
        float currentWeight = _animator.GetLayerWeight(AnimatorLayerIndexes.Lantern);
        bool equipping = _equippedLantern && currentWeight < 1, unequipping = !_equippedLantern && currentWeight > 0;
        if (equipping || unequipping)
        {
            float next;
            if (equipping)
            {
                next = _animator.GetLayerWeight(AnimatorLayerIndexes.Lantern) + Time.deltaTime;
                if (next > 1)
                    next = 1;
            }
            else
            {
                next = _animator.GetLayerWeight(AnimatorLayerIndexes.Lantern) - Time.deltaTime;
                if (next < 0)
                    next = 0;
            }
            _animator.SetLayerWeight(AnimatorLayerIndexes.Lantern, next);
        }
    }

    public virtual void ToggleLantern()
    {
        if (!HasLantern())
        {
            return;
        }
        float currentWeight = _animator.GetLayerWeight(AnimatorLayerIndexes.Lantern);
        bool equipping = _equippedLantern && currentWeight < 1, unequipping = !_equippedLantern && currentWeight > 0;
        if (!equipping && !unequipping)
        {
            _equippedLantern = !_equippedLantern;
            if (_equippedLantern)
                _lantern.SetActive(true);
            else
                _lantern.SetActive(false);
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

    protected override void DoMove(Vector3 normalizedDirection, float maxVelocity, float acceleration)
    {
        _animator.SetFloat(AnimatorParametersNames.DirectionX, normalizedDirection.x);
        _animator.SetFloat(AnimatorParametersNames.DirectionY, normalizedDirection.z);
        Vector3 relativeDirection = ((transform.forward * normalizedDirection.z) + (transform.right * normalizedDirection.x)).normalized;
        if (_body.velocity.sqrMagnitude < Math.Pow(maxVelocity, 2))
        {
            _body.velocity += relativeDirection * acceleration;
        }
        else
        {
            _body.velocity = relativeDirection * maxVelocity;
        }
    }

    protected override void DoStay()
    {
        _body.velocity = Vector3.zero;
    }

    public virtual bool HasLantern()
    {
        return _primaryItems.Any(item => item is Lantern);
    }

    public override void OnHear(PerceptionMark mark)
    {
    }

    public override void OnSight(PerceptionMark mark)
    {
    }

    public virtual void GrabbedBy(Enemy enemy)
    {
        if (IsDead)
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
        _animator.SetTrigger("Die");
        _dead = true;
    }
}
