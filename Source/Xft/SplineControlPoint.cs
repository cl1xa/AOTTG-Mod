using UnityEngine;

namespace Xft;

public class SplineControlPoint
{
	public int ControlPointIndex = -1;

	public float Dist;

	protected Spline mSpline;

	public Vector3 Normal;

	public Vector3 Position;

	public int SegmentIndex = -1;

	public bool IsValid => NextControlPoint != null;

	public SplineControlPoint NextControlPoint => mSpline.NextControlPoint(this);

	public Vector3 NextNormal => mSpline.NextNormal(this);

	public Vector3 NextPosition => mSpline.NextPosition(this);

	public SplineControlPoint PreviousControlPoint => mSpline.PreviousControlPoint(this);

	public Vector3 PreviousNormal => mSpline.PreviousNormal(this);

	public Vector3 PreviousPosition => mSpline.PreviousPosition(this);

	private Vector3 GetNext2Normal()
	{
		return NextControlPoint?.NextNormal ?? Normal;
	}

	private Vector3 GetNext2Position()
	{
		return NextControlPoint?.NextPosition ?? NextPosition;
	}

	public void Init(Spline owner)
	{
		mSpline = owner;
		SegmentIndex = -1;
	}

	public Vector3 Interpolate(float localF)
	{
		localF = Mathf.Clamp01(localF);
		return Spline.CatmulRom(PreviousPosition, Position, NextPosition, GetNext2Position(), localF);
	}

	public Vector3 InterpolateNormal(float localF)
	{
		localF = Mathf.Clamp01(localF);
		return Spline.CatmulRom(PreviousNormal, Normal, NextNormal, GetNext2Normal(), localF);
	}
}
