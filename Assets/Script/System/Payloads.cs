
using UnityEngine;

public class Payload_LoadScene
{
    public int SceneIdx;
    public bool IsAdditive;
    public Payload_LoadScene(int _SceneIdx, bool _IsAdditive = false)
    {
        SceneIdx = _SceneIdx;
        IsAdditive = _IsAdditive;
    }
}

public class Payload_SetOverlayCamera
{
    public Camera MainCamera;
    public Payload_SetOverlayCamera(Camera _MainCamera)
    {
        MainCamera = _MainCamera;
    }
}

#region MainGameScene
public class Payload_MainGameStart
{
    public int MaxLevel;
    public Payload_MainGameStart() { }
    /// <summary>
    /// 수정 필요
    /// </summary>
    /// <param name="_Level"></param>
    public Payload_MainGameStart(int _Level = 10)
    {
        MaxLevel = _Level;
    }
}

public class Payload_OnPlayerHit
{
    public int Damage;
    public int CurrentHp;
    public Payload_OnPlayerHit(int _Damage)
    {
        Damage = _Damage;
    }
    
}

public class Payload_OnBuildingDestroy
{
    public bool IsLast;
    public float Score;
    public Payload_OnBuildingDestroy(bool _IsLast, float _Score)
    {
        IsLast = _IsLast;
        Score = _Score;
    }
}

public class Payload_GamePause
{
    public bool IsPause;
    public Payload_GamePause(bool _IsPause)
    {
        IsPause = _IsPause;
    }
}


public class Payload_GameOver
{
    public float Time;
    public int Score;
    public Payload_GameOver() { }
    public Payload_GameOver(float _Time, int _Score)
    {
        Time = _Time;
        Score = _Score;
    }
}

#endregion~MainGameScene

