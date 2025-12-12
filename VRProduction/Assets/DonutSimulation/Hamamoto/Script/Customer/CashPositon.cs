using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// お客さんをレジに寄るプログラム
/// </summary>
public class CashPositon : MonoBehaviour
{
    [Header("レジに寄る位置")]
    public Transform m_CashPositon;

    [Header("レジが開いているか")]
    public bool m_IsOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Customer")) return;

        if (!m_IsOpen) return;

        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        MoveWaypoint move = other.GetComponent<MoveWaypoint>();

        if (agent == null || move == null) return;

        // レジへ向かわせる
        agent.destination = m_CashPositon.position;
        move.m_IsGoingToCash = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Customer")) return;

        MoveWaypoint move = other.GetComponent<MoveWaypoint>();
        if (move != null)
        {
            move.m_IsGoingToCash = false;
        }
    }

}
