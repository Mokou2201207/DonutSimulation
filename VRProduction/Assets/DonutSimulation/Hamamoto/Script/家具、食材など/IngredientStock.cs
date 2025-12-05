using UnityEngine;

public class IngredientStock: FurnitureOwner
{
    [Header("手に持つ食材")]
    public GameObject m_IngredientStock;
    [Header("PlayerPickupをアタッチ")]
    public PlayerPickup m_PlayerHand;
    private void Start()
    {
        if (m_IngredientStock == null)
        {
            Debug.LogError("手に持つ食材が入ってません");
        }
        if (m_PlayerHand==null)
        {
            Debug.LogError("PlayerPickupのscriptがアタッチされてません");
        }
        //Key入力
        useKey = UseKey.LeftClick;
        //コメントUI
        m_KeyHint = "左クリック";
    }

    public override void Interact()
    {
        if (m_PlayerHand == null)
        {
            Debug.LogError("PlayerHand が見つかりません");
            return;
        }      
        //チョコレートを手に（条件が満たせば）
        m_PlayerHand.HandHave(m_IngredientStock);
        
    }
}
