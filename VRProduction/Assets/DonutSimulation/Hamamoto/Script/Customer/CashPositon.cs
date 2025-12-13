using System;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// お客さんをレジに寄るプログラム
/// </summary>
public class CashPositon : MonoBehaviour
{
    [Header("レジに寄る位置"), SerializeField]
    private Transform[] m_CashPositon;

    [Header("レジに立っているリスト"), SerializeField]
    public GameObject[] m_CashNpcList;

    [Header("レジが開いているか"), SerializeField]
    private bool m_IsOpen = false;

    /// <summary>
    /// エリアに入ったらレジへ
    /// </summary>
    /// <param name="other">お客さん</param>
    private void OnTriggerEnter(Collider other)
    {
        //エリアに入ったTagがCustomerじゃなかったら処理しない
        if (!other.CompareTag("Customer")) return;

        //お店がOpenしたら処理をする。
        if (!m_IsOpen)
        {
            Debug.Log("まだお店は空いてません");
            return;
        }

        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        MoveWaypoint move = other.GetComponent<MoveWaypoint>();

        //会計が終わっていたら何もしない
        if (agent == null || move == null) return;
        if (move.m_IsCashed) return;

        //NPCリストが空いているかチェック
        for (int i = 0; i < m_CashNpcList.Length; i++)
        {
            if (m_CashNpcList[i] == null)
            {
                //ここのレジが空いてる
                m_CashNpcList[i] = other.gameObject;
                // このレジの位置へ移動
                move.GoingtoCash(m_CashPositon[i].position);

                break;
            }
        }
    }

    /// <summary>
    /// NPCが出たらレジを空ける
    /// </summary>
    /// <param name="other">お客さん</param>
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Customer")) return;

        MoveWaypoint move = other.GetComponent<MoveWaypoint>();

        //NPCをリストから抜く
        for (int i=0; i<m_CashNpcList.Length; i++)
        {
            //リストにそのオブジェクトがあるならnullを返す
            if (m_CashNpcList[i] == other.gameObject)
            {
                m_CashNpcList[i] = null;
                break;
            }
        }

    }

}
