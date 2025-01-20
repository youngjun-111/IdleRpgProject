using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown = -1,
        Title,
        Main,
        Loading
    }

    public enum UIEvent
    {
        Click,
        BeginDrag,
        Drag,
        EndDrag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click
    }


    //public enum Sound
    //{
    //    Bgm,
    //    Effect,
    //    MaxCount    // 아무것도 아니지만 Sound의 갯수를 세기 위해 추가
    //}
}
