using UnityEngine;
using static FurnitureOwner;
/// <summary>
/// プレイヤーの“拾う系の行動”を全部管理するプログラム
/// </summary>
public class PlayerPickup : MonoBehaviour
{

    [Header("アイテムを拾う音"), SerializeField]
    private AudioClip m_ItemGetSE;

    [Header("拾える距離"), SerializeField]
    private float m_PickUpDistance = 5f;

    [Header("アイテムの移動速度"), SerializeField]
    private float m_ItemMoveSpeed = 0.1f;

    [Header("拾える距離"), SerializeField]
    private float m_ItemDistance = 5f;

    [Header("アイテムを持つ場所"), SerializeField]
    private Transform m_HandHave;

    [Header("KeyUIをアタッチ"), SerializeField]
    private KeyUI m_KeyUI;

    [Header("今何を持っているか")]
    public GameObject m_HaveItem;

    [Header("手に持っているかどうか")]
    public bool m_HandHaveNow = false;

    [Header("アイテムを拾った直後のフレームかどうか"), SerializeField]
    private bool m_IsPickUpFrame = false;

    [Header("手からアイテムまでの距離"), SerializeField]
    private float m_PickUpItemDist;

    [Header("FurnitureOwner（自動）"), SerializeField]
    public FurnitureOwner m_currentFurniture;

    [Header("AudioSource（自動）"), SerializeField]
    private AudioSource m_AudioSource;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        if (m_AudioSource == null)
        {
            m_AudioSource = GetComponent<AudioSource>();
            if (m_AudioSource == null)
            {
                Debug.LogError("AudioSourceが入ってません。");
            }
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //Rayをカメラの真ん中に設定
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        //※Raycast が「何に当たったか？」を教えてくれるのが RaycastHit。
        RaycastHit hit;

        m_currentFurniture = null;

        bool furnitureHit = false;

        int layerMask = LayerMask.GetMask("Item", "RaycastBlock");

        //3m以内でヒットしたら処理を行う
        if (Physics.Raycast(ray, out hit, m_PickUpDistance, layerMask))
        {
            // FurnitureOwner コンポーネントがあれば取得
            FurnitureOwner furniture = hit.collider.GetComponent<FurnitureOwner>();
            if (furniture != null)
            {
                m_currentFurniture = furniture;
                furnitureHit = true;
                Debug.Log("物がクロスヘアが重なってます");
            }
        }

        // UIの表示/非表示は Raycast と FurnitureOwner の判定で決める
        //Image仮
        m_KeyUI.m_KeyImage.enabled = furnitureHit;
        if (furnitureHit)
        {
            m_KeyUI.SetKey(m_currentFurniture.m_KeyHint);
        }

        //Key入力
        if (m_currentFurniture != null)
        {
            if (CheckInput(m_currentFurniture.m_UseKey))
            {
                m_currentFurniture.Interact();
            }
        }
        UpdatePickUp();
    }

    /// <summary>
    /// 物理処理
    /// </summary>
    private void FixedUpdate()
    {
        //持ってるアイテムを手の位置に固定（動いても、左右回転しても）
        if (m_HaveItem != null)
        {
            Vector3 havePos = m_HandHave.position;
            m_HaveItem.transform.position = havePos + m_HandHave.forward * m_PickUpItemDist;
        }
    }

    /// <summary>
    /// 落とすKeyの処理とホイールで距離を調整
    /// </summary>
    private void UpdatePickUp()
    {

        if (m_HaveItem == null) return;

        //掴んだら直後クリックを無効化
        if (m_IsPickUpFrame)
        {
            m_IsPickUpFrame = false;
        }
        else
        {
            //左クリックを押すと落とす処理へ
            if (Input.GetMouseButtonDown(0))
            {
                Drop();
                return;
            }
        }

        //マウスホイール処理
        float scrollDelta = Input.mouseScrollDelta.y;

        if (scrollDelta != 0f)
        {
            m_PickUpItemDist += m_ItemMoveSpeed * scrollDelta;
            m_PickUpItemDist = Mathf.Clamp(m_PickUpItemDist, 0f, m_ItemDistance);
        }
    }

    /// <summary>
    /// Key入力の設定
    /// </summary>
    /// <param name="key">押すKeyの名前</param>
    /// <returns>キーが押されたとき true、押されていないとき false</returns>
    private bool CheckInput(UseKey key)
    {
        switch (key)
        {
            case UseKey.LeftClick:
                return Input.GetMouseButtonDown(0);

            case UseKey.RightClick:
                return Input.GetMouseButtonDown(1);

            case UseKey.E:
                return Input.GetKeyDown(KeyCode.E);

            case UseKey.F:
                return Input.GetKeyDown(KeyCode.F);

            case UseKey.Q:
                return Input.GetKeyDown(KeyCode.Q);
        }

        return false;
    }

