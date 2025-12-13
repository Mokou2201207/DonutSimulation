using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// フライヤーの処理
/// </summary>
public class Flayer : FurnitureOwner
{
    [Header("入ってるドーナツ"), SerializeField]
    private GameObject[] m_InDount;

    [Header("今入ってるドーナツの数"), SerializeField]
    private int m_CurrentCount = 0;

    [Header("PlayerPickupscriptをアタッチ"), SerializeField]
    private PlayerPickup m_FlayerPlayerPickup;

    [Header("アニメーター自動"), SerializeField]
    private Animator m_Animator;

    [Header("油のメーターをアタッチ"), SerializeField]
    private Slider m_Oilslider;

    [Header("オイルの秒数（OUT）"), SerializeField]
    private int m_Oilcount = 10;

    [Header("フライヤーの中に入っているドーナツの数"), SerializeField]
    private GameObject[] m_DountSlots;

    [Header("ドーナツの生地マテリアル"), SerializeField]
    private Material m_DoughnutDoughMaterial;

    [Header("ドーナツが上げ終わったときのマテリアル"),SerializeField]
    private Material m_DonutFryMaterial;

    [Header("揚げたドーナツのprefab"), SerializeField]
    private GameObject m_FringDountPrefab;

    [Header("油のSE"), SerializeField]
    private AudioSource m_OilInSE;

    [Header("タイマーのSE"), SerializeField]
    private AudioSource m_TimerSE;

    [Header("タイマーは一回だけ流すフラグ"), SerializeField]
    private bool m_IsTimerSoundPlayed = false;

    [Header("フライヤーにいれたかどうか"), SerializeField]
    private bool m_FlayerIN = false;

    [Header("ドーナツを揚げたかどうか"),SerializeField]
    private bool m_IsDountFring=false;

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

        // コンポーネント取得
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

        //SEを最初はStop
        m_OilInSE.Stop();
        m_TimerSE.Stop();
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    public override void Interact()
    {
        //ドーナツ生地を持っていればかつドーナツが三つ以上入ってなく揚げて終わった後じゃなければ
        if (m_FlayerPlayerPickup.CheckHaveItem("Doughnutdough") && m_CurrentCount < 3&&!m_IsDountFring)
        {
            m_FlayerPlayerPickup.UseItem();
            PutInChocolate();
        }

        //ドーナツが揚げていたら
        if (m_IsDountFring)
        {
            TakeFriedDonuts();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // PlayerPickup に触っている家具が Flayer かどうか確認
        if (m_FlayerPlayerPickup != null &&
            m_FlayerPlayerPickup.m_currentFurniture == this)
        {
            //右クリック押すとフライヤーにIn/Out
            if (Input.GetMouseButtonDown(1))
            {
                HandleFryerInOut();
            }
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
            else if (m_Oilslider.value >= m_Oilslider.maxValue)
            {
                Debug.Log("油からドーナツをOUT");
                m_Animator.SetBool("IN", false);

                m_FlayerIN = false;
                m_IsTimerSoundPlayed = false;
                m_IsDountFring = true;

                //表示
                m_Oilslider.gameObject.SetActive(false);

                //SEの音をstop
                m_OilInSE.Stop();
                m_TimerSE.Stop();
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

        //タイマーが10秒たったら
        if (m_Oilslider.value >= m_Oilslider.maxValue)
        {
            m_Oilslider.value = m_Oilslider.maxValue;

            if (!m_IsTimerSoundPlayed)
            {
                Debug.Log("タイマー音 再生！");
                m_TimerSE.loop = true;
                m_TimerSE.Play();

                //タイマーフラグオン
                m_IsTimerSoundPlayed = true;

                ChangeDountFryColor();
            }
        }
    }

    /// <summary>
    /// 揚げているドーナツを取る処理
    /// </summary>
    private void TakeFriedDonuts()
    {
        if (m_CurrentCount <= 0) return;

        //持つ処理
        m_FlayerPlayerPickup.HandHave(m_FringDountPrefab);

        //ドーナツを一つ減らし非表示の関数へ
        m_CurrentCount --;
        UpdateDountSlots();

        //ドーナツが０ならフラグを返す
        if (m_CurrentCount==0)
        {
            //フラグを戻す
            m_IsDountFring=false;
            //生地の色へ
            ChangeDoughnutDoughColor();
        }
    }

    /// <summary>
    /// アニメーションイベントでドーナツを油に入れたときのSE
    /// </summary>
    private void OilInSound()
    {
        Debug.Log("油の音を再生");

        //再生（油の音）
        m_OilInSE.loop = true;
        m_OilInSE.Play();
    }

    /// <summary>
    /// ドーナツを上げた時に色を変える処理
    /// </summary>
    private void ChangeDountFryColor()
    {
        foreach(GameObject donut in m_InDount)
        {

            if (donut == null) continue;

            //ローカル変数にドーナツのRendererをコンポーネント
            Renderer DontRenderer =donut.GetComponentInChildren<Renderer>();

            //マテリアルを変更
            if (DontRenderer != null)
            {
                DontRenderer.material = m_DonutFryMaterial;
            }

        }
    }
    /// <summary>
    /// ドーナツ生地のマテリアルに戻す処理
    /// </summary>
    private void ChangeDoughnutDoughColor()
    {
        foreach (GameObject donut in m_InDount)
        {

            if (donut == null) continue;

            //ローカル変数にドーナツのRendererをコンポーネント
            Renderer DontRenderer = donut.GetComponentInChildren<Renderer>();

            //マテリアルを変更
            if (DontRenderer != null)
            {
                DontRenderer.material = m_DoughnutDoughMaterial;
            }

        }
    }
    
}
