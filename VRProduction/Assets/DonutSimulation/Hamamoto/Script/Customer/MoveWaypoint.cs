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

    [Header("レジに向かっているかどうか")]
    public bool m_IsGoingToCash = false;

    [Header("会計が終わったかどうか")]
    public bool m_IsCashed = false;

    [Header("買っている最中かどうか"), SerializeField]
    private bool m_IsCashing = false;

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
    /// 会計最中
    /// </summary>
    /// <param name="pos"></param>
    public void GoingtoCash(Vector3 pos)
    {
        m_IsGoingToCash = true;
        m_agent.destination = pos;
    }

    /// <summary>
    /// 会計が終わった後
    /// </summary>
    public void Cashed()
    {
        m_IsCashed=true;
        m_IsGoingToCash = false;
        m_IsCashing=false;
        m_agent.isStopped = false;

        // 次の移動ポイントをセット
        m_agent.destination = m_Manager.m_Waypoints[m_currentIndex].position;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        if (m_IsGoingToCash)
        {
            if (!m_IsCashing)
            {
                if (!m_agent.pathPending && m_agent.remainingDistance <= m_agent.stoppingDistance)
                {
                    // レジに到着
                    m_agent.isStopped = true;
                    m_IsCashing = true;
                    Invoke("Cashed", 3f);
                }
            }
        }
        else
        {
            //NavMeshが有効化どうか
            if (m_agent.enabled)
            {
                //NavMeshAgentがまだ経路を計算中ではなく現在の目的地に到着したら
                if (!m_agent.pathPending && m_agent.remainingDistance < m_agent.stoppingDistance)
                    //次のPointへ
                    MoveToNextPoint();
            }
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
