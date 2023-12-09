using System.Reflection;
using Photon;
using UnityEngine;

[AddComponentMenu("Miscellaneous/Photon View &v")]
public class PhotonView : Photon.MonoBehaviour
{
	protected internal bool destroyedByPhotonNetworkOrQuit;

	protected internal bool didAwake;

	private bool failedToFindOnSerialize;

	public int group;

	private object[] instantiationDataField;

	public int instantiationId;

	protected internal object[] lastOnSerializeDataReceived;

	protected internal object[] lastOnSerializeDataSent;

	protected internal bool mixedModeIsReliable;

	public Component observed;

	private MethodInfo OnSerializeMethodInfo;

	public OnSerializeRigidBody onSerializeRigidBodyOption = OnSerializeRigidBody.All;

	public OnSerializeTransform onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;

	public int ownerId;

	public int prefixBackup = -1;

	public int subId;

	public ViewSynchronization synchronization;

	public object[] instantiationData
	{
		get
		{
			if (!didAwake)
			{
				instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(instantiationId);
			}
			return instantiationDataField;
		}
		set
		{
			instantiationDataField = value;
		}
	}

	public bool isMine
	{
		get
		{
			if (ownerId != PhotonNetwork.player.ID)
			{
				if (isSceneView)
				{
					return PhotonNetwork.isMasterClient;
				}
				return false;
			}
			return true;
		}
	}

	public bool isSceneView => ownerId == 0;

	public PhotonPlayer owner => PhotonPlayer.Find(ownerId);

	public int OwnerActorNr => ownerId;

	public int prefix
	{
		get
		{
			if (prefixBackup == -1 && PhotonNetwork.networkingPeer != null)
			{
				prefixBackup = PhotonNetwork.networkingPeer.currentLevelPrefix;
			}
			return prefixBackup;
		}
		set
		{
			prefixBackup = value;
		}
	}

	public int viewID
	{
		get
		{
			return ownerId * PhotonNetwork.MAX_VIEW_IDS + subId;
		}
		set
		{
			bool num = didAwake && subId == 0;
			ownerId = value / PhotonNetwork.MAX_VIEW_IDS;
			subId = value % PhotonNetwork.MAX_VIEW_IDS;
			if (num)
			{
				PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			}
		}
	}

	protected internal void Awake()
	{
		PhotonNetwork.networkingPeer.RegisterPhotonView(this);
		instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(instantiationId);
		didAwake = true;
	}

	protected internal void ExecuteOnSerialize(PhotonStream pStream, PhotonMessageInfo info)
	{
		if (!failedToFindOnSerialize)
		{
			if (OnSerializeMethodInfo == null && !NetworkingPeer.GetMethod(observed as UnityEngine.MonoBehaviour, PhotonNetworkingMessage.OnPhotonSerializeView.ToString(), out OnSerializeMethodInfo))
			{
				Debug.LogError("The observed monobehaviour (" + observed.name + ") of this PhotonView does not implement OnPhotonSerializeView()!");
				failedToFindOnSerialize = true;
				return;
			}
			object[] parameters = new object[2] { pStream, info };
			OnSerializeMethodInfo.Invoke(observed, parameters);
		}
	}

	public static PhotonView Find(int viewID)
	{
		return PhotonNetwork.networkingPeer.GetPhotonView(viewID);
	}

	public static PhotonView Get(Component component)
	{
		return component.GetComponent<PhotonView>();
	}

	public static PhotonView Get(GameObject gameObj)
	{
		return gameObj.GetComponent<PhotonView>();
	}

	protected internal void OnApplicationQuit()
	{
		destroyedByPhotonNetworkOrQuit = true;
	}

	protected internal void OnDestroy()
	{
		if (!destroyedByPhotonNetworkOrQuit)
		{
			PhotonNetwork.networkingPeer.LocalCleanPhotonView(this);
		}
		if (!destroyedByPhotonNetworkOrQuit && !Application.isLoadingLevel)
		{
			if (instantiationId > 0)
			{
				Debug.LogError(string.Concat("OnDestroy() seems to be called without PhotonNetwork.Destroy()?! GameObject: ", base.gameObject, " Application.isLoadingLevel: ", Application.isLoadingLevel));
			}
			else if (viewID <= 0)
			{
				Debug.LogWarning($"OnDestroy manually allocated PhotonView {this}. The viewID is 0. Was it ever (manually) set?");
			}
			else if (isMine && !PhotonNetwork.manuallyAllocatedViewIds.Contains(viewID))
			{
				Debug.LogWarning($"OnDestroy manually allocated PhotonView {this}. The viewID is local (isMine) but not in manuallyAllocatedViewIds list. Use UnAllocateViewID() after you destroyed the PV.");
			}
		}
		if (PhotonNetwork.networkingPeer.instantiatedObjects.ContainsKey(instantiationId))
		{
			bool flag = PhotonNetwork.networkingPeer.instantiatedObjects[instantiationId] == base.gameObject;
			if (flag)
			{
				object[] args = new object[5]
				{
					this,
					instantiationId,
					(!Application.isLoadingLevel) ? string.Empty : "Loading new scene caused this.",
					flag,
					destroyedByPhotonNetworkOrQuit
				};
				Debug.LogWarning(string.Format("OnDestroy for PhotonView {0} but GO is still in instantiatedObjects. instantiationId: {1}. Use PhotonNetwork.Destroy(). {2} Identical with this: {3} PN.Destroyed called for this PV: {4}", args));
			}
		}
	}

	public void RPC(string methodName, PhotonPlayer targetPlayer, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, parameters);
	}

	public void RPC(string methodName, PhotonTargets target, params object[] parameters)
	{
		if (PhotonNetwork.networkingPeer.hasSwitchedMC && target == PhotonTargets.MasterClient)
		{
			PhotonNetwork.RPC(this, methodName, PhotonNetwork.masterClient, parameters);
		}
		else
		{
			PhotonNetwork.RPC(this, methodName, target, parameters);
		}
	}

	public override string ToString()
	{
		object[] args = new object[4]
		{
			viewID,
			(base.gameObject == null) ? "GO==null" : base.gameObject.name,
			(!isSceneView) ? string.Empty : "(scene)",
			prefix
		};
		return string.Format("View ({3}){0} on {1} {2}", args);
	}
}
