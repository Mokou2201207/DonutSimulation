using UnityEngine;

public class Hotpot : FurnitureOwner
{
    [Header("PlayerPickupscriptをアタッチ")]
    public PlayerPickup m_PlayerPickup;
    [Header("鍋に入ってるチョコ")]
    public GameObject m_HptpotChoko;
    //チョコをいれたかどうか
    private bool m_InChoko=false;
    private void Start()
    {
        //非表示
        m_HptpotChoko.SetActive(false);
        //Key入力とUI表示
        useKey = UseKey.LeftClick;
        m_KeyHint = "左クリック";
    }
    public override void Interact()
    {
        //何も持っていなかったら
        if (m_PlayerPickup.m_HaveItem==null)
        {
            Debug.Log("何も持ってません");
            return;
        }

        //チョコを持っていれば
        if (m_PlayerPickup.CheckHaveItem("Chocolate"))
        {

            m_PlayerPickup.UseItem();
            PutInChocolate();
        }
        else
        {
            Debug.Log("それはチョコじゃない！");
        }

       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Item")) return;

        GameObject itemObj= other.gameObject;
        if(itemObj.CompareTag("Chocolate"))
        {
            bool success = PutInChocolate();
            if (success)
            {
                Destroy(itemObj);
            }
        }
    }
    private bool PutInChocolate()
    {
        if (m_InChoko)
        {
            Debug.Log("もう中にチョコが入ってます");
            return false;
        }

        Debug.Log("チョコを鍋に");
        m_HptpotChoko.SetActive(true);
        m_InChoko = true;

        return true;
    }
}
