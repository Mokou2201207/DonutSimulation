using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor;
using UnityEngine.AI;
using UnityEngine.InputSystem;
/// <summary>
/// NPCの移動プログラム
/// </summary>
public class MoveWaypoint : MonoBehaviour
{
    [Header("WaypointManagerのscriptを自動アタッチ"), SerializeField]
    private WaypointManager m_Manager;

    [Header("NavMeshAgentをアタッチ"), SerializeField]
    private NavMeshAgent m_agent;

    [Header("どのWaypointに向かっているか"), SerializeField]
    private int m_currentIndex = 0;

    /// <summary>
    /// 開始
    /// </summary>
    [System.Obsolete]
    private void Start()
    {
        //マネージャが未設定なら近くのものを自動探索
        if (m_Manager == null)
        {
            m_Manager = FindClosestManager();
            if (m_Manager == null)
            {
                Debug.LogError("WaypointManagerが入ってません。");
            }
        }

        //コンポーネントを取得
        m_agent = GetComponent<NavMeshAgent>();

        ////次のPointへ
        MoveToNextPoint();
    }
    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //NavMeshが有効化どうか
        if (m_agent.enabled)
        {
            //NavMeshAgentがまだ経路を計算中ではなく現在の目的地に到着したら
            if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
                //次のPointへ
                MoveToNextPoint();
        }
    }
    /// <summary>
    /// Pointについたら次のPointへ
    /// </summary>
    void MoveToNextPoint()
    {
        if (m_Manager == null || m_Manager.m_Waypoints.Length == 0)
        {
            Debug.Log("WaypointManagerのscriptが原因です。");
            return;
        }

        // 最後の waypoint に着いたかチェック
        if (m_currentIndex >= m_Manager.m_Waypoints.Length)
        {
            // 自分を削除
            Destroy(gameObject);
            return;
        }

        // 次の移動ポイントをセット
        m_agent.destination = m_Manager.m_Waypoints[m_currentIndex].position;

        // 次の index へ
        m_currentIndex++;
    }
    /// <summary>
    /// シーン内のWaypointManagerの中から最も近いものを検索します
    /// </summary>
    /// <returns>最も近いWaypointManager</returns>
    [System.Obsolete]
    WaypointManager FindClosestManager()
    {
        //シーンにある全てのWaypointManagerを獲得
        WaypointManager[] managers = FindObjectsOfType<WaypointManager>();

        //最も近いものを保存する物
        WaypointManager closest = null;

        //最小距離の初期値
        float minDist = Mathf.Infinity;

        //獲得したWaypointManagerを一つずつ調べる
        foreach (var m in managers)
        {
            //距離を計算
            float dist = Vector3.Distance(transform.position, m.transform.position);

            //今までよりも近いものがあれば更新
            if (dist < minDist)
            {
                minDist = dist;
                closest = m;
            }
        }

        //最終的に近かったものをいれる
        return closest;
    }
}
