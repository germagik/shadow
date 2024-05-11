using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Player : Character
{
    [SerializeField] protected float _maxResistance = 100f;
    protected float _resistance;
    protected List<Item> _items = new();
    protected List<PrimaryItem> _primaryItems = new();
    protected bool _equippedLantern = false;
    private Lantern _lantern;

    protected override void Awake()
    {
        base.Awake();
        _lantern = GetComponentInChildren<Lantern>(true);
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
        float currentWeight = _animator.GetLayerWeight(AnimatorLayerIndexes.Lantern);
        bool equipping = _equippedLantern && currentWeight < 1, unequipping = !_equippedLantern && currentWeight > 0;
        if (!equipping && !unequipping)
        {
            _equippedLantern = !_equippedLantern;
            if (_equippedLantern)
                _lantern.gameObject.SetActive(true);
            else
                _lantern.gameObject.SetActive(false);
        }
    }
}
