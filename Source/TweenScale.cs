using UnityEngine;

[AddComponentMenu("NGUI/Tween/Scale")]
public class TweenScale : UITweener
{
	public Vector3 from = Vector3.one;

	private UITable mTable;

	private Transform mTrans;

	public Vector3 to = Vector3.one;

	public bool updateTable;

	public Transform cachedTransform
	{
		get
		{
			if (mTrans == null)
			{
				mTrans = base.transform;
			}
			return mTrans;
		}
	}

	public Vector3 scale
	{
		get
		{
			return cachedTransform.localScale;
		}
		set
		{
			cachedTransform.localScale = value;
		}
	}

	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.scale;
		tweenScale.to = scale;
		if (duration <= 0f)
		{
			tweenScale.Sample(1f, isFinished: true);
			tweenScale.enabled = false;
		}
		return tweenScale;
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		cachedTransform.localScale = from * (1f - factor) + to * factor;
		if (!updateTable)
		{
			return;
		}
		if (mTable == null)
		{
			mTable = NGUITools.FindInParents<UITable>(base.gameObject);
			if (mTable == null)
			{
				updateTable = false;
				return;
			}
		}
		mTable.repositionNow = true;
	}
}
