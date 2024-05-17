using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SightPoint : MonoBehaviour
{
    [SerializeField] protected GameObject _from;
    public GameObject From {
        get
        {
            return _from;
        }
    }
}
