using UnityEngine;
/// <summary>
/// 持っているアイテムを落とす処理
/// </summary>
public class ItemDrop : MonoBehaviour
{
    [Header("（自動）PlayerPickupアタッチ"), SerializeField]
    private PlayerPickup m_playerPickup;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        if (m_playerPickup==null)
        {
            m_playerPickup = GetComponent<PlayerPickup>();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //アイテムを持っていたなら
        if (m_playerPickup.m_HandHaveNow)
        {
            if (Input.GetMouseButton(1))
            {
                
               m_playerPickup.PlaceItem();
            }
        }
    }

}
