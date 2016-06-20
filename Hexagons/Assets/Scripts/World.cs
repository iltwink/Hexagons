using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{

	Dictionary<Vector2, Hex> Hexes { get; set; }

	void Start ()
	{
		Hexes = new Dictionary<Vector2, Hex> ();


		for (int i = 0; i < 5; i++)
		{
			var position = new Vector2 (i, 0);
			var pos = ToPixel (position);

			var hex = new GameObject ();
			hex.AddComponent<Hex> ();
			var hm = hex.GetComponent<Hex> ();
			hm.HexPosition = position;
			hex.transform.position = pos;
			hex.transform.parent = transform.FindChild ("HexHolder");
			hm.InitializeModel ();
			this.Hexes.Add (position, hm);
		}
	}

	#region support
	private Vector2 ToHex (Vector3 pos)
	{
		var px = pos.x + Globals.HalfWidth;
		var py = pos.z + Globals.Radius;

		int gridX = (int) Math.Floor (px / Globals.Width);
		int gridY = (int) Math.Floor (py / Globals.RowHeight);

		float gridModX = Math.Abs (px % Globals.Width);
		float gridModY = Math.Abs (py % Globals.RowHeight);

		bool gridTypeA = (gridY % 2) == 0;

		var resultY = gridY;
		var resultX = gridX;
		var m = Globals.ExtraHeight / Globals.HalfWidth;

		if (gridTypeA)
		{
			// middle
			resultY = gridY;
			resultX = gridX;
			// left
			if (gridModY < (Globals.ExtraHeight - gridModX * m))
			{
				resultY = gridY - 1;
				resultX = gridX - 1;
			}
			// right
			else if (gridModY < (-Globals.ExtraHeight + gridModX * m))
			{
				resultY = gridY - 1;
				resultX = gridX;
			}
		}
		else
		{
			if (gridModX >= Globals.HalfWidth)
			{
				if (gridModY < (2 * Globals.ExtraHeight - gridModX * m))
				{
					// Top
					resultY = gridY - 1;
					resultX = gridX;
				}
				else
				{
					// Right
					resultY = gridY;
					resultX = gridX;
				}
			}

			if (gridModX < Globals.HalfWidth)
			{
				if (gridModY < (gridModX * m))
				{
					// Top
					resultY = gridY - 1;
					resultX = gridX;
				}
				else
				{
					// Left
					resultY = gridY;
					resultX = gridX - 1;
				}
			}
		}

		return new Vector3 (resultX, resultY);
	}

	private Vector3 ToPixel (Vector2 hc)
	{
		var x = (hc.x * Globals.Width) + (((int) hc.y & 1) * Globals.Width / 2);
		return new Vector3 (x, 0, (float) (hc.y * 1.5 * Globals.Radius));
	}

	public IEnumerable<Vector2> GetRing (Vector2 hcrd, int ring)
	{
		var left = new Vector2 (hcrd.x - ring, hcrd.y);
		yield return left;

		var tx = left.x;
		var ty = left.y;
		for (var i = 1; i < ring + 1; i++)
		{
			tx = NextX (tx, ty);
			ty = ty + 1;
			yield return new Vector2 (tx, ty);
		}

		var bx = left.x;
		var by = left.y;
		for (var i = 1; i < ring + 1; i++)
		{
			bx = NextX (bx, by);
			by = by - 1;
			yield return new Vector2 (bx, by);
		}

		for (int i = 1; i <= ring; i++)
		{
			yield return new Vector2 (tx + i, ty);
			yield return new Vector2 (bx + i, by);
		}

		tx += ring;
		bx += ring;
		for (var i = 1; i < ring; i++)
		{
			tx = NextX (tx, ty);
			ty = ty - 1;
			yield return new Vector2 (tx, ty);
		}
		for (var i = 1; i < ring; i++)
		{
			bx = NextX (bx, by);
			by = by + 1;
			yield return new Vector2 (bx, by);
		}

		yield return new Vector2 (hcrd.x + ring, hcrd.y);
	}

	private int NextX (float x, float y)
	{
		if (y % 2 == 0)
			return (int) x;
		else
			return (int) (x + 1);
	}
	#endregion
}
