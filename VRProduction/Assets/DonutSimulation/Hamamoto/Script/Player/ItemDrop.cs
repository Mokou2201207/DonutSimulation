using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [Header("（自動）PlayerPickupアタッチ")]
    [SerializeField]private PlayerPickup m_playerPickup;

    private void Start()
    {
        if (m_playerPickup==null)
        {
            m_playerPickup = GetComponent<PlayerPickup>();
            if (m_playerPickup==null)
            {
                Debug.LogError("PlayerPickupがアタッチされてません");
            }
        }

    }

    private void Update()
    {
        //アイテムを持っていたなら
        if (m_playerPickup.m_HandHaveNow)
        {
            if (Input.GetMouseButton(1))
            {
               // m_playerPickup.Drop();
            }
        }
    }

}
