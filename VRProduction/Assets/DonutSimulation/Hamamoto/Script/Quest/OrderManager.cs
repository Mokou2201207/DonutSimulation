using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    [Header("可能なオーダーのリスト"), SerializeField]
    private List<OrderData> m_PossibleOrders;

    [Header("レジSE"), SerializeField]
    private AudioSource m_RegistSE;

    // 現在進行中のオーダー情報
    public OrderData currentOrder { get; private set; }
    
    // 現在の進行状況（元データを壊さないためのコピー）
    private List<OrderCondition> m_ActiveConditions = new List<OrderCondition>();

    public List<OrderCondition> GetActiveConditions() => m_ActiveConditions;

    private void Start()
    {
        // 最初にランダムなオーダーをセット（必要に応じて）
        SetRandomOrder();
    }

    /// <summary>
    /// ランダムなオーダーをセットする
    /// </summary>
    public void SetRandomOrder()
    {
        if (m_PossibleOrders == null || m_PossibleOrders.Count == 0) return;

        // ランダムに選択
        int index = Random.Range(0, m_PossibleOrders.Count);
        currentOrder = m_PossibleOrders[index];

        // 進行状況を初期化（元データからコピー）
        m_ActiveConditions.Clear();
        foreach (var cond in currentOrder.conditions)
        {
            m_ActiveConditions.Add(new OrderCondition 
            { 
                m_ItemType = cond.m_ItemType, 
                m_Count = cond.m_Count 
            });
        }

        Debug.Log($"新しいオーダーをセットしました: {currentOrder.name}");
    }

    /// <summary>
    /// 今渡されたアイテムが、現在のオーダーにちゃんと合っているか
    /// </summary>
    /// <param name="item">プレイヤーが持っていたアイテム</param>
    public void Deliver(Item item)
    {
        // オーダーが無い or アイテム無し
        if (currentOrder == null || item == null) return;

        // 条件を1つずつチェック
        foreach (OrderCondition cond in m_ActiveConditions)
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
        foreach (OrderCondition cond in m_ActiveConditions)
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
        Debug.Log("すべてのオーダーを達成しました！");

        //再生
        if (m_RegistSE != null) m_RegistSE.Play();

        // 完了したら次のランダムオーダーへ
        SetRandomOrder();
    }
}
