
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/TextSpacing1")]
public class TextSpacing1 : BaseMeshEffect
{
    public float TextHorizontalSpacing = 1f;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0)
        {
            return;
        }

        Text text = GetComponent<Text>();
        if (text == null)
        {
            Debug.LogError("Missing Text component");
            return;
        }

        List<UIVertex> vertexs = new List<UIVertex>();
        vh.GetUIVertexStream(vertexs);
        int indexCount = vh.currentIndexCount;

        string[] lineTexts = text.text.Split('\n');

        Line[] lines = new Line[lineTexts.Length];

        //����lines�����и���Ԫ�صĳ��ȼ���ÿһ���е�һ�����������ÿ���֡���ĸ����ĸ��ռ6����
        for (int i = 0; i < lines.Length; i++)
        {
            //�����һ���⣬vertexs����ǰ�漸�ж��лس���ռ��6����
            if (i == 0)
            {
                lines[i] = new Line(0, lineTexts[i].Length + 1);
            }
            else if (i > 0 && i < lines.Length - 1)
            {
                lines[i] = new Line(lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length + 1);
            }
            else
            {
                lines[i] = new Line(lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length);
            }
        }

        UIVertex vt;

        for (int i = 0; i < lines.Length; i++)
        {
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;
            Vector3 defaultStartPos = Vector3.zero;
            Vector3 defaultEndPos = Vector3.zero;
            for (int j = lines[i].StartVertexIndex; j <= lines[i].EndVertexIndex; j++)
            {
                if (j < 0 || j >= vertexs.Count)
                {
                    continue;
                }
                vt = vertexs[j];
                if (defaultStartPos == Vector3.zero)
                {
                    defaultStartPos = new Vector3(vt.position.x, vt.position.y, vt.position.z);
                }
                defaultEndPos = new Vector3(vt.position.x, vt.position.y, vt.position.z);
                if (j != 0)
                {
                    vt.position += new Vector3(TextHorizontalSpacing * ((j - lines[i].StartVertexIndex) / 6), 0, 0);
                }
                if (startPos == Vector3.zero)
                {
                    startPos = new Vector3(vt.position.x, vt.position.y, vt.position.z);
                }
                endPos = new Vector3(vt.position.x, vt.position.y, vt.position.z);
                vertexs[j] = vt;
                //����ע����������Ķ�Ӧ��ϵ
                if (j % 6 <= 2)
                {
                    vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
                }
                if (j % 6 == 4)
                {
                    vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                }
            }
            if (text.alignment == TextAnchor.MiddleCenter || text.alignment == TextAnchor.UpperCenter || text.alignment == TextAnchor.LowerCenter)
            {
                Vector3 defaultCenterPos = defaultStartPos + (defaultEndPos - defaultStartPos) / 2;
                Vector3 centerPos = startPos + (endPos - startPos) / 2;
                for (int j = lines[i].StartVertexIndex; j <= lines[i].EndVertexIndex; j++)
                {
                    if (j < 0 || j >= vertexs.Count)
                    {
                        continue;
                    }
                    vt = vertexs[j];
                    vt.position = vt.position + defaultCenterPos - centerPos;
                    vertexs[j] = vt;
                    //����ע����������Ķ�Ӧ��ϵ
                    if (j % 6 <= 2)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
                    }
                    if (j % 6 == 4)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                    }
                }
            }
            if (text.alignment == TextAnchor.MiddleRight || text.alignment == TextAnchor.UpperRight || text.alignment == TextAnchor.LowerRight)
            {
                Vector3 defaultRightPos = defaultEndPos;
                Vector3 rightPos = endPos;
                for (int j = lines[i].StartVertexIndex; j <= lines[i].EndVertexIndex; j++)
                {
                    if (j < 0 || j >= vertexs.Count)
                    {
                        continue;
                    }
                    vt = vertexs[j];
                    vt.position = vt.position + defaultRightPos - rightPos;
                    vertexs[j] = vt;
                    //����ע����������Ķ�Ӧ��ϵ
                    if (j % 6 <= 2)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
                    }
                    if (j % 6 == 4)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                    }
                }
            }
        }
    }
}

public class Line
{

    private int _startVertexIndex = 0;
    /// <summary>
    /// �������
    /// </summary>
    public int StartVertexIndex
    {
        get
        {
            return _startVertexIndex;
        }
    }

    private int _endVertexIndex = 0;
    /// <summary>
    /// �յ�����
    /// </summary>
    public int EndVertexIndex
    {
        get
        {
            return _endVertexIndex;
        }
    }

    private int _vertexCount = 0;
    /// <summary>
    /// ����ռ�ĵ���Ŀ
    /// </summary>
    public int VertexCount
    {
        get
        {
            return _vertexCount;
        }
    }

    public Line(int startVertexIndex, int length)
    {
        _startVertexIndex = startVertexIndex;
        _endVertexIndex = length * 6 - 1 + startVertexIndex;
        _vertexCount = length * 6;
    }
}