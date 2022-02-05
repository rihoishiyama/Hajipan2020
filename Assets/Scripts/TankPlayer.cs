using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TankPlayer : MonoBehaviourPunCallbacks//, IPunObservable
{
	PhotonView m_photonView;

	[SerializeField]
	private Button onFireButton;
	[SerializeField]
	private ShotBullet shotBullet;
	[SerializeField]
	private PhotonView photonview;

	private const float InterpolationDuration = 0.2f;

	private Vector3 startPosition;
	private Vector3 endPosition;
	private Quaternion startRotation;
	private Quaternion endRotation;
	private float elapsedTime = 0f;
	public float moveSpeed = 4f;
	public AudioClip dieSound;
	public Joystick joystick;


	// enum名仮（現状色変更のみ）
	public enum shotType
	{
		red = 0,
		blue,
		green,
	}

	[SerializeField]
	private shotType type = shotType.red; 

	void Awake()
	{
		startPosition = transform.position;
		endPosition = transform.position;
		startRotation = transform.rotation;
		endRotation = transform.rotation;
	}

	void Start()
	{
		// 自身が所有者かどうかを判定する
		if (photonView.IsMine) {
			// 所有者を取得する
			Player owner = photonView.Owner;
			// 所有者のプレイヤー名とIDをコンソールに出力する
			Debug.Log($"{owner.NickName}({photonView.OwnerActorNr})");
		}
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Space) && GameState.m_gameState == GameState.e_GameState.Game) {
			if (photonView.IsMine) {
				shotBullet.ButtonShot(type, moveSpeed);
			}
		}
		// 自分の生成したタンクの移動を制御をする
        if (photonView.IsMine && GameState.m_gameState == GameState.e_GameState.Game) {
            var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if (input != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(input.normalized);
                transform.Translate(0f, 0f , moveSpeed * Time.deltaTime);
            }      
        }
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Bullet"))
		{
			AudioSource.PlayClipAtPoint(dieSound, transform.position);
			//this.gameObject.GetComponent<PhotonView> ().TransferOwnership (PhotonNetwork.player.ID);
			if (PhotonNetwork.InRoom)
			{
				Hashtable customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
				int alivePlayerNum = (PhotonNetwork.CurrentRoom.CustomProperties["alivePlayer"] is int value) ? value : 0;
				if(alivePlayerNum > 0)
                {
					customProperties["alivePlayer"] = --alivePlayerNum;
					PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
				}
			}
			// 自分のタンクの場合そのまま破壊
			if (photonView.IsMine) {
				PhotonNetwork.Destroy(this.gameObject);
			}
			// 当たった弾の所有権をプレイヤーにタンクの所有権を譲渡
			this.gameObject.GetComponent<PhotonView> ().TransferOwnership (other.gameObject.GetComponent<PhotonView>().OwnerActorNr);
			// タンクを破壊
			PhotonNetwork.Destroy(this.gameObject);
		}
	}
}
