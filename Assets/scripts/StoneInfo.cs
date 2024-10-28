using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneInfo : MonoBehaviour
{
    public GameObject StoneGameObject { private get; set; } = null;

    public StoneStatus Status { get; set; } = StoneStatus.None;
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

    public StoneInfo LeftStone { get; private set; } = null;
    public StoneInfo RightStone { get; private set; } = null;
    public StoneInfo UpStone { get; private set; } = null;
    public StoneInfo DownStone { get; private set; } = null;
    public StoneInfo LeftUpStone { get; private set; } = null;
    public StoneInfo RightUpStone { get; private set; } = null;
    public StoneInfo LeftDownStone { get; private set; } = null;
    public StoneInfo RightDownStone { get; private set; } = null;

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

    public void PutStonePossibilityCheck(StoneStatus putStoneStatus,List<StoneInfo> saveStoneInfos)
    {
        if (Status == StoneStatus.None)
        {
            List<StoneInfo> infos = new List<StoneInfo>();
            StoneInfo info = null;

            //左方向のチェック
            info = LeftStone;
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
                        saveStoneInfos.AddRange(infos);
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
                        saveStoneInfos.AddRange(infos);
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
                        saveStoneInfos.AddRange(infos);
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
                        saveStoneInfos.AddRange(infos);
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
                        saveStoneInfos.AddRange(infos);
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
                        saveStoneInfos.AddRange(infos);
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
                        saveStoneInfos.AddRange(infos);
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
                        saveStoneInfos.AddRange(infos);
                    }
                    infos.Clear();
                    break;
                }
                infos.Add(info);
                info = info.RightDownStone;
            }




        }
        else
        {
            PutPossibility = PutStonePossibility.Impossible;
        }
    }


}
