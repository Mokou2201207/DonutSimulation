using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// フライヤーの処理
/// </summary>
public class Flayer : FurnitureOwner
{
    [Header("入ってるドーナツ"),SerializeField]
    private GameObject[] m_InDount;

    [Header("今入ってるドーナツの数"),SerializeField]
    private int m_CurrentCount = 0;

    [Header("PlayerPickupscriptをアタッチ"),SerializeField]
    private PlayerPickup m_FlayerPlayerPickup;

    [Header("アニメーター自動"), SerializeField]
    private Animator m_Animator;

    [Header("油のメーターをアタッチ"),SerializeField]
    private Slider m_Oilslider;

    [Header("オイルの秒数（OUT）"),SerializeField]
    private int m_Oilcount=10;

    [Header("フライヤーにいれたかどうか"),SerializeField]
    private bool m_FlayerIN = false;

    [Header("フライヤーの中に入っているドーナツの数"),SerializeField]
    private GameObject[] m_DountSlots;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        // 最初は全部非表示にする
        foreach (GameObject slot in m_InDount)
        {
            slot.SetActive(false);
        }

        // Animatorを全て取得
        m_Animator = GetComponent<Animator>();

        //Keyの入力を入れる
        m_UseKey = UseKey.LeftClick;

        //UIを表示
        m_KeyHint = "左クリック";

        //オイルの時間を設定
        m_Oilslider.maxValue = 10f;
        m_Oilslider.value = 0f;

        //非表示
        m_Oilslider.gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Interact()
    {
        //ドーナツ生地を持っていればかつドーナツが三つ以上入って無ければ
        if (m_FlayerPlayerPickup.CheckHaveItem("Doughnutdough") && m_CurrentCount < 3)
        {
            m_FlayerPlayerPickup.UseItem();
            PutInChocolate();
        }
    }

/// <summary>
/// 更新
/// </summary>
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
    
    /// <summary>
    /// フライヤーのアニメーションの処理
    /// </summary>
    private void HandleFryerInOut()
    {
        //ドーナツが一つでも入ってたら処理を行う
        if (m_CurrentCount >= 1)
        {
            if (!m_FlayerIN)
            {
                Debug.Log("油にドーナツをIN");
                m_Animator.SetBool("IN", true);
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
                m_Animator.SetBool("IN", false);
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

    /// <summary>
    /// TagがDoughnutDoughならドーナツのカウントを処理
    /// </summary>
    /// <param name="other">ドーナツ</param>
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
    /// <summary>
    /// ドーナツカウント処理
    /// </summary>
    /// <returns>ドーナツを入れられたらtrue,満タンならfalse</returns>
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

    /// <summary>
    /// ドーナツを入れたときにフライヤーの中にドーナツを表示
    /// </summary>
    private void UpdateDountSlots()
    {
        for (int i = 0; i < m_InDount.Length; i++)
        {
            m_InDount[i].SetActive(i < m_CurrentCount);
        }
    }

   /// <summary>
   /// ドーナツを上げる際のカウント
   /// </summary>
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
