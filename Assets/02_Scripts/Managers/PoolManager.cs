using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager
{
    Transform _root;

    // ���� Ǯ���� �̸�(string)�̶�� Ű�� �̿��ؼ� ����
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();

    #region Pool
    // Ǯ�Ŵ����� �������� Ǯ���� ���� ����
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;

            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            if (_poolStack.Count > 0)
            {
                poolable = _poolStack.Pop();
            }
            else
            {
                poolable = Create();
            }

            poolable.gameObject.SetActive (true);

            // DontDestroyOnLoad ���� �뵵
            if (parent == null)
            {
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;
            }

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion



    public void Init()
    {
        if ( _root == null)
        {
            // Ǯ���� �� ������Ʈ�� �ִٸ� @Pool_Root ���Ͽ� ��� �ְ� �� ����
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (!_pool.ContainsKey(original.name))
        {
            CreatePool(original);
        }

        return _pool[original.name].Pop(parent);
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public GameObject GetOriginal(string name)
    {
        if (!_pool.ContainsKey(name))
        {
            return null;
        }

        return _pool[name].Original;
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (!_pool.ContainsKey(name))
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public void Clear()
    {
        foreach(Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }

        _pool.Clear();
    }
}
