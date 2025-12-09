using UnityEngine;
/// <summary>
/// チョコレートの欠片の処理
/// </summary>
public class ChocolateFragment : FurnitureOwner
{
    [Header("（自動）PlayerPickupをアタッチ"),SerializeField]
    private PlayerPickup m_PlayerPickup;

    [Header("チョコレートの欠片")]
    private GameObject m_Choko;

    private void Start()
    {
        // タグ "Player" のオブジェクトを探して PlayerPickup を取得
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            m_PlayerPickup = playerObj.GetComponent<PlayerPickup>();
            if (m_PlayerPickup == null)
            {
                Debug.LogError("PlayerPickup が Player にアタッチされていません");
            }
        }

        //Key入力
        m_UseKey = UseKey.LeftClick;

        //コメントUI
        m_KeyHint = "左クリック";
        
    }
    public override void Interact()
    {
        if (m_PlayerPickup != null)
        {
           
            // Scene上の m_Choko を拾う場合
            m_PlayerPickup.PickUp(m_Choko);

        }
    }

}
