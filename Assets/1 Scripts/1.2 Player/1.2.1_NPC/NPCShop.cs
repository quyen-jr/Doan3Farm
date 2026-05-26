using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShop : NPCBase
{
    public override void OnPlayerTouch()
    {
        ScreenGameManager.instance.Open<ShopScreen>();
    }
}
