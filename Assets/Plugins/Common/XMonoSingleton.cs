using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// X Mono singleton.
/// </summary>
public abstract class XMonoSingleton<T> : MonoBehaviour, ISingleton where T : XMonoSingleton<T> {
    private static T instance = null;

    /// <summary>
    /// 手动初始化，多线程操作时避免子线程中Instance为空
    /// </summary>
    public static void Init() {
        if (instance == null) {
            instance = XSingletonCreator.CreateMonoSingleton<T>();
        }
    }
    public static T Instance {
        get {
            Init();
            return instance;
        }
    }

    /// <summary>
    /// 调用初始化Instance
    /// </summary>
    public virtual void Initialize() {

    }

    /// <summary>
    /// Instance初始化时会调用此函数
    /// </summary>
    public virtual void OnInit() {

    }

    /// <summary>
    /// 销毁
    /// </summary>
    protected virtual void OnDestroy() {
        Dispose();
    }


    /// <summary>
    /// 手动销毁
    /// </summary>
    public void Dispose() {
        instance = null;
        //Destroy(gameObject);
    }

}

/// <summary>
/// X Singleton.
/// </summary>
public abstract class XSingleton<T> : ISingleton where T : XSingleton<T> {
    protected static T instance = null;
    private static object instanceLock = new object();

    protected XSingleton() { }

    /// <summary>
    /// 手动初始化，多线程操作时避免子线程中Instance为空
    /// </summary>
    public static void Init() {
        lock (instanceLock) {
            if (instance == null) {
                instance = XSingletonCreator.CreateSingleton<T>();
            }
        }
    }
    public static T Instance {
        get {
            Init();
            return instance;
        }
    }
    /// <summary>
    /// 调用初始化Instance
    /// </summary>
    public virtual void Initialize() {

    }

    /// <summary>
    /// Instance初始化时会调用此函数
    /// </summary>
    public virtual void OnInit() {
    }

    public void Dispose() {
        instance = null;
    }
}


public class XSingletonCreator {
    public static T CreateSingleton<T>() where T : class, ISingleton {
        if (AppStatus.IsAppQuit) {
            return null;
        }

        T retInstance = default(T);
        // get all non_public constructor
        ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
        // get constructor without parameters
        ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

        if (ctor == null) {
            //Debug.LogWarning("Non-public ctor() not found!");
            ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
        }

        retInstance = ctor.Invoke(null) as T;
        retInstance.OnInit();

        return retInstance;
    }

    public static T CreateMonoSingleton<T>() where T : MonoBehaviour, ISingleton {
        if (AppStatus.IsAppQuit) {
            return null;
        }

        T instance = null;

        if (instance == null && !AppStatus.IsAppQuit) {
            instance = GameObject.FindObjectOfType(typeof(T)) as T;
            if (instance == null) {
                MemberInfo info = typeof(T);
                object[] attributes = info.GetCustomAttributes(true);
                for (int i = 0; i < attributes.Length; ++i) {
                    XMonoSingletonAttribute defineAttri = attributes[i] as XMonoSingletonAttribute;
                    if (defineAttri == null) {
                        continue;
                    }
                    instance = CreateComponentOnGameObject<T>(defineAttri.AbsolutePath, true);
                    break;
                }

                if (instance == null) {
                    GameObject obj = new GameObject(typeof(T).Name);
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                    instance = obj.AddComponent<T>();
                }

                instance.OnInit();
            }else
            {
                UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
            }
        }

        return instance;
    }

    protected static T CreateComponentOnGameObject<T>(string _path, bool _dontDestroy) where T : MonoBehaviour {
        GameObject obj = FindGameObject(null, _path, true, _dontDestroy);
        if (obj == null) {
            obj = new GameObject(typeof(T).Name);
            if (_dontDestroy) {
                UnityEngine.Object.DontDestroyOnLoad(obj);
            }
        }

        return obj.AddComponent<T>();
    }

    static GameObject FindGameObject(GameObject _root, string _path, bool _build, bool _dontDestroy) {
        if (_path == null || _path.Length == 0) {
            return null;
        }

        string[] subPath = _path.Split('/');
        if (subPath == null || subPath.Length == 0) {
            return null;
        }

        return FindGameObject(_root, subPath, 0, _build, _dontDestroy);
    }

    static GameObject FindGameObject(GameObject _root, string[] _subPath, int _index, bool _build, bool _dontDestroy) {
        GameObject client = null;

        if (_root == null) {
            client = GameObject.Find(_subPath[_index]);
        } else {
            var child = _root.transform.Find(_subPath[_index]);
            if (child != null) {
                client = child.gameObject;
            }
        }

        if (client == null) {
            if (_build) {
                client = new GameObject(_subPath[_index]);
                if (_root != null) {
                    client.transform.SetParent(_root.transform);
                }
                if (_dontDestroy && _index == 0) {
                    GameObject.DontDestroyOnLoad(client);
                }
            }
        }

        if (client == null) {
            return null;
        }

        if (++_index == _subPath.Length) {
            return client;
        }

        return FindGameObject(client, _subPath, _index, _build, _dontDestroy);
    }
}
[AttributeUsage(AttributeTargets.Class)]
public class XMonoSingletonAttribute : Attribute {
    private string absolutePath;

    public XMonoSingletonAttribute(string _relativePath) {
        absolutePath = _relativePath;
    }

    public string AbsolutePath {
        get { return absolutePath; }
    }
}

/// <summary>
/// Singleton Interface
/// </summary>
public interface ISingleton {
    void OnInit();
}

public class AppStatus : MonoBehaviour {
    private static bool isApplicationQuit = false;
    public static bool IsAppQuit {
        get {
            return isApplicationQuit;
        }
    }
    void OnApplicationQuit() {
        isApplicationQuit = true;
    }
}