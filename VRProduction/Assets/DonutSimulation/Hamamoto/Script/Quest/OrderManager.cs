using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public OrderData currentOrder;
    [Header("レジSE"), SerializeField]
    private AudioSource m_RegistSE;
    /// <summary>
    /// 今渡されたアイテムが、現在のオーダーにちゃんと合っているか
    /// </summary>
    /// <param name="item">プレイヤーが持っていたアイテム</param>
    public void Deliver(Item item)
    {
        // オーダーが無い or アイテム無し
        if (currentOrder == null || item == null) return;

        // 条件を1つずつチェック
        foreach (OrderCondition cond in currentOrder.conditions)
        {
            // 種類が一致 ＆ まだ必要数が残っている
            if (cond.m_ItemType == item.m_ItemType && cond.m_Count > 0)
            {
                cond.m_Count--;          // 納品カウントを減らす
                CheckComplete();       // クリア確認
                return;                // 1個納品したら終了
            }
        }

        // ここに来たら「注文と違う物」
        Debug.Log("注文と違うアイテムです");
    }

    /// <summary>
    /// オーダーがすべて通ったか確認
    /// </summary>
    private void CheckComplete()
    {
        // すべての条件が 0 か確認
        foreach (OrderCondition cond in currentOrder.conditions)
        {
            if (cond.m_Count > 0)
                // まだ未達成
                return; 
        }

        CompleteOrder();
    }

    /// <summary>
    /// オーダー完了
    /// </summary>
    private void CompleteOrder()
    {
        Debug.Log("オーダー達成！");

        //再生
        m_RegistSE.Play();

        // 次のオーダーへ（今は仮）
        currentOrder = null;
    }
}
