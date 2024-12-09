using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject StonePrefab = null;
    [SerializeField]
    private GameObject BlockPrefab = null;
    [SerializeField]
    private Vector3 BlockGenerationStartPos = Vector3.zero;
    private const float SLIDE_NUM = 9.5f;
    private List<List<StoneInfo>> StoneInfos = new List<List<StoneInfo>>();
    public StoneInfo GetStoneInfo(int column_x = 1, int row_z = 1)
    {
        if (StoneInfos.Count == 0) { return null; }
        string result = GridCheck(column_x, row_z);
        if (result == "OK")
        {
            return StoneInfos[column_x - 1][row_z - 1];
        }
        else
        {
            return null;
        }
    }

    public static GameStatus GameStatusData { get; private set; } = GameStatus.Black;
    private GameStatus NextTurn { get; set; } = GameStatus.White;

    // Start is called before the first frame update
    private void Start()
    {
        InstantiateBlocks();
        StoneConnecting();
        StartStoneSetting();
        StonesPutPossibilityUpdate();
        Debug.Log($"最初は{GameManager.GameStatusData}のターンからです!");

        StartCoroutine(GameUpdate());

    }

    // Update is called once per frame
    private void Update()
    {


    }

    private IEnumerator GameUpdate()
    {
        while (true)
        {
            if (GameManager.GameStatusData == GameStatus.PutPossibilityUpdateing)
            {
                StonesPutPossibilityUpdate();
                GameManager.GameStatusData = NextTurn;
                if (NextTurn == GameStatus.Black)
                {
                    NextTurn = GameStatus.White;
                }
                else
                {
                    NextTurn = GameStatus.Black;
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    StoneInfo stoneInfo = hit.transform.gameObject.GetComponent<StoneInfo>();
                    if (stoneInfo != null && stoneInfo.PutPossibility != PutStonePossibility.Impossible)
                    {
                        if (GameManager.GameStatusData == GameStatus.Black)
                        {
                            GameManager.GameStatusData = GameStatus.StoneChange;
                            StoneInstance(stoneInfo, StoneStatus.Black);
                            yield return stoneInfo.StonesReverse(StoneStatus.Black);
                        }
                        else
                        {
                            GameManager.GameStatusData = GameStatus.StoneChange;
                            StoneInstance(stoneInfo, StoneStatus.White);
                            yield return stoneInfo.StonesReverse(StoneStatus.White);
                        }
                    }

                    GameManager.GameStatusData = GameStatus.PutPossibilityUpdateing;
                }
            }


            yield return null;
        }
    }

    /// <summary>
    /// colum_xとrow_zが適切な数値化確認する関数
    /// </summary>
    /// <param name="column_x">横方向に何マス目</param>
    /// <param name="row_z">縦方向に何マス目</param>
    /// <returns> 
    /// リターンパターン
    /// <list type="bullet">
    /// <item>OK</item>
    /// <item>row_zの値が適切ではありません</item>
    /// <item>column_xの値が適切ではありません</item>
    /// </list>
    /// </returns>
    private string GridCheck(int column_x = 1, int row_z = 1)
    {

        if (StoneInfos.Count >= column_x && column_x >= 1)
        {
            if (StoneInfos[column_x - 1].Count >= row_z && row_z >= 1)
            {
                return "OK";
            }
            else
            {
                return "row_zの値が適切ではありません";
            }
        }
        else
        {
            return "column_xの値が適切ではありません";
        }
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
                stoneInfo.GameManager = this;
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

    /// <summary>
    /// 最初の4つ、石を生成するプログラム
    /// </summary>
    private void StartStoneSetting()
    {
        StoneInstance(GetStoneInfo(4, 4), StoneStatus.Black);
        StoneInstance(GetStoneInfo(5, 4), StoneStatus.White);
        StoneInstance(GetStoneInfo(4, 5), StoneStatus.White);
        StoneInstance(GetStoneInfo(5, 5), StoneStatus.Black);
    }

    /// <summary>
    /// 石を生成
    /// </summary>
    /// <param name="stoneInfo">石の情報</param>
    /// <param name="stoneStatus">石のステータス</param>
    private void StoneInstance(StoneInfo stoneInfo, StoneStatus stoneStatus = StoneStatus.None)
    {
        if (stoneInfo != null && stoneStatus != StoneStatus.None)
        {
            stoneInfo.StatusChange(stoneStatus);
            stoneInfo.StoneGameObject = Instantiate(StonePrefab, stoneInfo.StonePosition, stoneInfo.StoneQuaternion);
        }
    }

    /// <summary>
    /// 石を置けるか確認
    /// </summary>
    public void StonesPutPossibilityUpdate()
    {
        foreach (List<StoneInfo> Infos in StoneInfos)
        {
            foreach (StoneInfo info in Infos)
            {
                info.PutPossibilityUpdate();
            }
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
    PutPossibilityUpdateing,
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