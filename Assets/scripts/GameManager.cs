using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    private GameObject StonePrefab { get; set; } = null;
    private GameObject BlockPrefab { get; set; } = null;
    private AsyncOperationHandle<GameObject> BlockOperationHandle;
    private AsyncOperationHandle<GameObject> StoneOperationHandle;
    [SerializeField]
    private string BLOCKPREFAB_KEY = "Block";
    [SerializeField]
    private string STONEPREFAB_KEY = "Stone";
    [SerializeField]
    private Vector3 BlockGenerationStartPos = Vector3.zero;
    private const float SLIDE_NUM = 9.5f;
    private List<List<StoneInfo>> StoneInfos = new();
    public StoneInfo GetStoneInfo(int column_x = 1, int row_z = 1)
    {
        if (StoneInfos.Count == 0) { return null; }
        if (StoneInfos.Count >= column_x && column_x >= 1)
        {
            if (StoneInfos[column_x - 1].Count >= row_z && row_z >= 1)
            {
                return StoneInfos[column_x - 1][row_z - 1];
            }
        }
        return null;
    }

    public GameStatus GameStatus { get; private set; } = GameStatus.Black;


    // Start is called before the first frame update
    private async void Start()
    {
        BlockOperationHandle = Addressables.LoadAssetAsync<GameObject>(BLOCKPREFAB_KEY);
        await BlockOperationHandle.Task;
        BlockPrefab = BlockOperationHandle.Result;
        Addressables.Release(BlockOperationHandle);

        StoneOperationHandle = Addressables.LoadAssetAsync<GameObject>(STONEPREFAB_KEY);
        await StoneOperationHandle.Task;
        StonePrefab = StoneOperationHandle.Result;
        Addressables.Release(StoneOperationHandle);


        InstantiateBlocks();
        StoneConnecting();
        StartStoneSetting();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    /// <summary>
    /// マス目(グリッドを8✕8で生成)
    /// </summary>
    private void InstantiateBlocks()
    {
        Vector3 vec = Vector3.zero;
        for (int i = 0; i < 8; i++)
        {
            List<StoneInfo> infos = new();
            for (int j = 0; j < 8; j++)
            {
                vec = new Vector3(BlockGenerationStartPos.x + i * SLIDE_NUM, BlockGenerationStartPos.y, BlockGenerationStartPos.z + j * SLIDE_NUM);
                GameObject gameObject = Instantiate(BlockPrefab, vec, Quaternion.identity);
                StoneInfo stoneInfo = gameObject.GetComponent<StoneInfo>();
                stoneInfo.StonePosition = vec + new Vector3(0f, 1f, 0f);
                infos.Add(stoneInfo);
            }
            StoneInfos.Add(infos);
        }
    }

    /// <summary>
    /// StoneInfos内それぞれのStoneInfoのコネクティングを実行
    /// </summary>
    private void StoneConnecting()
    {
        const int LEFT = -1;
        const int RIGHT = 1;
        const int UP = -1;
        const int DOWN = 1;

        for (int i = 1; i < StoneInfos.Count + 1; i++)
        {
            for (int j = 1; j < StoneInfos[i - 1].Count + 1; j++)
            {
                StoneInfo left = GetStoneInfo(i + LEFT, j);
                StoneInfo right = GetStoneInfo(i + RIGHT, j);
                StoneInfo up = GetStoneInfo(i, j + UP);
                StoneInfo down = GetStoneInfo(i, j + DOWN);
                StoneInfo left_up = GetStoneInfo(i + LEFT, j + UP);
                StoneInfo right_up = GetStoneInfo(i + RIGHT, j + UP);
                StoneInfo left_down = GetStoneInfo(i + LEFT, j + DOWN);
                StoneInfo right_down = GetStoneInfo(i + RIGHT, j + DOWN);
                StoneInfos[i - 1][j - 1].SetConnect(left, right, up, down, left_up, right_up, left_down, right_down);
            }
        }
    }

    private void StartStoneSetting()
    {
        StoneInstance(4, 4, StoneStatus.Black);
        StoneInstance(5, 4, StoneStatus.White);
        StoneInstance(4, 5, StoneStatus.White);
        StoneInstance(5, 5, StoneStatus.Black);
    }

    /// <summary>
    /// 石を生成
    /// </summary>
    /// <param name="stoneStatus">石のステータス</param>
    /// <param name="column_x">横方向に何マス目</param>
    /// <param name="row_z">縦方向に何マス目</param>
    private void StoneInstance(int column_x, int row_z, StoneStatus stoneStatus = StoneStatus.None)
    {

        if (StoneInfos.Count >= column_x && column_x >= 1)
        {

            if (StoneInfos[column_x - 1].Count >= row_z && row_z >= 1)
            {

                if (stoneStatus != StoneStatus.None)
                {
                    bool result = StoneStatusChange(stoneStatus, column_x, row_z);
                    if (result == false)
                    {
                        return;
                    }
                }

                StoneInfo stoneInfo = StoneInfos[column_x - 1][row_z - 1];
                if (stoneInfo.Status == StoneStatus.None)
                {
                    Debug.LogWarning("StoneStatusがNoneなので生成できませんでした。");
                    return;
                }

                stoneInfo.StoneGameObject = Instantiate(StonePrefab, stoneInfo.StonePosition, stoneInfo.StoneQuaternion);
            }
            else
            {
                Debug.LogWarning("row_zの値が適切ではありません");
                return;
            }
        }
        else
        {
            Debug.LogWarning("column_xの値が適切ではありません");
            return;
        }
    }

    public bool StoneStatusChange(StoneStatus stoneStatus, int column_x, int row_z)
    {
        if (StoneInfos.Count >= column_x && column_x >= 1)
        {
            if (StoneInfos[column_x - 1].Count >= row_z && row_z >= 1)
            {
                StoneInfo stoneInfo = StoneInfos[column_x - 1][row_z - 1];
                stoneInfo.Status = stoneStatus;
                return true;
            }
            else
            {
                Debug.LogWarning("row_zの値が適切ではありません");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("column_xの値が適切ではありません");
            return false;
        }
    }

}

/// <summary>
/// <para>ゲームの進行を管理</para>
/// <para>「黒のターン」「白のターン」「石をひっくり返している最中」etc</para>
/// </summary>
public enum GameStatus
{
    Black,
    White,
    StoneChange,
}

/// <summary>
/// コマのステータス管理
/// </summary>
public enum StoneStatus
{
    None,
    Black,
    White,
}

/// <summary>
/// コマを設置可能か否かステータス管理
/// </summary>
public enum PutStonePossibility
{
    Possible_B,
    Possible_W,
    Possible_BW,
    Impossible,
}