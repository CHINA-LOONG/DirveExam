using UnityEngine;
using System.Collections;

public class EditerIcon : MonoBehaviour 
{
    public string m_IconName;

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, m_IconName, true);
    }
}
