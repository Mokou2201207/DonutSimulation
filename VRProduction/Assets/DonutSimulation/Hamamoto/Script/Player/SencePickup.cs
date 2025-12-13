using UnityEngine;

/// <summary>
/// Playerがシーンにある物を取る処理
/// </summary>
public class SencePickup : FurnitureOwner
{
    [Header("PlayerPickupのscript(自動アタッチ)"), SerializeField]
    private PlayerPickup m_PlayerPickup;
    [Header("持つアイテム"), SerializeField]
    private GameObject m_GetItem;
    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player タグのオブジェクトが見つかりません");
            return;
        }

        m_PlayerPickup = player.GetComponent<PlayerPickup>();

        //Keyの入力
        m_UseKey = UseKey.Q;

        //UIを表示
        m_KeyHint = "Q";

    }
    public override void Interact()
    {
        m_PlayerPickup.PickUp(m_GetItem);
    }
}
