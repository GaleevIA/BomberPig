using UniRx;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class Pig : Unit
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Space, SerializeField] private float speed;

    public Subject<Vector3> OnBombPlant = new Subject<Vector3>();

    private Vector3 direction;

    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    //Передвижение поросенка в зависимости от полученного направления
    public void Move(Vector3 direction)
    {
        this.direction = direction;

        PlayAnimation();
       
        transform.position += direction * speed * Time.deltaTime;        
    }

    //Спавн бомбы поросенком
    public void PlantBomb()
    {      
        OnBombPlant.OnNext(transform.position);
    }

    //Проигрываем анимацию в зависимости от направления движения поросенка
    private void PlayAnimation()
    {
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);

        if(Mathf.Abs(direction.x) <= Mathf.Abs(direction.y))
        {
            if(direction.y < 0)
                animator.SetBool("Down", true);
            else if(direction.y > 0)
                animator.SetBool("Up", true);
        }
        else if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x < 0)
                animator.SetBool("Left", true);
            else if(direction.x > 0)
                animator.SetBool("Right", true);
        }

        //if (direction == new Vector3(1, 0))
        //    animator.SetBool("Right", true);
        //else if (direction == new Vector3(-1, 0))
        //    animator.SetBool("Left", true);
        //else if (direction == new Vector3(0, 1))
        //    animator.SetBool("Up", true);
        //else if (direction == new Vector3(0, -1))
        //    animator.SetBool("Down", true);
        //else if (direction == new Vector3(1, 1))
        //    animator.SetBool("Up", true);
        //else if (direction == new Vector3(-1, -1))
        //    animator.SetBool("Down", true);
    }
}
