using UnityEngine;

public struct Identifier
{
    public int l;
    public int r;

    public Color GetColor()
    {
        int index = (int)l + (int)r * 4;

        Color[] colors =
        {
            new Color(1, 1, 1, 1),
            new Color(1, 0, 0, 1),
            new Color(0, 1, 0, 1),
            new Color(0, 0, 1, 1),

            new Color(0.75f, 0.75f, 0.75f, 1),
            new Color(0.75f, 0, 0, 1),
            new Color(0, 0.75f, 0, 1),
            new Color(0, 0, 0.75f, 1),

            new Color(0.5f, 0.5f, 0.5f, 1),
            new Color(0.5f, 0, 0, 1),
            new Color(0, 0.5f, 0, 1),
            new Color(0, 0, 0.5f, 1),

            new Color(0.25f, 0.25f, 0.25f, 1),
            new Color(0.25f, 0, 0, 1),
            new Color(0, 0.25f, 0, 1),
            new Color(0, 0, 0.25f, 1),
        };

        return colors[index];
    }
}

