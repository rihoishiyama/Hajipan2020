using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPunCallbacks
{
	/* パラメータ */
	[SerializeField] private float m_attack_power = 1.0f; //火力
	[SerializeField] private float m_bullet_size  = 1.0f; //弾の大きさ
	[SerializeField] private int   m_rebound_limit = 1; //跳ね返る上限
	[SerializeField] private Color m_bullet_color = Color.red; //弾の色
	private Hashtable customProperties;

	/* ----------------------------------------- */

	/*
	 * 挙動方法
	 *
	 * 1.受け取ったパラメータ入力値を設定 ok
	 * 2.色を変える ok
	 * 3.相手のタンクにどうやってダメージ与えよう。。。→以下みたいなの書けば大丈夫
	 * void OnCollisionEnter2D(Collision2D collision) {
			if (collision.gameObject.GetComponent<Player>()) {
				collision.gameObject.GetComponent<Player>().Damage(1);
			}
		}
	 * 4.あとはそれぞれパラメーターに沿って処理を書く
	 */



	public void SetBulletParam(float attack_power, float size, int rebound_limit, Color color)
	{
		//パラメータ初期設定
		m_attack_power	= attack_power;
		m_bullet_size   = size;
		m_rebound_limit = rebound_limit;
		m_bullet_color  = color;
		SetBulletColor(color);
	}

	void SetBulletColor(Color color)
	{
		this.gameObject.GetComponent<Renderer>().material.color = color;
	}






	// 跳ね返り
	[SerializeField] private AudioClip reboundSound;
	private int rebound_cnt;

	[SerializeField] private PhotonView m_photonview;


    void OnCollisionEnter(Collision other)
	{
		//跳ね返り処理
		if (other.gameObject.CompareTag("Wall"))
		{

			rebound_cnt += 1;
			Debug.Log(rebound_cnt + " / " + m_rebound_limit);
			if (rebound_cnt > m_rebound_limit)
			{
                if (m_photonview.IsMine)
                {
                    ShotBullet.bulletcount -= 1;
                }
            PhotonNetwork.Destroy(this.gameObject);
			}
			else
			{
				AudioSource.PlayClipAtPoint(reboundSound, transform.position);
			}
		}

		//弾消失（このままでいいのでは）
		if (other.gameObject.CompareTag("Bullet"))
		{

			if (m_photonview.IsMine)
			{
				ShotBullet.bulletcount -= 1;
			}
			PhotonNetwork.Destroy(this.gameObject);
		}

		//攻撃処理
		if (other.gameObject.CompareTag("Player"))
		{

			//TODO:Tank側にダメージ処理追加されたらここ追加
			/*
			if (other.gameObject.GetComponent<TankPlayer>())
			{
				other.gameObject.GetComponent<TankPlayer>().Damage(1);
			}
			*/
			if( GetLayer() == 9 )
			{
				if (m_photonview.IsMine)
				{
					ShotBullet.bulletcount -= 1;
				}
				PhotonNetwork.Destroy(this.gameObject);
			}

		}

    }



	// 時間経過
	private float m_elapsed_time = 0.0f;
	private bool is_comp_change_layer = false;
	void SetLayer(int layer)
	{
		this.gameObject.layer = layer;
	}

	int GetLayer()
	{
		return this.gameObject.layer;
	}

	void Start()
	{
		// customProperties = PhotonNetwork.CurrentRoom.CustomProperties;

		SetLayer(10); // NewBullet
		Debug.Log("弾生成");
 	}
	void Update()
	{
		if(m_elapsed_time <= 1.0f)
		{
			m_elapsed_time += Time.deltaTime;
		}

		if (m_elapsed_time >= 1.0f && !is_comp_change_layer)
		{
			SetLayer(9); // Bullet
			is_comp_change_layer = true;
			Debug.Log("弾レイヤー変更");

		}
	}

}