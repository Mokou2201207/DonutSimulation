using UnityEngine;

public class Register : FurnitureOwner
{
    [Header("オーダー管理")]
    public OrderManager m_OrderManager;

    [Header("PlayerPickupをアタッチ")]
    public PlayerPickup m_RegisterPlayerPickup;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //Key入力とUI表示
        m_UseKey = UseKey.LeftClick;
        m_KeyHint = "左クリック";
    }
    public override void Interact()
    {
       
        //プレイヤーが何も持ってなかったら何もしない
        Item item = m_RegisterPlayerPickup.GetHoldItem();
        if (item == null) return;

        //オーダーに納品
        m_OrderManager.Deliver(item);

        //プレイヤーの手から消す
        m_RegisterPlayerPickup.RemoveItem();
    }
}
