using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    private Player _player;
    private bool _didFertilizeInCurrentAnim;
    private Photon.Pun.PhotonView _photonView;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _photonView = GetComponent<Photon.Pun.PhotonView>();
    }

    private bool IsLocalPlayer()
    {
        return _photonView == null || _photonView.IsMine;
    }
    public void DisableAllTools()
    {
        if (!IsLocalPlayer()) return;
        UIController.Instance.landInteraction.DisableAllTools();
    }
    public void DisableWateringCan()
    {
        if (!IsLocalPlayer()) return;
        UIController.Instance.landInteraction.DisableTool("Watering can");
    }
    public void DisableFertilizer()
    {
        if (!IsLocalPlayer()) return;
        UIController.Instance.landInteraction.DisableTool("Fertilizer");
    }
    // hoeing
    public void HoeingPlant()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedSmallPlot() != null)
        {
            UIController.Instance.GetCurrentSelectedLandPlot().HoeingPlant(UIController.Instance.GetCurrentSelectedSmallPlot());
        }
    }
    public void EndHoeing()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        UIController.Instance.GetCurrentSelectedLandPlot().ResetActionAfterWorkInSmallPlot();

        //   Debug.Log("end");
        //if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        //if (!UIController.Instance.GetCurrentSelectedLandPlot().IsCancelAction())
        //    UIController.Instance.GetCurrentSelectedLandPlot().SetCurrentActionType(LandPlot.ActionType.Hoeing);
        //else
        //    UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();

        // tutorial part
        //if(TutorialController.Instance.CheckTutorial("Planting", 1)){
        //    Player.LocalPlayer.playerMovement.CancleActionWhenUseJoyStick();
        //    TutorialController.Instance.RunTutorial();        
        //}
        // end
        UnLockMoving();
    }
    // plan crop
    public void PlantCrop()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedSmallPlot() != null)
        {
            LandPlot landPlot = UIController.Instance.GetCurrentSelectedLandPlot();
            UIController.Instance.GetCurrentSelectedLandPlot().PlantCrop(landPlot.GetCurrentItemDataCrop(), UIController.Instance.GetCurrentSelectedSmallPlot());
        }
    }
    public void EndPlantCrop()
    {
        if (!IsLocalPlayer()) return;

        if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        //  UIController.Instance.GetCurrentSelectedLandPlot().ResetActionAfterWorkInSmallPlot();
        // Player.LocalPlayer.playerMovement.SetMoving(false);
        UIController.Instance.landInteraction.Water();
        //if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        //if (!UIController.Instance.GetCurrentSelectedLandPlot().IsCancelAction())
        //    UIController.Instance.GetCurrentSelectedLandPlot().SetCurrentActionType(LandPlot.ActionType.PlantCrop);
        //else
        //    UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();

        //   UnLockMoving();
    }
    // 
    public void UsePesticide()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedSmallPlot() != null)
        {
            UIController.Instance.GetCurrentSelectedLandPlot().UsePesticide(UIController.Instance.GetCurrentSelectedSmallPlot());
        }
    }
    public void EndUsingPesticide()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();
        //if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        //if (!UIController.Instance.GetCurrentSelectedLandPlot().IsCancelAction())
        //    UIController.Instance.GetCurrentSelectedLandPlot().SetCurrentActionType(LandPlot.ActionType.Pesticedes);
        //else
        //    UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();

        UnLockMoving();
    }
    public void Fertilize()
    {
        if (!IsLocalPlayer()) return;
        if (_didFertilizeInCurrentAnim) return;
        _didFertilizeInCurrentAnim = true;

        if (UIController.Instance.GetCurrentSelectedSmallPlot() != null)
        {
            UIController.Instance.GetCurrentSelectedLandPlot().UseFertilizer(UIController.Instance.GetCurrentSelectedSmallPlot());
        }
    }


    public void EAFertilize()
    {
        if (!IsLocalPlayer()) return;
        if (_didFertilizeInCurrentAnim) return;
        _didFertilizeInCurrentAnim = true;

        if (UIController.Instance.GetCurrentSelectedSmallPlot() != null)
        {
            UIController.Instance.GetCurrentSelectedLandPlot().UseFertilizer(UIController.Instance.GetCurrentSelectedSmallPlot());
        }
    }

    public void EndUsingFertilize()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();
        //if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        //if (!UIController.Instance.GetCurrentSelectedLandPlot().IsCancelAction())
        //    UIController.Instance.GetCurrentSelectedLandPlot().SetCurrentActionType(LandPlot.ActionType.Pesticedes);
        //else
        //    UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();

        UnLockMoving();
    }
    public void RankingPlant()
    {
        if (!IsLocalPlayer()) return;

        if (UIController.Instance.GetCurrentSelectedSmallPlot() != null)
        {


            UIController.Instance.GetCurrentSelectedLandPlot().RankingPlant(UIController.Instance.GetCurrentSelectedSmallPlot());
        }
    }
    public void EndRankingPlant()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        UIController.Instance.GetCurrentSelectedLandPlot().ResetActionAfterWorkInSmallPlot();
        //if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        //if (!UIController.Instance.GetCurrentSelectedLandPlot().IsCancelAction())
        //    UIController.Instance.GetCurrentSelectedLandPlot().SetCurrentActionType(LandPlot.ActionType.Ranking);
        //else
        //    UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();

        // tutorial part
        //if(TutorialController.Instance.CheckTutorial("Planting", 3)){
        //    Player.LocalPlayer.playerMovement.CancleActionWhenUseJoyStick();
        //    TutorialController.Instance.RunTutorial();        
        //}
        // end
        UnLockMoving();
    }
    public void Watering()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedSmallPlot())
        {
            UIController.Instance.GetCurrentSelectedLandPlot().WateringPlant(UIController.Instance.GetCurrentSelectedSmallPlot());
        }
    }
    public void EndWatering()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        if (UIController.Instance.GetCurrentSelectedLandPlot().GetCurrentActionType() != LandPlot.ActionType.PlantCrop)
            UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();
        else UIController.Instance.GetCurrentSelectedLandPlot().ResetActionAfterWorkInSmallPlot();
        //if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        //if (!UIController.Instance.GetCurrentSelectedLandPlot().IsCancelAction())
        //    UIController.Instance.GetCurrentSelectedLandPlot().SetCurrentActionType(LandPlot.ActionType.Watering);
        //else
        //    UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();
        //if(TutorialController.Instance.CheckTutorial("Planting", 9)){
        //    Player.LocalPlayer.playerMovement.CancleActionWhenUseJoyStick();
        //    TutorialController.Instance.RunTutorial();        
        //}
        UnLockMoving();
    }
    public void Harvesting()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedSmallPlot())
        {
            UIController.Instance.GetCurrentSelectedLandPlot().HaverstPlant(UIController.Instance.GetCurrentSelectedSmallPlot());
            //  Player.LocalPlayer.playerInteractHandler.Harvest(UIController.Instance.GetCurrentSelectedSmallPlot().GetCurrentCrop());
        }
    }
    public void EndHarvesting()
    {
        if (!IsLocalPlayer()) return;
        if (UIController.Instance.GetCurrentSelectedLandPlot() == null) return;
        UIController.Instance.GetCurrentSelectedLandPlot().ResetAllAndEnablePlayerMovement();
        //if (!UIController.Instance.GetCurrentSelectedLandPlot().IsCancelAction())
        //    UIController.Instance.GetCurrentSelectedLandPlot().SetCurrentActionType(LandPlot.ActionType.Haverst);
        //else


        UnLockMoving();
    }
    public void UnLockMoving()
    {
        if (!IsLocalPlayer()) return;

        _player.playerAnimation.ClearAllTrigger();
        _player.moveSpeed = 3;

    }
    public void StartJumping()
    {
        if (!IsLocalPlayer()) return;
        _player.playerMovement.Jumping();
    }
    public void SetJumping()
    {
        if (!IsLocalPlayer()) return;
        _player.playerMovement.SetJumpingTrue();
    }
}

