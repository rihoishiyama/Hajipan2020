using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Tank : MonoBehaviour
{
    /*
     * 操作はwasd space発射
     * Tankがshotbulletを使い
     * shotbulletがbulletを使う
     * 
     * SelectTankからTankの個性値を受け取り, ここで代入.
     */
    [SerializeField]
    private SelectTank selectTank;
    [SerializeField]
    private ShotBullet shotBullet;

    [SerializeField]
    private int m_hp;
    [SerializeField]
    private int m_speed;
    [SerializeField]
    private float m_stepDistance;
    [SerializeField]
    private float m_accelerateTime;
    [SerializeField]
    private bool m_isEnableFire;
    [SerializeField]
    private bool m_isEnableStep;
    [SerializeField]
    private bool m_isMatchless;

    public int HP
    {
        get { return m_hp; }
        private set { m_hp = value; }
    }

    public int SPEED
    {
        get { return m_speed; }
        private set { m_speed = value; }
    }

    

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void SetTankStatus()
    {

    }
}
