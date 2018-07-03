using UnityEngine;
using System;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T m_Instance = null;
	private static bool applicationQuited = false;
	private bool inited = false;
    public static T Instance
    {
        get
        {
			if (applicationQuited)
			{
				Debug.LogWarning("Call Instance of an Singleton on application quit.");
				return null;
			}
            // Instance requiered for the first time, we look for it
            if( m_Instance == null)
            {
                m_Instance = FindObjectOfType(typeof(T)) as T;

                // Object not found, we create a temporary one
                if( m_Instance == null )
                {
                    m_Instance = new GameObject("_" + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    // Problem during the creation, this should not happen
                    if( m_Instance == null )
                    {
                        Debug.LogError("Problem during the creation of " + typeof(T).ToString());
						return null;
                    }
					
					AddToMonoParent("_MonoSingletons", m_Instance);
                }
				
				if (!m_Instance.inited)
				{
                	m_Instance.Init();
				}
            }
            return m_Instance;
        }
    }
    // If no other monobehaviour request the instance in an awake function
    // executing before this one, no need to search the object.
    private void Awake()
    {	
		if (applicationQuited)
		{
			return;
		}
		
		if (!inited)
		{
			inited = true;
	        if( m_Instance == null )
	        {
	            m_Instance = this as T;
	            m_Instance.Init();
	        }
		}
		else
		{
			Destroy(this);
		}
    }
	
	private static GameObject GetMonoParent(String name)
	{
		GameObject parent = GameObject.Find(name);
		
		if (parent == null)
		{
			parent = new GameObject(name);
		}
		
		return parent;
	}
	
	private static void AddToMonoParent(String name, MonoBehaviour obj)
	{
		GameObject parent = GetMonoParent(name);
		obj.transform.parent = parent.transform;
	}


    // This function is called when the instance is used the first time
    // Put all the initializations you need here, as you would do in Awake
    public virtual void Init(){}
	
	private void OnDestory()
	{
		
	}
	
    // Make sure the instance isn't referenced anymore when the user quit, just in case.
    private void OnApplicationQuit()
    {
		applicationQuited = true;
        m_Instance = null;
    }
}