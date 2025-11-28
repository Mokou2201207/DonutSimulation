using UnityEngine;

public class Chocolate : FurnitureOwner
{
    [Header("チョコ（欠片）")]
    public GameObject m_Choko;
    public PlayerPickup m_PlayerHand;
    private void Start()
    {
        if (m_Choko==null)
        {
            Debug.LogError("チョコレートの欠片が入ってません");
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
        m_PlayerHand.HandHave(m_Choko);
        
    }
}
