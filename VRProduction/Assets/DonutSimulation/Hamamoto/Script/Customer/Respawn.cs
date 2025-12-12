using UnityEngine;
/// <summary>
/// NPC（お客さん）のスポーンの処理
/// </summary>
public class Respawn : MonoBehaviour
{
    [Header("お客さんの種類"), SerializeField]
    private GameObject[] m_Customers;

    [Header("スポーンする位置"), SerializeField]
    private Transform[] m_RespawnPositions;

    [Header("最低限のリスポーン時間"), SerializeField]
    private float m_RespawnTime = 20;

    [Header("最大限のリスポーン時間"), SerializeField]
    private float m_RespawnMaxTime = 40;
    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        Spawn();
    }
    /// <summary>
    /// キャラクターの種類、スポーン位置、時間をランダムに設定してスポーン
    /// </summary>
    void Spawn()
    {
        //ランダムのキャラを選ぶ
        int randCustomer = Random.Range(0, m_Customers.Length);
        GameObject customer = m_Customers[randCustomer];

        //ランダムのスポーンの位置選ぶ
        int randPos = Random.Range(0, m_RespawnPositions.Length);
        Transform spawnPos = m_RespawnPositions[randPos];

        //その位置にスポーン
        Instantiate(customer, spawnPos.position, spawnPos.rotation);

        ////次のスポーンをランダムで時間を決める
        float nextspown = Random.Range(m_RespawnTime, m_RespawnMaxTime);
        Invoke(nameof(Spawn), nextspown);
    }
}
