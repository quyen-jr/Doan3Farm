using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private PlayerPickedUpItem _playerPickedUpItem;

    private void Start()
    {
        _playerPickedUpItem = GetComponent<PlayerPickedUpItem>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ITriggerCharacterStand>(out var trigger))
        {
            trigger.OnPlayerEnter();
        }

        if (other.TryGetComponent<ITeleport>(out var teleport))
        {
            teleport.Teleport();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ITriggerCharacterStand>(out var trigger))
        {
            trigger.OnPlayerExit();
        }
    }
}

public enum ETriggerState
{
    enter,
    exit,
}
