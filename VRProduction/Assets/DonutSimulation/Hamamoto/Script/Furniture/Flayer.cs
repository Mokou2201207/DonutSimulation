using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
/// <summary>
/// フライヤーの処理
/// </summary>
public class Flayer : FurnitureOwner
{
    [Header("入っているドーナツ"), SerializeField]
    private GameObject[] m_InDount;

    [Header("入っているドーナツの数"), SerializeField]
    private int m_CurrentCount = 0;

    [Header("PlayerPickupscriptアタッチ"), SerializeField]
    private PlayerPickup m_FlayerPlayerPickup;

    [Header("アニメーター"), SerializeField]
    private Animator m_Animator;

    [Header("油のメーターアタッチ"), SerializeField]
    private Slider m_Oilslider;

    [Header("油の秒数（OUT）"), SerializeField]
    private int m_Oilcount = 10;

    [Header("フライヤーの中に入っているドーナツの数"), SerializeField]
    private GameObject[] m_DountSlots;

    [Header("ドーナツの生地マテリアル"), SerializeField]
    private Material m_DoughnutDoughMaterial;

    [Header("ドーナツ揚げ終わりのマテリアル"),SerializeField]
    private Material m_DonutFryMaterial;

    [Header("揚げドーナツprefab"), SerializeField]
    private GameObject m_FringDountPrefab;

    [Header("油SE"), SerializeField]
    private AudioSource m_OilInSE;

    [Header("タイマーSE"), SerializeField]
    private AudioSource m_TimerSE;

    [Header("タイマーは一回だけフラグ"), SerializeField]
    private bool m_IsTimerSoundPlayed = false;

    [Header("フライヤーに入れられたどうか"), SerializeField]
    private bool m_FlayerIN = false;

    [Header("ドーナツ揚げたどうか"),SerializeField]
    private bool m_IsDountFring=false;

    [Header("PlayerInput参照"), SerializeField]
    private PlayerInput m_PlayerInput;

    // VR入力アクション
    private InputAction m_inputA;       // Aボタン（ドーナツ回収）
    private InputAction m_inputGrip;    // 右グリップ（揚げ操作）

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        // PlayerInputがなければPlayerPickupから取得を試みる
        if (m_PlayerInput == null && m_FlayerPlayerPickup != null)
        {
            m_PlayerInput = m_FlayerPlayerPickup.GetComponent<PlayerInput>();
        }

