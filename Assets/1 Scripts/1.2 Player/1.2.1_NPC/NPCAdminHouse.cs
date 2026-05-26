using UnityEngine;

public class NPCAdminHouse : NPCBase
{
    public override void OnPlayerTouch()
    {
        Debug.Log("On touch NPCAdminHouse");
        ScreenGameManager.instance.Open<AdminHouseScreen>();
    }
}
