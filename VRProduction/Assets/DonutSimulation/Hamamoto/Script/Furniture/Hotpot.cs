using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// チョコレートコーティングの処理
/// </summary>
public class Hotpot : FurnitureOwner
{
    [Header("ChocolateCoatingscriptアタッチ"), SerializeField]
    private ChocolateCoating m_HotpotChocolateCoating;

    [Header("PlayerPickupscriptアタッチ"), SerializeField]
    private PlayerPickup m_PlayerPickup;

    [Header("鍋に入っているチョコ"), SerializeField]
    private GameObject m_HptpotChoko;

    [Header("チョコレートが入れられたどうか"), SerializeField]
    public bool m_InChoko = false;

    [Header("チョコに入れるSE"), SerializeField]
    private AudioSource m_InChocolateSE;

    [Header("ドーナツにチョコを付けるSE"), SerializeField]
    private AudioSource m_CoatingChocolateSE;

    [Header("PlayerInput参照"), SerializeField]
    private PlayerInput m_PlayerInput;

    // VR入力アクション
    private InputAction m_inputGrip;  // 右グリップ（チョココーティング）

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        // PlayerInputがなければPlayerPickupから取得を試みる
        if (m_PlayerInput == null && m_PlayerPickup != null)
        {
            m_PlayerInput = m_PlayerPickup.GetComponent<PlayerInput>();
        }

        if (m_PlayerInput != null)
        {
            m_PlayerInput.currentActionMap.Enable();
            m_inputGrip = m_PlayerInput.currentActionMap.FindAction("GripButtonR");
        }
    }

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //非表示
        m_HptpotChoko.SetActive(false);

        //Keyの入力用UI表示
        m_UseKey = UseKey.RightClick;
        m_KeyHint = "右クリック";

        //停止
        m_InChocolateSE.Stop();
        m_CoatingChocolateSE.Stop();
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // PlayerPickupに触れていて、このHotpotが対象の場合
        if (m_PlayerPickup != null &&
            m_PlayerPickup.m_currentFurniture == this)
        {
            // VR右グリップでチョコレートコーティング
            bool gripPressed = m_inputGrip != null && m_inputGrip.WasPressedThisFrame();
            if (gripPressed)
            {
                // チョコが入っていて、ドーナツを持っている場合
                if (m_InChoko && m_PlayerPickup.CheckHaveItem("Dount"))
                {
                    CoatDonutWithChocolate();
                }
            }
        }
    }

    /// <summary>
    /// 鍋にチョコがあるかどうかを調べる
    /// </summary>
    public override void Interact()
    {
        //持っていない場合
        if (m_PlayerPickup.m_HaveItem == null)
        {
            Debug.Log("何も持っていません");
            return;
        }

        //チョコレートを持っている場合
        if (m_PlayerPickup.CheckHaveItem("Chocolate"))
        {

            m_PlayerPickup.UseItem();
            PutInChocolate();
        }
        else
        {
            Debug.Log("手に持っているのはチョコじゃない！");
        }

        //鍋にチョコレートが入っていて、ドーナツを持っている場合
        if (m_InChoko && m_PlayerPickup.CheckHaveItem("Dount"))
        {
            CoatDonutWithChocolate();
        }

    }

    /// <summary>
    /// ドーナツにチョコレートをコーティングする処理
    /// </summary>
    private void CoatDonutWithChocolate()
    {
        //SE再生
        m_CoatingChocolateSE.Play();
        //手に持っているドーナツのコンポーネント取得
        Dount dount = m_PlayerPickup.m_HaveItem.GetComponent<Dount>();
        if (dount != null)
        {
            dount.DountChangeMaterial();
        }
    }

    /// <summary>
    /// 鍋の中がChocolateかどうか調べる
    /// </summary>
    /// <param name="other">鍋の中身</param>
    private void OnTriggerEnter(Collider other)
    {
        //レイヤーがItemなら処理しない
        if (other.gameObject.layer != LayerMask.NameToLayer("Item")) return;

        //Chocolateなら持っているチョコレートを消して鍋の処理
        GameObject itemObj = other.gameObject;
        if (itemObj.CompareTag("Chocolate"))
        {
            bool success = PutInChocolate();
            if (success)
            {
                Destroy(itemObj);
            }
        }
    }

    /// <summary>
    /// 鍋にチョコレートを入れる処理
    /// </summary>
    /// <returns>チョコを入れられなかったらtrue,入れられたらfalse</returns>
    private bool PutInChocolate()
    {
        //鍋にチョコレートが入っていたら何もしない
        if (m_InChoko)
        {
            Debug.Log("すでに鍋にチョコが入っています");
            return false;
        }

        Debug.Log("チョコレートを鍋に入れました");

        //表示
        m_HptpotChoko.SetActive(true);

        //フラグオン
        m_InChoko = true;

        //SE再生
        m_InChocolateSE.Play();

        return true;
    }
}
