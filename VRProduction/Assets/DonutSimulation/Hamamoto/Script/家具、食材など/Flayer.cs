using UnityEngine;

public class Flayer :FurnitureOwner
{
    [Header("入ってるドーナツ")]
    public GameObject[] m_InDount;
    [Header("今入ってるドーナツの数")]
    public int m_CurrentCount=0;
    [Header("PlayerPickupscriptをアタッチ")]
    public PlayerPickup m_playerpickup;
    [Header("アニメーター自動")]
    [SerializeField] Animator m_animator;
    [SerializeField] bool m_FlayerIN = false;
    // 入っている状態を見せるスロット3つ
    public GameObject[] m_DountSlots;
    private void Start()
    {
        // 最初は全部非表示にする
        foreach (GameObject slot in m_InDount)
        {
            slot.SetActive(false);
        }
        // Animatorを全て取得
        m_animator = GetComponent<Animator>();
        //Keyの入力を入れる
        useKey = UseKey.LeftClick;
        //UIを表示
        m_KeyHint = "左クリック";
    }

    public override void Interact()
    {
        //ドーナツ生地を持っていればかつドーナツが三つ以上入って無ければ
        if (m_playerpickup.CheckHaveItem("Doughnutdough")&& m_CurrentCount < 3)
        {

            m_playerpickup.UseItem();
            PutInChocolate();
        }
        /*//左クリック押したら油にIN
        if (!m_FlayerIN)
        {
            Debug.Log("油にドーナツをIN");
            m_animator.SetBool("IN", true);
            //油に入れた
            m_FlayerIN=true;
        }
        else
        {
            Debug.Log("油にドーナツを出した");
            m_animator.SetBool("IN", false);
            //油から出した
            m_FlayerIN = false;
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        //手に持っているものがItemじゃなかったら処理しない
        if (other.gameObject.layer != LayerMask.NameToLayer("Item")) return;

        GameObject itemObj = other.gameObject;
        if (itemObj.CompareTag("DoughnutDough"))
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
        //三つ以上入れようとしたら入れれなくなる
        if (m_CurrentCount>=3)
        {
            Debug.Log("もうドーナツが満タンです");
            return false;
        }
        //ドーナツを加算
        m_CurrentCount++;
        Debug.Log($"ドーナツを油の網に入れました（現在 {m_CurrentCount}/3）");
        // 入れた数に応じてスロット表示
        UpdateDountSlots();
        return true;
    }
    private void UpdateDountSlots()
    {
        for (int i = 0; i < m_InDount.Length; i++)
        {
            m_InDount[i].SetActive(i < m_CurrentCount);
        }
    }
}
