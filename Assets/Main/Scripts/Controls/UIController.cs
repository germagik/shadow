using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _hintText;
    private static UIController _instance;
    public static UIController Instance
    {
        get
        {
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public virtual void SetHint(string hintText)
    {
        _hintText.SetText(hintText);
    }

    public virtual void ClearHint()
    {
        _hintText.SetText("");
    }
}
