using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StoneInfo : MonoBehaviour
{
    public GameObject StonePrefab { get; private set; } = null;
    private AsyncOperationHandle<GameObject> OperationHandle;
    [SerializeField]
    private string STONEPREFAB_KEY = "Stone";
    public StoneStatus Status { get; set; } = StoneStatus.None;
    public PutStonePossibility PutPossibility { get; set; } = PutStonePossibility.Impossible;

    public Vector3 StonePosition { get; private set; } = Vector3.zero;

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
    private async void Start()
    {
        OperationHandle = Addressables.LoadAssetAsync<GameObject>(STONEPREFAB_KEY);
        await OperationHandle.Task;
        StonePrefab = OperationHandle.Result;
        Addressables.Release(OperationHandle);

        StonePosition = transform.position + new Vector3(0f, 0.5f, 0f);
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


}
