using TMPro;
using UnityEngine;

/// <summary>
/// レジ処理 ＋ 注文テキスト表示
/// </summary>
public class Register : FurnitureOwner
{
    [Header("オーダー管理")]
    [SerializeField] private OrderManager m_OrderManager;

    [Header("PlayerPickupをアタッチ")]
    [SerializeField] private PlayerPickup m_RegisterPlayerPickup;

    [Header("レジのクロスヘアに当たるコライダー")]
    [SerializeField] private BoxCollider m_BoxCollider;

    [Header("このオブジェクトは何番目を担当しているか")]
    [SerializeField] private int m_MyIndex;

    [Header("注文のテキスト")]
    [SerializeField] private TextMeshProUGUI[] m_QuestTexts;

    [Header("参照")]
    [SerializeField] private CashPositon m_CashPositon;

    private bool m_IsShown = false;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        if (m_BoxCollider == null)
            m_BoxCollider = GetComponent<BoxCollider>();

        // 初期状態：非表示
        m_BoxCollider.enabled = false;

        foreach (var text in m_QuestTexts)
        {
            if (text != null)
                text.gameObject.SetActive(false);
        }

        // Key入力とUI表示
        m_UseKey = UseKey.LeftClick;
        m_KeyHint = "左クリック";
    }

    private void Update()
    {
        // すでに表示済みなら何もしない
        if (m_IsShown) return;

        // 自分の担当レジにお客さんが来たら
        if (m_CashPositon != null &&
            m_CashPositon.m_CashNpcList != null &&
            m_CashPositon.m_CashNpcList.Length > m_MyIndex &&
            m_CashPositon.m_CashNpcList[m_MyIndex] != null)
        {
            ShowQuestTexts();
            m_BoxCollider.enabled = true;
            m_IsShown = true;
        }
    }

    /// <summary>
    /// 注文内容を表示
    /// </summary>
    private void ShowQuestTexts()
    {
        if (m_OrderManager == null || m_OrderManager.currentOrder == null)
        {
            ClearAllTexts();
            return;
        }

        var conditions = m_OrderManager.GetActiveConditions();

        for (int i = 0; i < m_QuestTexts.Length; i++)
        {
            if (m_QuestTexts[i] == null) continue;

            if (i < conditions.Count)
            {
                var cond = conditions[i];
                m_QuestTexts[i].text =
                    $"{cond.m_ItemType}\nあと {cond.m_Count}個";
            }
            else
            {
                m_QuestTexts[i].text = string.Empty;
            }

            m_QuestTexts[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 全テキストを消す
    /// </summary>
    private void ClearAllTexts()
    {
        foreach (var text in m_QuestTexts)
        {
            if (text != null)
                text.text = string.Empty;
        }
    }

    /// <summary>
    /// プレイヤーがレジを操作
    /// </summary>
    public override void Interact()
    {
        Item item = m_RegisterPlayerPickup.GetHoldItem();
        if (item == null) return;

        // オーダーに納品
        m_OrderManager.Deliver(item);

        // プレイヤーの手から消す
        m_RegisterPlayerPickup.RemoveItem();

        // 注文表示を更新
        ShowQuestTexts();
    }
}
