using UnityEngine;
/// <summary>
/// アイテムを持つ処理
/// </summary>
public class IngredientStock : FurnitureOwner
{
    [Header("手に持つ食材"), SerializeField]
    private GameObject m_IngredientStock;

    [Header("PlayerPickupをアタッチ"), SerializeField]
    private PlayerPickup m_PlayerHand;
    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //Key入力
        m_UseKey = UseKey.LeftClick;

        //コメントUI
        m_KeyHint = "左クリック";
    }
    /// <summary>
    /// 食べ物を手に持つ
    /// </summary>
    public override void Interact()
    {
        //PlayerPickupのscriptが無ければ処理をしない
        if (m_PlayerHand == null)
        {
            Debug.LogError("PlayerHand が見つかりません");
            return;
        }

        //食べ物を手に（条件が満たせば）
        m_PlayerHand.HandHave(m_IngredientStock);
    }
}
