using UnityEngine;

public struct Identifier
{
    public const int kNumLeft = 3;
    public const int kNumRight = 3;
    public const int kNumTypes = kNumLeft + kNumRight;
    public const int kNumPermutations = kNumLeft * kNumRight;

    public int l;
    public int r;

	enum Counts
	{
		kNumLeft=4,
		kNumRight=4,
		kNumTypes=kNumLeft*kNumRight,
	};
	public int GetColorID() { return r; }
	public int GetMeshID() { return l; }

    public bool IsValid
    {
        get
        {
            return l >= 0 && l < kNumLeft && r >= 0 && r < kNumRight;
        }
    }

    public int ID
    {
        get
        {
            return l * kNumLeft + r;
        }
        set
        {
            l = value / kNumLeft; r = value - (l * kNumLeft);
        }
    }

	public Color Color
    {
        get
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

    public Identifier(int id) { l = r = -1; ID = id; }
    public Identifier(int l_, int r_) { l = l_; r = r_; }

    public static Identifier Invalid
    {
        get { return new Identifier(-1, -1); }
    }
}