    /// <summary>
    ///手に持つ処理（テーブルから取る場合）
    /// </summary>
    /// <param name="item">拾うアイテム</param>
    public void PickUp(GameObject item)
    {
        //アイテムを持っていなかったら掴む
        if (!m_HandHaveNow)
        {
            m_HandHaveNow = true;
            m_IsPickUpFrame = true;

            // Scene上のアイテムを手の子にする
             item.transform.SetParent(m_HandHave);

            // ローカル座標をリセット
            item.transform.position = m_HandHave.position;
            item.transform.rotation = m_HandHave.rotation;
            m_PickUpItemDist = 0f;

            m_HaveItem = item;

            //持ったらコライダーをオフ
            Collider col = m_HaveItem.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            //持ったらキネマをオン
            Rigidbody rb = m_HaveItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            Debug.Log("アイテム取得：" + item.name);
        }
        else
        {
            Debug.Log("すでにアイテムを持っています。");
        }

    }

    /// <summary>
    /// 物を落とす処理
    /// </summary>
    public void Drop()
    {
        if (m_HaveItem == null) return;

        //手から離したらコライダーをオンに
        Collider col = m_HaveItem.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        //手から離したらキネマをオフに
        Rigidbody rb = m_HaveItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // 所持情報をクリア
        m_HaveItem = null;

        // 持っていない状態に戻す
        m_HandHaveNow = false;
    }

    /// <summary>
    /// 手に持つ処理(テーブルに置いてない物を取る場合)
    /// </summary>
    /// <param name="Item">拾うアイテム</param>
    public void HandHave(GameObject Item)
    {
        //アイテムを持っていなかったら掴む
        if (!m_HandHaveNow)
        {
            //アイテムゲットSE
            m_AudioSource.PlayOneShot(m_ItemGetSE);

            //手に持っている
            m_HandHaveNow = true;
            m_IsPickUpFrame = true;

            //プレイヤーの手の位置に生成
            GameObject obj = Instantiate(Item, m_HandHave.position, m_HandHave.rotation);
            obj.transform.position = m_HandHave.position;
            obj.transform.rotation = m_HandHave.rotation;
            m_PickUpItemDist = 0f;

            //今なにを持っているか保存
            m_HaveItem = obj;

            //手に持っているときはコライダーをオフ
            Collider col = m_HaveItem.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            //手に持っている時はキネマをオン
            Rigidbody rb = m_HaveItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            Debug.Log("アイテム取得：" + Item.name);
        }
        else
        {
            Debug.Log("すでにアイテムを持っています。");
        }
    }

    // アイテムを離す処理
    public void PlaceItem()
    {
        // アイテムを持っていなかったら実行しない
        if (!m_HandHaveNow || m_HaveItem == null) return;

        // クロスヘア方向にRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0)
        );

        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Furniture"); // 置けるテーブル用のLayer

        if (Physics.Raycast(ray, out hit, 5f, layerMask))
        {
            // 手から外す
            m_HaveItem.transform.SetParent(null);

            // アイテムの位置をRayが当たった位置に
            m_HaveItem.transform.position = hit.point;

            // アイテムの回転をテーブルの法線方向に合わせる
            m_HaveItem.transform.rotation =
                Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);

            // 手を空にする
            m_HaveItem = null;
            m_HandHaveNow = false;

            Debug.Log("アイテムを置きました：" + hit.collider.name);
        }
        else
        {
            Debug.Log("置ける場所がありません");
        }
    }

    /// <summary>
    /// 現在持っているアイテムのタグが指定したタグと一致するか確認する
    /// </summary>
    /// <param name="tag">確認したいタグ名</param>
    /// <returns>タグが一致していれば true、違えば false</returns>
    public bool CheckHaveItem(string tag)
    {
        //何もアイテムを持っていなかったら処理しない
        if (m_HaveItem == null) return false;
        //アイテムとTagが同じかチェック
        return m_HaveItem.CompareTag(tag);
    }

    /// <summary>
    /// アイテム、Playerが持ってるものをクリア
    /// </summary>
    public void UseItem()
    {
        if (m_HaveItem == null) return;

        // 手に持っているアイテムを消す
        Destroy(m_HaveItem);

        // 所持情報をクリア
        m_HaveItem = null;

        // 持っていない状態に戻す
        m_HandHaveNow = false;
    }
}
