
using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMaskPlayerCanInteract;
    [SerializeField] private float _maxDistance = 100f;

    private int LAND;
    private int PLAYER;
    private RaycastHit _hit;
    private GameObject _currentInteractObj;
    private Player _player;
    private PlayerMovement _playerMovement;
    public GameObject ITEM_PREFAB;


    private void Awake()
    {
        LAND = LayerMask.NameToLayer("Land");
        PLAYER = LayerMask.NameToLayer("Player");
        _player = GetComponent<Player>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    public void Interact(Vector2 touchPos)
    {
        if (!UIController.Instance.IsTouchingUI(touchPos))
        {
            // Debug.Log(touchPos);
            Vector2 screenPos = new Vector3(touchPos.x, touchPos.y);
            Ray ray = CameraController.Instance.GetCurrentCamera().GetComponent<Camera>().ScreenPointToRay(screenPos);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
            Physics.Raycast(ray, out _hit, _maxDistance, _layerMaskPlayerCanInteract);

            if (_hit.collider != null)
            {

                int targetLayer = _hit.collider.gameObject.layer;
                if (Vector3.Distance(transform.position, _hit.collider.gameObject.transform.position) <= _player.landInteractDistance)
                {

                    if (targetLayer == LAND)
                    {
                        //   if (UIController.Instance.CircleMenuIsActive()) return;
                        // Debug.Log(_hit.transform.name);
                        _currentInteractObj = _hit.collider.transform.root.gameObject;
                        SetSelectedObj();

                    }
                    else if (targetLayer == PLAYER && _playerMovement.IsbusyDoingAction())
                    {
                        // EnableActionUI();
                    }
                    else if (targetLayer == LayerMask.NameToLayer("Crop"))
                    {
                        //if (UIController.Instance.CircleMenuIsActive()) return; 
                        if (_playerMovement.IsbusyDoingAction()) return;
                        _currentInteractObj = _hit.collider.transform.parent.gameObject;

                        _currentInteractObj = _currentInteractObj.GetComponent<Crop>().GetCurrentSmallPlot().gameObject;
                        _playerMovement.SetMoveToTarget(_currentInteractObj.transform);
                    }
                    else
                    {
                        if (_hit.collider.TryGetComponent(out IPlayerCanTouchable obj))
                        {
                            obj.OnPlayerTouch();
                        }
                    }
                }
                else
                {
                    if (_currentInteractObj != null)
                        UnSelectedObj();
                }

            }
        }

    }

    //private void EnableActionUI()
    //{
    //    UIController.Instance.ToggleActionPanelUI(true);
    //}
    public void SetSelectedObj()
    {
        //Debug.Log("selected");
        if (_currentInteractObj.GetComponent<Outline>() == null)
        {
            if (_playerMovement.IsbusyDoingAction()) return;
            if (Vector3.Distance(transform.position, _hit.collider.gameObject.transform.position) > 0.01f)
            {
                _playerMovement.SetMoveToTarget(_hit.collider.gameObject.transform);
                return;
            }
            // Debug.Log(_hit.collider.transform.GetComponentInParent<LandPlot>());
            UIController.Instance.SetCurrentSelectedLandPlot(_hit.collider.GetComponentInParent<LandPlot>());
            UIController.Instance.ToggleCircleUI(true);
        }
        else
        {
            UnSelectedObj();
        }
    }

    public void UnSelectedObj()
    {
        if (_playerMovement.IsbusyDoingAction()) return;
        Destroy(_currentInteractObj.GetComponent<Outline>());
        _currentInteractObj = null;
        //UIController.Instance.SetCurrentSelectedLandPlot(null);
        UIController.Instance.ToggleCircleUI(false);
    }
    public GameObject GetCurrentInteractObj() => _currentInteractObj;
    public void Harvest(Crop _currentCrop)
    {
        Debug.Log("harvest");

        ItemData cropData = null;
        if (_currentCrop != null)
        {
            cropData = _currentCrop.GetPlantData();
            //InventoryController.Instance.AddItem(cropData, InventoryController.InventoryType.Agricultural);
        }
    }
}