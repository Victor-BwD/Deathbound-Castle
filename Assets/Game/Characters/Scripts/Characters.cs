using UnityEngine;

public class Characters : MonoBehaviour
{
    public Transform skin;
    public int life;
    
    void Update()
    {
        if(life <= 0)
        {
            OnDeath();
        }
    }

    public void PlayerTakaDamage(int damage)
    {
        life -= damage;
        skin.GetComponent<Animator>().Play("PlayerTakeDamage", 1);
    }

    protected virtual void OnDeath()
    {
        skin.GetComponent<Animator>().Play("Die", -1);
    }
}
