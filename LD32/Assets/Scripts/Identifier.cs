using UnityEngine;

public struct Identifier
{
    public int l;
    public int r;

	public Identifier(int id) { l=r=0; SetID (id); }

	enum Counts
	{
		kNumLeft=4,
		kNumRight=4,
		kNumTypes=kNumLeft*kNumRight,
	};

	public int ID() { return r*(int)Counts.kNumLeft + l; }
	void SetID(int id) { r = id / (int)Counts.kNumRight; l = id - (r*(int)Counts.kNumRight); }
	public int GetColorID() { return r; }
	public int GetMeshID() { return l; }

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

