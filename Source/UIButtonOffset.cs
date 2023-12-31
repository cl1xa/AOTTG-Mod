using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	public float duration = 0.2f;

	public Vector3 hover = Vector3.zero;

	private bool mHighlighted;

	private Vector3 mPos;

	private bool mStarted;

	public Vector3 pressed = new Vector3(2f, -2f);

	public Transform tweenTarget;

	private void OnDisable()
	{
		if (mStarted && tweenTarget != null)
		{
			TweenPosition component = tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.position = mPos;
				component.enabled = false;
			}
		}
	}

	private void OnEnable()
	{
		if (mStarted && mHighlighted)
		{
			OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!mStarted)
			{
				Start();
			}
			TweenPosition.Begin(tweenTarget.gameObject, duration, (!isOver) ? mPos : (mPos + hover)).method = UITweener.Method.EaseInOut;
			mHighlighted = isOver;
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!mStarted)
			{
				Start();
			}
			TweenPosition.Begin(tweenTarget.gameObject, duration, isPressed ? (mPos + pressed) : ((!UICamera.IsHighlighted(base.gameObject)) ? mPos : (mPos + hover))).method = UITweener.Method.EaseInOut;
		}
	}

	private void Start()
	{
		if (!mStarted)
		{
			mStarted = true;
			if (tweenTarget == null)
			{
				tweenTarget = base.transform;
			}
			mPos = tweenTarget.localPosition;
		}
	}
}
