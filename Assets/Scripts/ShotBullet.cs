using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShotBullet : MonoBehaviourPunCallbacks
{
	/* パラメータ */
	[SerializeField] private Vector3 m_vector = new Vector3(); //発射方向
	[SerializeField] private int m_bullet_count = 5; //発射可能弾数
	[SerializeField] private string[] m_shot_mode = {"Singl", "Multi" }; //発射モード[散弾 or 単弾]
	[SerializeField] private AudioClip m_shot_audio; //発射音 散弾の場合音を一つにしたいため
	[SerializeField] private float m_shot_speed = 1.0f; //弾速
	/* ----------------------------------------- */

	/*
	 * 挙動方法
	 * 
	 * どのタイプの打ち方にするかを引数に入力させる関数（最上位）
	 * Todo:（（弾数管理どうしよう...Bulletクラスはどこも参照して欲しくないのに今のところShotBulletを参照している
	 * 現在のButtonShotを名前変更して段数管理系の名前にする
	 * 
	 * 各タイプのshot関数を用意する（今の中身から根本は変えなくていい）
	 * 入力値によってどの関数にするか選択
	 * Todo：どうにかしてBullet関数に初期値を入力したい
	 */





	[SerializeField]
	//private PhotonView photonView;
	public Transform shotPlace;
	public GameObject shellPrefab;
	public float shotSpeed;
	public AudioClip shotSound;
	public AudioClip shotmissSound;
	public static int bulletcount;

	public void ButtonShot()
	{
        // Shot();
        // AudioSource.PlayClipAtPoint(shotSound, transform.position);
		// if (photonView.isMine)
		// {
		// もしも「Fire1」というボタンが押されたら（条件）
		if (bulletcount < 5)
		{
			Shot();
			// ②効果音を再生する。
			AudioSource.PlayClipAtPoint(shotSound, transform.position);
			bulletcount += 1;
		}
		else
		{
			AudioSource.PlayClipAtPoint(shotmissSound, transform.position);
		}
		// }
	}

	public void Shot()
	{
		// プレファブから砲弾(Shell)オブジェクトを作成し、それをshellという名前の箱に入れる。
		GameObject shell = PhotonNetwork.Instantiate("bullet", shotPlace.position, Quaternion.identity, 0);
		//GameObject shell = Instantiate(shellPrefab, shotPlace.position, Quaternion.identity);

		// Rigidbodyの情報を取得し、それをshellRigidbodyという名前の箱に入れる。
		Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();

		// shellRigidbodyにz軸方向の力を加える。
		shellRigidbody.AddForce(transform.forward * shotSpeed);
	}
}