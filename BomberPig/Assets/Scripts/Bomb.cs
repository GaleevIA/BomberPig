using UniRx;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bomb : MonoBehaviour
{
    public Subject<Bomb> OnBombExplode = new Subject<Bomb>();

    private List<Unit> targets = new List<Unit>();
    private List<Bomb> bombsInRange = new List<Bomb>();
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //При попадании объекта в зону триггера, он добавляется в список целей.
    //Срабатывание бомбы происходит только при попадании в зону собаки
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Unit>(out Unit unit)) 
            targets.Add(collision.gameObject.GetComponent<Unit>());       

        if (collision.gameObject.tag == "Bomb")
            bombsInRange.Add(collision.gameObject.GetComponent<Bomb>());

        if (collision.gameObject.tag == "Dog")
            Triggered();
    }

    //При выходе из зоны триггера, объект удаляется из списка целей
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Unit>(out Unit unit)) 
            targets.Remove(unit);
    }

    public void Triggered()
    {
        if (animator != null)
            animator.SetTrigger("isTriggered");
    }

    //При взрыве бомба наносит урон всем целям, которые остались в зоне поражения и уничтожается сама
    public void Explode()
    {     
        for(int i = 0; i < targets.Count; i++) 
            targets[i]?.GetDamage();

        for (int i = 0; i < bombsInRange.Count; i++)
            bombsInRange[i]?.Triggered();

        OnBombExplode.OnNext(this);
    }
}