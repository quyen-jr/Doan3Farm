public class NPCClothingMarket : NPCBase
{
    public string id; // Đảm bảo ID này khớp với ID trong ChatManager list

    public override void OnPlayerTouch()
    {
        //Debug.Log($"Đang tương tác với NPC: {id}");
        // Truyền ID vào để Manager biết mở Canvas nào và gửi API cho ai
        ChatManager.Instance.OpenChat(id);
    }
}