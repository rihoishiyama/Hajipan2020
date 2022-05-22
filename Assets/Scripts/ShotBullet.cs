using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShotBullet : MonoBehaviourPunCallbacks
{
	private const float DEFAULT_MOVE_SPEED   = 4.0f; //ここに書くのがいいのかわからんから消すかも
	private const float DEFAULT_BULLET_SPEED = 40.0f;
	private const float DEFAULT_BULLET_SIZE  = 0.3f;

	/* パラメータ */
	[SerializeField] private Vector3 m_vector = new Vector3(); //発射方向
	[SerializeField] private int m_bullet_count = 5; //発射可能弾数
	[SerializeField] private string[] m_shot_mode = {"Singl", "Multi" }; //発射モード[散弾 or 単弾]
	[SerializeField] private AudioClip m_shot_audio; //発射音 散弾の場合音を一つにしたいため
	[SerializeField] private float m_shot_speed = 40.0f; //弾速
	[SerializeField] private float m_shot_size = 0.3f; //弾の大きさ　テスト用
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
	 * 
	 * new!-弾のスピード＋車体の速さ＋弾の大きさ　を考慮してshotPlaceのz軸の位置を決める 
	 	-> 最初の0.5sくらい自身への当たり判定ないほうがいいのでは
	 * クールタイム実装する
	 */


	//[SerializeField]
	//private PhotonView photonView;
	public Transform shotPlace;
	public GameObject shellPrefab;
	//public float shotSpeed;
	public AudioClip shotSound;
	public AudioClip shotmissSound;
	public static int bulletcount;

	public void ButtonShot(TankPlayer.shotType typeID, float tank_move_speed/*, float bullet_speed, float bullet_size*/)
	{
        // Shot();
        // AudioSource.PlayClipAtPoint(shotSound, transform.position);
		// if (photonView.isMine)
		// {
		if (bulletcount < 5)
		{
			switch(typeID)
			{
				case TankPlayer.shotType.red:
					Shot(tank_move_speed, m_shot_speed, m_shot_size, Color.red);
					break;
				
				case TankPlayer.shotType.blue:
					Shot(tank_move_speed, m_shot_speed, m_shot_size, Color.blue);
					break;
				
				case TankPlayer.shotType.green:
					Shot(tank_move_speed, m_shot_speed, m_shot_size, Color.green);
					break;

				default:
					break;
			}

			//Shot(tank_move_speed, bullet_speed, bullet_size);
			
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

	public void Shot(float tank_move_speed, float bullet_speed, float bullet_size, Color color)
	{
		// プレファブから砲弾(Shell)オブジェクトを作成し、それをshellという名前の箱に入れる。
		GameObject shell = PhotonNetwork.Instantiate("bullet", shotPlace.position, Quaternion.identity, 0);
		shell.GetComponent<Bullet>().SetBulletParam(1.0f, bullet_size, 1, color);
		//弾のサイズ変更
		shell.transform.localScale = new Vector3(m_shot_size, m_shot_size, m_shot_size);
		
		// Rigidbodyの情報を取得し、それをshellRigidbodyという名前の箱に入れる。
		Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();

		// shellRigidbodyにz軸方向の力を加える。
		shellRigidbody.AddForce(transform.forward * bullet_speed);
	}
}