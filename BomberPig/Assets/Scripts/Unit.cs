using UnityEngine;
using UniRx;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private int lifes;

    public Subject<Unit> OnDeath = new Subject<Unit>();
    public Subject<Unit> OnGetDamage = new Subject<Unit>();

    //Получение урона юнитом
    public virtual void GetDamage()
    {
        lifes--;

        OnGetDamage.OnNext(this);

        CheckDeath();
    }

    //Проверка смерти юнита
    public void CheckDeath()
    {
        if (lifes <= 0) 
            OnDeath.OnNext(this);
    }
}