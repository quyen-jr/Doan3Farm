using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _displayText;

    [SerializeField] private Color _successColor = Color.green;
    [SerializeField] private Color _failColor = Color.red;


    [SerializeField] private Button _hideButton;
    private void OnEnable()
    {
        _hideButton.onClick.AddListener(HideNotification);
    }


    private void OnDisable()
    {
        _hideButton?.onClick.RemoveListener(HideNotification);
    }

    public void SetNotification(string text, bool isSucces)
    {
        _displayText.text = text;

        if (isSucces)
        {
            _displayText.color = _successColor;
        }
        else
        {
            _displayText.color = _failColor;
        }
    }


    public void HideNotification()
    {
        gameObject.SetActive(false);
    }
}