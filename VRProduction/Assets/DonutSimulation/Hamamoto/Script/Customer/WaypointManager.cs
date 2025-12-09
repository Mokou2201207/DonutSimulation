using UnityEngine;
/// <summary>
/// NPCのルートのプログラム
/// </summary>
public class WaypointManager : MonoBehaviour
{
    [Header("敵の移動ポイントを導入")]
    public Transform[] m_Waypoints;
    /// <summary>
    ///  子オブジェクトからWaypointを自動登録します
    /// </summary>
    void Awake()
    {
        // 子オブジェクト自動登録
        if (m_Waypoints == null || m_Waypoints.Length == 0)
        {
            m_Waypoints = GetComponentsInChildren<Transform>();
            Debug.Log("子オブジェクトから自動でWaypointsを登録しました。");
        }
        else
        {
            Debug.Log("インスペクターでWaypointsが手動設定されています。");
        }
    }
}
