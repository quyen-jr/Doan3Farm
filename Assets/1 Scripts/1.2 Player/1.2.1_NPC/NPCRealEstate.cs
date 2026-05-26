public class NPCRealEstate : NPCBase
{
    public override void OnPlayerTouch()
    {
        ScreenGameManager.instance.Open<RealEstateScreen>();
    }
}
