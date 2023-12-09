using UnityEngine;

public class EffectController : MonoBehaviour
{
	protected XffectCache EffectCache;

	public Transform ObjectCache;

	protected Vector3 GetFaceDirection()
	{
		return base.transform.TransformDirection(Vector3.forward);
	}

	private void OnEffect(string eftname)
	{
		switch (eftname)
		{
		case "lightning":
		{
			for (int i = 0; i < 9; i++)
			{
				Xffect component6 = EffectCache.GetObject(eftname).GetComponent<Xffect>();
				Vector3 zero = Vector3.zero;
				zero.x = Random.Range(-2.2f, 2.3f);
				zero.z = Random.Range(-2.1f, 2.1f);
				component6.SetEmitPosition(zero);
				component6.Active();
			}
			break;
		}
		case "cyclone":
		{
			Xffect component5 = EffectCache.GetObject(eftname).GetComponent<Xffect>();
			component5.SetDirectionAxis(GetFaceDirection().normalized);
			component5.Active();
			break;
		}
		case "crystal":
		{
			EffectCache.GetObject("crystal_surround").GetComponent<Xffect>().Active();
			Xffect component = EffectCache.GetObject("crystal").GetComponent<Xffect>();
			component.SetEmitPosition(new Vector3(0f, 1.9f, 1.4f));
			component.Active();
			Xffect component2 = EffectCache.GetObject("crystal_lightn").GetComponent<Xffect>();
			component2.SetDirectionAxis(new Vector3(-1.5f, 1.8f, 0f));
			component2.Active();
			Xffect component3 = EffectCache.GetObject("crystal").GetComponent<Xffect>();
			component3.SetEmitPosition(new Vector3(0f, 1.5f, -1.2f));
			component3.Active();
			Xffect component4 = EffectCache.GetObject("crystal_lightn").GetComponent<Xffect>();
			component4.SetDirectionAxis(new Vector3(1.4f, 1.4f, 0f));
			component4.Active();
			break;
		}
		default:
			EffectCache.GetObject(eftname).GetComponent<Xffect>().Active();
			break;
		}
	}

	private void OnGUI()
	{
		GUI.Box(new Rect(0f, 0f, 100f, 225f), "Effect List");
		GUI.Label(new Rect(150f, 0f, 350f, 25f), "alt+left mouse button to rotation.  mouse wheel to zoom.");
		if (GUI.Button(new Rect(10f, 20f, 80f, 20f), "Effect1"))
		{
			OnEffect("crystal");
		}
		if (GUI.Button(new Rect(10f, 45f, 80f, 20f), "Effect2"))
		{
			OnEffect("rage_explode");
		}
		if (GUI.Button(new Rect(10f, 70f, 80f, 20f), "Effect3"))
		{
			OnEffect("cyclone");
		}
		if (GUI.Button(new Rect(10f, 95f, 80f, 20f), "Effect4"))
		{
			OnEffect("lightning");
		}
		if (GUI.Button(new Rect(10f, 120f, 80f, 20f), "Effect5"))
		{
			OnEffect("hit");
		}
		if (GUI.Button(new Rect(10f, 145f, 80f, 20f), "Effect6"))
		{
			OnEffect("firebody");
		}
		if (GUI.Button(new Rect(10f, 170f, 80f, 20f), "Effect7"))
		{
			OnEffect("explode");
		}
		if (GUI.Button(new Rect(10f, 195f, 80f, 20f), "Effect8"))
		{
			OnEffect("rain");
		}
	}

	private void Start()
	{
		EffectCache = ObjectCache.GetComponent<XffectCache>();
	}
}
