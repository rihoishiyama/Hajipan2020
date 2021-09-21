using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPunCallbacks
{
	/* パラメータ */
	[SerializeField] private float m_attack_power = 1.0f; //火力
	[SerializeField] private Color m_bullet_color = Color.red; //弾の色
	[SerializeField] private float m_bullet_size  = 1.0f; //弾の大きさ
	[SerializeField] private int   m_rebound_limit = 1; //跳ね返る上限
	/* ----------------------------------------- */

	/*
	 * 挙動方法
	 * 
	 * 受け取ったパラメータ入力値を設定
	 * 色を変える
	 * Todo:相手のタンクにどうやってダメージ与えよう。。。
	 * あとはそれぞれパラメーターに沿って処理を書く
	 */


















	/* 跳ね返り */
	[SerializeField] private AudioClip reboundSound;
	private int rebound_cnt;

	[SerializeField] private PhotonView m_photonview;


    void Awake()
    {
		this.gameObject.GetComponent<Material>().color = Color.red;   
    }


    void OnCollisionEnter(Collision other)
	{
		//跳ね返り処理
		if (other.gameObject.CompareTag("Wall"))
		{

			rebound_cnt += 1;

			if (rebound_cnt > 1)
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
			if (m_photonview.IsMine)
			{
			ShotBullet.bulletcount -= 1;
			}
			PhotonNetwork.Destroy(this.gameObject);
		}
			
    }
	
}