using UnityEngine;

public struct Identifier
{
    public int l;
    public int r;

    public Color GetColor()
    {
        Color[] colors =
        {
            new Color(1, 1, 1, 1),
            new Color(1, 0, 0, 1),
            new Color(0, 1, 0, 1),
            new Color(0, 0, 1, 1),
        };

        return colors[r];
    }
}

