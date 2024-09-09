using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    private GameObject BlockPrefab = null;
    private AsyncOperationHandle<GameObject> OperationHandle;
    [SerializeField]
    private string BLOCKPREFAB_KEY = "Block";
    [SerializeField]
    private Vector3 BlockGenerationStartPos = Vector3.zero;
    private const float SLIDE_NUM = 9.5f;
    private List<List<StoneInfo>> StoneInfos = new();
    public StoneInfo GetStoneInfo(int column_x = 0, int row_z = 0)
    {
        if (StoneInfos.Count == 0) { return null; }
        if (StoneInfos.Count - 1 >= column_x && column_x >= 0)
        {
            if (StoneInfos[column_x].Count >= row_z && row_z >= 0)
            {
                return StoneInfos[column_x][row_z];
            }
        }
        return null;
    }

    public GameStatus GameStatus { get; private set; } = GameStatus.Black;


    // Start is called before the first frame update
    async void Start()
    {
        OperationHandle = Addressables.LoadAssetAsync<GameObject>(BLOCKPREFAB_KEY);
        await OperationHandle.Task;
        BlockPrefab = OperationHandle.Result;
        Addressables.Release(OperationHandle);

        InstantiateBlocks();
        StoneConnecting();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InstantiateBlocks()
    {
        Vector3 vec = Vector3.zero;
        for (int i = 0; i < 8; i++)
        {
            List<StoneInfo> infos = new();
            for (int j = 0; j < 8; j++)
            {
                vec = new Vector3(BlockGenerationStartPos.x + i * SLIDE_NUM, BlockGenerationStartPos.y, BlockGenerationStartPos.z + j * SLIDE_NUM);
                GameObject gameObject = Instantiate(BlockPrefab, vec, Quaternion.identity);
                infos.Add(gameObject.GetComponent<StoneInfo>());
            }
            StoneInfos.Add(infos);
        }
        Debug.Log($"StoneInfos.Count={StoneInfos.Count}");
    }

    void StoneConnecting()
    {
        const int LEFT = -1;
        const int RIGHT = 1;
        const int UP = -1;
        const int DOWN = 1;

        for (int i = 0; i < StoneInfos.Count; i++)
        {
            for (int j = 0; j < StoneInfos[i].Count; j++)
            {
                StoneInfo left = GetStoneInfo(i + LEFT, j);
                StoneInfo right = GetStoneInfo(i + RIGHT, j);
                StoneInfo up = GetStoneInfo(i, j + UP);
                StoneInfo down = GetStoneInfo(i, j + DOWN);
                StoneInfo left_up = GetStoneInfo(i + LEFT, j + UP);
                StoneInfo right_up = GetStoneInfo(i + RIGHT, j + UP);
                StoneInfo left_down = GetStoneInfo(i + LEFT, j + DOWN);
                StoneInfo right_down = GetStoneInfo(i + RIGHT, j + DOWN);
                StoneInfos[i][j].SetConnect(left, right, up, down, left_up, right_up, left_down, right_down);
            }
        }

    }
}

// ゲームの進行状態を管理
public enum GameStatus
{
    Black,
    White,
    Change,
}

// コマの状態
public enum StoneStatus
{
    None,
    Black,
    White,
}

// コマを設置可能か否か
public enum PutStonePossibility
{
    Possible_B,
    Possible_W,
    Possible_BW,
    Impossible,
}