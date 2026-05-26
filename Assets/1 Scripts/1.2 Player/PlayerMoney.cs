using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    private float _currentMoney = 0;
    private void Start()
    {
        UpdateMoney(0);
    }
    public void UpdateMoney(float delta)
    {
        _currentMoney += delta;
        //UIController.Instance.PlayerMoney.text = _currentMoney.ToString() + "$";
    }
}
