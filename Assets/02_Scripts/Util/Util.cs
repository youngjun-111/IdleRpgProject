using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Util
{
    // 게임오브젝트용 미리 만들어둔 랩핑
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
                            // T FindChild<T>
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
        {
            return null;
        }
        return transform.gameObject;
    }

    // 최상위 부모, 이름은 비교하지 않고 그 타입에만 해당하면 리턴 ( 컴퍼넌트 이름 ),
    // 재귀적으로 사용, 자식만 찾을건지 자식의 자식도 찾을 건지
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false)
        where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false) // 직속 자식만
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else    // 자식의 자식까지
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {   // 이름이 비어있거나 내가 찾으려는 이름과 같다면
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    // 게임오브젝트(go)에 해당 컴포넌트가 없으면 T 컴포넌트 추가
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }

        return component;
    }

    public static bool ChackFOV(Transform player, Transform target, int angle, int radius)
    {

        // 시야각이 90도일 때, 45도
        Vector3 rightDir = AngleToDir(angle * 0.5f);
        
        // 시야각이 90도일 때, -45도
        Vector3 leftDir = AngleToDir(angle * -1 * 0.5f);

        // 연산을 빠르게 처리하기 위해 제곱된 값을 구한다.
        float sqrDistance = Vector3.SqrMagnitude(target.position - player.position);
        if (sqrDistance < radius * radius &&
            GetAngle(player.position, target.position) < angle * 0.5f)
        {
            return true;
        }
        else { return false; }
    }

    static Vector3 AngleToDir(float angle)
    {
        // UnityEngine.Mathf 의 Sin, Cos 에 들어가는 파라미터 값은 라디안이다.
        // 그러므로, 라디안으로 바꿔줘야 한다. 
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }

    static float GetAngle(Vector3 origin, Vector3 target)
    {
        Vector3 direction = target.normalized - origin.normalized;

        /**
         * z가 전방이기 때문에
         * y 파라미터 : x
         * x 파라미터 : z
         * 값이 들어간다.
         */
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }
    public static void BindUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            case Define.UIEvent.BeginDrag:
                evt.OnBeginDragHandler -= action;
                evt.OnBeginDragHandler += action;
                break;
            case Define.UIEvent.EndDrag:
                evt.OnEndDragHandler -= action;
                evt.OnEndDragHandler += action;
                break;
        }
    }
}
