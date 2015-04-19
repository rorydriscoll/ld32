﻿using UnityEngine;

public struct Identifier
{
	public const int kNumCreatures = 3;
	public const int kNumUndead = 2;
	public const int kNumTypes = kNumCreatures + kNumUndead;
	public const int kNumPermutations = kNumCreatures * kNumUndead;

	public int l;
	public int r;

	public int ColorID
	{
		get { return r; }
	}

	public int MeshID
	{
		get { return l; }
	}

	public bool IsValid
	{
		get { return l >= 0 && l < kNumCreatures && r >= 0 && r < kNumUndead; }
	}

	public int ID
	{
		get
		{
			return l * kNumCreatures + r;
		}
		set
		{
            l = value / kNumUndead; r = value % kNumUndead;
		}
	}

	public Identifier(int id) { l = r = -1; ID = id; }
	public Identifier(int l_, int r_) { l = l_; r = r_; }

	public static Identifier Invalid
	{
		get { return new Identifier(-1, -1); }
	}
}

