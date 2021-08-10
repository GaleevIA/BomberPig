using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Dog : Unit
{
    [SerializeField] private float speed;   

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Vector3 direction;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        ChangeDirection();
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    //При получении урона собака входит в режим бешенства
    public override void GetDamage()
    {
        base.GetDamage();

        PlayAnimation(true);
    }

    //В случае столкновения с препятствием ищем новое направление для движения
    private void ChangeDirection()
    {
        var prevDirection = direction;
        var i = Random.Range(1, 5);

        switch(i)
        {
            case 1: direction = new Vector3(1, 0); break;
            case 2: direction = new Vector3(0, 1); break;
            case 3: direction = new Vector3(-1, 0); break;
            case 4: direction = new Vector3(0, -1); break;
        }

        if (prevDirection == direction) ChangeDirection();

        PlayAnimation();
    }

    //При столкновении с препятствием либо меняем направление, либо наносим урон, если это поросенок
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") 
            collision.gameObject.GetComponent<Pig>().GetDamage();
        else 
            ChangeDirection();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ChangeDirection();
    }

    //Проигрываем анимации в зависимости от направления и состояния собаки
    private void PlayAnimation(bool isEnraged = false)
    {
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);

        if (isEnraged)
        {
            animator.SetBool("Enraged", true);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g - 0.5f, spriteRenderer.color.b - 0.2f, spriteRenderer.color.a);
        }                 
        else
        {
            if (direction == new Vector3(1, 0))
                animator.SetBool("Right", true);
            else if (direction == new Vector3(-1, 0))
                animator.SetBool("Left", true);
            else if (direction == new Vector3(0, 1))
                animator.SetBool("Up", true);
            else if (direction == new Vector3(0, -1))
                animator.SetBool("Down", true);
        }       
    }
}
