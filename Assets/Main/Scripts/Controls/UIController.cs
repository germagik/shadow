using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _hintText;
    public virtual void SetHint(string hintText)
    {
        _hintText.SetText(hintText);
    }

    public virtual void ClearHint()
    {
        _hintText.SetText("");
    }
}