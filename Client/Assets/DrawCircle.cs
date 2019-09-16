using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public Transform m_Transform;
    public float m_Raius = 2;
    public float m_Theta = 0.1f;
    public Color m_color = Color.green;

    private void OnDrawGizmos()
    {
        if (m_Transform == null) return;
        if (m_Theta < 0.0001f) m_Theta = 0.0001f;

        Matrix4x4 de = Gizmos.matrix;
        Gizmos.matrix = m_Transform.localToWorldMatrix;

        Color color = Gizmos.color;
        Gizmos.color = m_color;

        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;

        for (float i = 0; i < 2 * Mathf.PI; i += m_Theta)
        {
            float x = m_Raius * Mathf.Cos(i);
            float z = m_Raius * Mathf.Sin(i);

            Vector3 endPoint = new Vector3(x, 0, z);
            if (i == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Gizmos.DrawLine(beginPoint, endPoint);
            }
            beginPoint = endPoint;
        }

        Gizmos.DrawLine(firstPoint, beginPoint);
        Gizmos.color = color;
        Gizmos.matrix = de;

    }
}