        if (m_PlayerInput != null)
        {
            m_PlayerInput.currentActionMap.Enable();
            m_inputA = m_PlayerInput.currentActionMap.FindAction("A");
            m_inputGrip = m_PlayerInput.currentActionMap.FindAction("GripButtonR");
        }
    }

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        // 最初は全非表示にする
        foreach (GameObject slot in m_InDount)
        {
            slot.SetActive(false);
        }

        // コンポーネント取得
        m_Animator = GetComponent<Animator>();

        //Keyの入力用
        m_UseKey = UseKey.LeftClick;

        //UI表示
        m_KeyHint = "クリック";

        //オイルの時間を設定
        m_Oilslider.maxValue = 10f;
        m_Oilslider.value = 0f;

        //非表示
        m_Oilslider.gameObject.SetActive(false);

        //SE最初Stop
        m_OilInSE.Stop();
        m_TimerSE.Stop();
    }

    /// <summary>
    /// インタラクト処理
    /// </summary>
    public override void Interact()
    {
        //ドーナツ生地を持っていればドーナツを3個以上入っていなくて揚げ終わっていなければ
        if (m_FlayerPlayerPickup.CheckHaveItem("Doughnutdough") && m_CurrentCount < 3&&!m_IsDountFring)
        {
            m_FlayerPlayerPickup.UseItem();
            PutInChocolate();
        }

        //ドーナツ揚げてれば
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
        // PlayerPickup に触れていると同時に Flayer かどうか確認
        if (m_FlayerPlayerPickup != null &&
            m_FlayerPlayerPickup.m_currentFurniture == this)
        {
            // 右クリックまたはVR右グリップでフライヤーIn/Out
            bool gripPressed = m_inputGrip != null && m_inputGrip.WasPressedThisFrame();
            if (Input.GetMouseButtonDown(1) || gripPressed)
            {
                HandleFryerInOut();
            }

            // VR Aボタンで揚げたドーナツを回収
            bool aButtonPressed = m_inputA != null && m_inputA.WasPressedThisFrame();
            if (aButtonPressed && m_IsDountFring)
            {
                TakeFriedDonuts();
            }
        }
        // INのときのカウント進める
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
        //ドーナツが入っていれば処理実行
        if (m_CurrentCount >= 1)
        {
            if (!m_FlayerIN)
            {
                Debug.Log("ドーナツIN");
                m_Animator.SetBool("IN", true);
                m_FlayerIN = true;

                //カウントセット
                m_Oilslider.value = 0f;

                //表示
                m_Oilslider.gameObject.SetActive(true);
            }
            //カウント0になったら揚げ終わり
            else if (m_Oilslider.value >= m_Oilslider.maxValue)
            {
                Debug.Log("ドーナツOUT");
                m_Animator.SetBool("IN", false);

                m_FlayerIN = false;
                m_IsTimerSoundPlayed = false;
                m_IsDountFring = true;

                //非表示
                m_Oilslider.gameObject.SetActive(false);

                //SEの音stop
                m_OilInSE.Stop();
                m_TimerSE.Stop();
            }
            else
            {
                Debug.Log("まだ揚げあがってない！");
            }
        }
    }

    /// <summary>
    /// TagがDoughnutDoughならドーナツのカウント処理
    /// </summary>
    /// <param name="other">ドーナツ</param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter検知: {other.gameObject.name}, Layer: {LayerMask.LayerToName(other.gameObject.layer)}, Tag: {other.gameObject.tag}");

        //今持っているのがItemなら処理しない → Itemレイヤーでなければ処理しない
        if (other.gameObject.layer != LayerMask.NameToLayer("Item"))
        {
            Debug.Log($"Itemレイヤーではないのでスキップ: {LayerMask.LayerToName(other.gameObject.layer)}");
            return;
        }

        GameObject itemObj = other.gameObject;
        if (itemObj.CompareTag("DoughnutDough"))
        {
            Debug.Log("DoughnutDoughタグ検知！ドーナツを入れます");
            bool success = PutInChocolate();
            if (success)
            {
                Destroy(itemObj);
            }
        }
        else
        {
            Debug.Log($"タグが一致しません: {itemObj.tag}");
        }
    }
    /// <summary>
    /// ドーナツカウント処理
    /// </summary>
    /// <returns>ドーナツ入れられたらtrue,入れられなければfalse</returns>
    private bool PutInChocolate()
    {
        //3個以上入れようとしたら入らない
        if (m_CurrentCount >= 3)
        {
            Debug.Log("ドーナツ満タンです");
            return false;
        }

        //ドーナツ増える
        m_CurrentCount++;
        Debug.Log($"ドーナツを油の中に入れました（計 {m_CurrentCount}/3）");

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
    /// ドーナツ揚げ中のカウント
    /// </summary>
    private void OilCount()
    {
        m_Oilslider.value += Time.deltaTime;

        //タイマーが10秒以上
        if (m_Oilslider.value >= m_Oilslider.maxValue)
        {
            m_Oilslider.value = m_Oilslider.maxValue;

            if (!m_IsTimerSoundPlayed)
            {
                Debug.Log("タイマー完了 再生！");
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

        //取る
        m_FlayerPlayerPickup.HandHave(m_FringDountPrefab);

        //ドーナツ減らし表示の関数
        m_CurrentCount --;
        UpdateDountSlots();

        //ドーナツが0なら フラグ戻す
        if (m_CurrentCount==0)
        {
            //フラグ戻す
            m_IsDountFring=false;
            //生地の色に戻す
            ChangeDoughnutDoughColor();
        }
    }

    /// <summary>
    /// アニメーションイベントでドーナツを油に入れたときSE
    /// </summary>
    private void OilInSound()
    {
        Debug.Log("油の音再生");

        //再生（油の音）
        m_OilInSE.loop = true;
        m_OilInSE.Play();
    }

    /// <summary>
    /// ドーナツ揚げ終わりに色を変える処理
    /// </summary>
    private void ChangeDountFryColor()
    {
        foreach(GameObject donut in m_InDount)
        {

            if (donut == null) continue;

            //ローカル変数にドーナツのRendererコンポーネント
            Renderer DontRenderer =donut.GetComponentInChildren<Renderer>();

            //マテリアル変更
            if (DontRenderer != null)
            {
                DontRenderer.material = m_DonutFryMaterial;
            }

        }
    }
    /// <summary>
    /// ドーナツ生地のマテリアルに戻す
    /// </summary>
    private void ChangeDoughnutDoughColor()
    {
        foreach (GameObject donut in m_InDount)
        {

            if (donut == null) continue;

            //ローカル変数にドーナツのRendererコンポーネント
            Renderer DontRenderer = donut.GetComponentInChildren<Renderer>();

            //マテリアル変更
            if (DontRenderer != null)
            {
                DontRenderer.material = m_DoughnutDoughMaterial;
            }

        }
    }
    
}
