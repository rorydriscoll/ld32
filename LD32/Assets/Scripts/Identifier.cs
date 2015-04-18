using UnityEngine;

public struct Identifier
{
    public int l;
    public int r;

	public Identifier(int id) { l=r=0; SetID (id); }

	enum Counts
	{
		kNumLeft=3,
		kNumRight=4,
		kNumTypes=kNumLeft*kNumRight,
	};
	public int ID() { return l*(int)Counts.kNumLeft + r; }
	void SetID(int id) { l = id / (int)Counts.kNumLeft; r = id - (l*(int)Counts.kNumLeft); }
    public Color GetColor()
    {
        Color[] colors =
        {
            new Color(1, 0, 0, 1),
            new Color(0, 1, 0, 1),
            new Color(0, 0, 1, 1),
			new Color(1, 1, 0, 1),
		};

        return colors[r];
    }
}

