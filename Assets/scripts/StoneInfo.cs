using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoneInfo : MonoBehaviour
{
    public GameObject StoneGameObject { private get; set; } = null;
    public GameManager GameManager { private get; set; } = null;

    private StoneStatus Status { get; set; } = StoneStatus.None;
    public PutStonePossibility PutPossibility { get; set; } = PutStonePossibility.Impossible;
    public List<StoneInfo> ChangeStoneList
    {
        get
        {
            if (GameManager.GameStatusData == GameStatus.Black)
            {
                return blackChangeStoneList;
            }
            else if (GameManager.GameStatusData == GameStatus.White)
            {
                return whiteChangeStoneList;
            }
            else
            {
                return new List<StoneInfo>();
            }
        }
    }
    private List<StoneInfo> blackChangeStoneList = new List<StoneInfo>();
    private List<StoneInfo> whiteChangeStoneList = new List<StoneInfo>();

    public Vector3 StonePosition { get; set; } = Vector3.zero;

    public Quaternion StoneQuaternion
    {
        get
        {
            if (Status == StoneStatus.White)
            {
                return Quaternion.Euler(180f, 0, 0);
            }
            else
            {
                return Quaternion.identity;
            }
        }
    }

    private StoneInfo LeftStone { get; set; } = null;
    private StoneInfo RightStone { get; set; } = null;
    private StoneInfo UpStone { get; set; } = null;
    private StoneInfo DownStone { get; set; } = null;
    private StoneInfo LeftUpStone { get; set; } = null;
    private StoneInfo RightUpStone { get; set; } = null;
    private StoneInfo LeftDownStone { get; set; } = null;
    private StoneInfo RightDownStone { get; set; } = null;

    private float ReversiElapsedTime;
    private const float REVERSI_TIME = 2f;

    public bool IsLeftConnected()
    {
        return UpStone != null;
    }
    public bool IsRightConnected()
    {
        return RightStone != null;
    }

    public bool IsUpConnected()
    {
        return UpStone != null;
    }

    public bool IsDownConnected()
    {
        return DownStone != null;
    }

    public bool IsLeftUpConnected()
    {
        return LeftUpStone != null;
    }

    public bool IsRightUpConnected()
    {
        return RightUpStone != null;
    }

    public bool IsLeftDownConnected()
    {
        return LeftDownStone != null;
    }

    public bool IsRightDownConnected()
    {
        return RightDownStone != null;
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    /// <summary>
    /// 隣接するマスのStoneInfoデータを設定
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="right">右</param>
    /// <param name="up">上</param>
    /// <param name="down">下</param>
    /// <param name="leftup">左上</param>
    /// <param name="rightup">右上</param>
    /// <param name="leftdown">左下</param>
    /// <param name="rightdown">右下</param>
    public void SetConnect(StoneInfo left, StoneInfo right, StoneInfo up, StoneInfo down, StoneInfo leftup, StoneInfo rightup, StoneInfo leftdown, StoneInfo rightdown)
    {
        if (left != null)
        {
            LeftStone = left;
        }
        if (right != null)
        {
            RightStone = right;
        }
        if (up != null)
        {
            UpStone = up;
        }
        if (down != null)
        {
            DownStone = down;
        }
        if (leftup != null)
        {
            LeftUpStone = leftup;
        }
        if (rightup != null)
        {
            RightUpStone = rightup;
        }
        if (leftdown != null)
        {
            LeftDownStone = leftdown;
        }
        if (rightdown != null)
        {
            RightDownStone = rightdown;
        }
    }

    /// <summary>
    /// 石のステータスを更新するプログラム
    /// </summary>
    /// <param name="stoneStatus">石のステータス</param>
    public void StatusChange(StoneStatus stoneStatus)
    {
        Status = stoneStatus;
    }

    /// <summary>
    /// 指定したStoneSatusに応じて、ひっくり返せるコマのリストを取得する
    /// </summary>
    /// <param name="putStoneStatus">マスに置きたい石の状態</param>
    /// <returns></returns>
    private List<StoneInfo> GetReversibleStones(StoneStatus putStoneStatus)
    {
        List<StoneInfo> returnInfos = new List<StoneInfo>();
        if (Status == StoneStatus.None)
        {
            List<StoneInfo> infos = new List<StoneInfo>();

            //左方向のチェック
            StoneInfo info = LeftStone;
            while (info != null)
            {
                if (info.Status == StoneStatus.None)
                {
                    infos.Clear();
                    break;
                }
                if (info.Status == putStoneStatus)
                {
                    if (infos.Count > 0)
                    {
                        returnInfos.AddRange(infos);
                    }
                    infos.Clear();
                    break;
                }
                infos.Add(info);
                info = info.LeftStone;
            }

            //右方向のチェック
            info = RightStone;
            while (info != null)
            {
                if (info.Status == StoneStatus.None)
                {
                    infos.Clear();
                    break;
                }
                if (info.Status == putStoneStatus)
                {
                    if (infos.Count > 0)
                    {
                        returnInfos.AddRange(infos);
                    }
                    infos.Clear();
                    break;
                }
                infos.Add(info);
                info = info.RightStone;
            }

            //上方向のチェック
            info = UpStone;
            while (info != null)
            {
                if (info.Status == StoneStatus.None)
                {
                    infos.Clear();
                    break;
                }
                if (info.Status == putStoneStatus)
                {
                    if (infos.Count > 0)
                    {
                        returnInfos.AddRange(infos);
                    }
                    infos.Clear();
                    break;
                }
                infos.Add(info);
                info = info.UpStone;
            }

            //下方向のチェック
            info = DownStone;
            while (info != null)
            {
                if (info.Status == StoneStatus.None)
                {
                    infos.Clear();
                    break;
                }
                if (info.Status == putStoneStatus)
                {
                    if (infos.Count > 0)
                    {
                        returnInfos.AddRange(infos);
                    }
                    infos.Clear();
                    break;
                }
                infos.Add(info);
                info = info.DownStone;
            }

            //左上方向のチェック
            info = LeftUpStone;
            while (info != null)
            {
                if (info.Status == StoneStatus.None)
                {
                    infos.Clear();
                    break;
                }
                if (info.Status == putStoneStatus)
                {
                    if (infos.Count > 0)
                    {
                        returnInfos.AddRange(infos);
                    }
                    infos.Clear();
                    break;
                }
                infos.Add(info);
                info = info.LeftUpStone;
            }

            //右上方向のチェック
            info = RightUpStone;
            while (info != null)
            {
                if (info.Status == StoneStatus.None)
                {
                    infos.Clear();
                    break;
                }
                if (info.Status == putStoneStatus)
                {
                    if (infos.Count > 0)
                    {
                        returnInfos.AddRange(infos);
                    }
                    infos.Clear();
                    break;
                }
                infos.Add(info);
                info = info.RightUpStone;
            }

            //左下方向のチェック
            info = LeftDownStone;
            while (info != null)
            {
                if (info.Status == StoneStatus.None)
                {
                    infos.Clear();
                    break;
                }
                if (info.Status == putStoneStatus)
                {
                    if (infos.Count > 0)
                    {
                        returnInfos.AddRange(infos);
                    }
                    infos.Clear();
                    break;
                }
                infos.Add(info);
                info = info.LeftDownStone;
            }

            //右下方向のチェック
            info = RightDownStone;
            while (info != null)
            {
                if (info.Status == StoneStatus.None)
                {
                    infos.Clear();
                    break;
                }
                if (info.Status == putStoneStatus)
                {
                    if (infos.Count > 0)
                    {
                        returnInfos.AddRange(infos);
                    }
                    infos.Clear();
                    break;
                }
                infos.Add(info);
                info = info.RightDownStone;
            }
        }

        return returnInfos;

    }

    /// <summary>
    /// PutPossibilityの状態を更新する
    /// </summary>
    public void PutPossibilityUpdate()
    {
        blackChangeStoneList = GetReversibleStones(StoneStatus.Black);
        whiteChangeStoneList = GetReversibleStones(StoneStatus.White);

        if (blackChangeStoneList.Count > 0 && whiteChangeStoneList.Count > 0)
        {
            PutPossibility = PutStonePossibility.Possible_BW;
        }
        else if (blackChangeStoneList.Count > 0)
        {
            PutPossibility = PutStonePossibility.Possible_B;
        }
        else if (whiteChangeStoneList.Count > 0)
        {
            PutPossibility = PutStonePossibility.Possible_W;
        }
        else
        {
            PutPossibility = PutStonePossibility.Impossible;
        }
    }

    /// <summary>
    /// stoneをREVERSI_TIMEの秒数で回転させるコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator StoneRotate()
    {
        while (ReversiElapsedTime < REVERSI_TIME)
        {
            Quaternion startQuaternion = StoneGameObject.transform.rotation;
            StoneGameObject.transform.rotation = Quaternion.Slerp(startQuaternion, StoneQuaternion, ReversiElapsedTime / REVERSI_TIME);
            yield return null;
            ReversiElapsedTime += Time.deltaTime;
        }
        ReversiElapsedTime = 0f;
    }

    /// <summary>
    /// ひっくり返せる石をすべてひっくり返すコルーチン
    /// </summary>
    public IEnumerator StonesReverse(StoneStatus changeStatus)
    {
        Debug.Log(ChangeStoneList.Count);
        ChangeStoneList.ForEach(t => { Debug.Log(t.StonePosition); });
        foreach (StoneInfo stoneInfo in ChangeStoneList)
        {
            stoneInfo.StatusChange(changeStatus);
            yield return null;
        }
        List<Coroutine> StoneRotateCoroutineList = new List<Coroutine>();
        foreach (StoneInfo stoneInfo in ChangeStoneList)
        {
            StoneRotateCoroutineList.Add(StartCoroutine(stoneInfo.StoneRotate()));
            yield return null;
        }
        foreach (Coroutine stoneRotateCoroutine in StoneRotateCoroutineList)
        {
            yield return stoneRotateCoroutine;
        }
    }



}
