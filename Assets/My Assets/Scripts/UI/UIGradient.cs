using UnityEngine;
using UnityEngine.UI;

public class UIGradient : BaseMeshEffect
{
    public Color m_color1;
    public Color m_color2;
    public Color thing1;
    public Color thing2;
    [Range(-180f, 180f)]
    public float m_angle;
    public bool m_ignoreRatio;

    void Update()
    {
        thing2 = new Color(thing1.r + 0.0001f, thing1.g, thing1.b);
        if (m_color2 == thing1)
        {
            m_color2 = thing2;
        }
        else
        {
            m_color2 = thing1;
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        Rect rect = graphic.rectTransform.rect;
        Vector2 dir = UIGradientUtils.RotationDir(m_angle);

        if (!m_ignoreRatio)
        {
            dir = UIGradientUtils.CompensateAspectRatio(rect, dir);
        }

        UIGradientUtils.Matrix2x3 localPositionMatrix = UIGradientUtils.LocalPositionMatrix(rect, dir);

        UIVertex vertex = default;

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            Vector2 localPosition = localPositionMatrix * vertex.position;
            vertex.color = Color.Lerp(m_color2, m_color1, localPosition.y);
            vh.SetUIVertex(vertex, i);
        }
    }
}
