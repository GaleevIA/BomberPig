using UnityEngine;

public class Stone : Unit
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    //При получении урона камни немного меняют цвет
    //SpriteRenderer решил получать именно здесь, а не в Start, чтобы не получать лишние компоненты у камней, которые возможно никогда не заденет бомбой
    public override void GetDamage() 
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g - 0.2f, spriteRenderer.color.b - 0.2f, spriteRenderer.color.a);

        base.GetDamage();      
    }
}
