using UnityEngine;
using UnityEngine.UI;

public class Flayer : FurnitureOwner
{
    [Header("入ってるドーナツ")]
    public GameObject[] m_InDount;
    [Header("今入ってるドーナツの数")]
    public int m_CurrentCount = 0;
    [Header("PlayerPickupscriptをアタッチ")]
    public PlayerPickup m_playerpickup;
    [Header("アニメーター自動")]
    [SerializeField] Animator m_animator;
    [Header("油のメーターをアタッチ")]
    [SerializeField] Slider m_Oilslider;
    [SerializeField] int m_Oilcount=10;
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
        //オイルの時間を設定
        m_Oilslider.maxValue = 10f;
        m_Oilslider.value = 0f;
        //非表示
        m_Oilslider.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        //ドーナツ生地を持っていればかつドーナツが三つ以上入って無ければ
        if (m_playerpickup.CheckHaveItem("Doughnutdough") && m_CurrentCount < 3)
        {

            m_playerpickup.UseItem();
            PutInChocolate();
        }

    }



    private void Update()
    {
        //右クリック押すとフライヤーにIn
        if (Input.GetMouseButtonDown(1)) // 右クリック
        {
            HandleFryerInOut();
        }
        // INのときだけ油のカウントを進める
        if (m_FlayerIN)
        {
            OilCount();
        }
    }
    //フライヤーアニメーション処理
    private void HandleFryerInOut()
    {
        //ドーナツが一つでも入ってたら処理を行う
        if (m_CurrentCount >= 1)
        {
            if (!m_FlayerIN)
            {
                Debug.Log("油にドーナツをIN");
                m_animator.SetBool("IN", true);
                m_FlayerIN = true;
                //カウントをリセット
                m_Oilslider.value = 0f;
                //表示
                m_Oilslider.gameObject.SetActive(true);
            }
            //カウントが０になったら上げる
            else if(m_Oilslider.value >= m_Oilslider.maxValue)
            {
                Debug.Log("油からドーナツをOUT");
                m_animator.SetBool("IN", false);
                m_FlayerIN = false;
                //表示
                m_Oilslider.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("まだ焼き上がってない！");
            }
        }
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
    //ドーナツをカウントする処理
    private bool PutInChocolate()
    {
        //三つ以上入れようとしたら入れれなくなる
        if (m_CurrentCount >= 3)
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
    //ドーナツを入れたときの表示
    private void UpdateDountSlots()
    {
        for (int i = 0; i < m_InDount.Length; i++)
        {
            m_InDount[i].SetActive(i < m_CurrentCount);
        }
    }
    //オイルに入れたときのタイマー
    private void OilCount()
    {
        m_Oilslider.value += Time.deltaTime;

        // 10秒経過したら完成
        if (m_Oilslider.value >= m_Oilslider.maxValue)
        {
            m_Oilslider.value = m_Oilslider.maxValue;
            Debug.Log("揚げ終わり！ OUTできる状態！");
        }
    }
}
