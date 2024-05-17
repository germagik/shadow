
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Lantern")]
public class Lantern : PrimaryItem
{
    [SerializeField] protected float _maxDuration = 300f;
    [SerializeField] protected float _duration = 0f;
    [SerializeField] protected bool _fired;
}
