using TMPro;
using UnityEngine;

/// <summary>
/// 注文の時のTextを表示するクラス
/// </summary>
public class QuestText : MonoBehaviour
{
    [Header("クエスト情報取得元"), SerializeField]
    private OrderManager m_OrderManager;

    [Header("注文のテキストを表示する要素"), SerializeField]
    private TextMeshProUGUI[] m_QuestTexts;

    private void Update()
    {
        UpdateQuestDisplay();
    }

    /// <summary>
    /// クエストの表示を更新する
    /// </summary>
    private void UpdateQuestDisplay()
    {
        // if (m_OrderManager == null || m_OrderManager.currentOrder == null)
        //{
        // ClearAllTexts();
        //return;
        // }

        var conditions = m_OrderManager.GetActiveConditions();

        for (int i = 0; i < m_QuestTexts.Length; i++)
        {
            if (i < conditions.Count)
            {
                var cond = conditions[i];
                // 注文の種類ごとに一行 or 二行で表示
                m_QuestTexts[i].text = $"{cond.m_ItemType}\nあと {cond.m_Count}個";
            }
            else
            {
                // 余ったテキスト要素は空にする
                m_QuestTexts[i].text = string.Empty;
            }
        }
    }

    /// <summary>
    /// すべてのテキストをクリアする
    /// </summary>
    private void ClearAllTexts()
    {
        foreach (var text in m_QuestTexts)
        {
            if (text != null) text.text = string.Empty;
        }
    }
}
