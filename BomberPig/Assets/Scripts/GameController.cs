using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private Unit pig;
    [SerializeField] private Unit dog;
    [SerializeField] private Text battleEndText;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject farm;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject bombExplosionPrefab;

    void Start()
    {
        Time.timeScale = 1;

        panel.gameObject.SetActive(false);

        var stones = farm.GetComponentsInChildren<Unit>();

        foreach(var stone in stones)
        {
            stone.OnDeath.Subscribe(x => OnStoneDestroy(x));
        }

        pig.OnDeath.Subscribe(x => OnUnitDeath(x)).AddTo(pig);
        ((Pig)pig).OnBombPlant.Subscribe(x => OnBombPlant(x)).AddTo(pig);
        dog.OnDeath.Subscribe(x => OnUnitDeath(x)).AddTo(dog);
        dog.OnGetDamage.Subscribe(x => DogOnGetDamage(x)).AddTo(dog);
    }

    //При смерти собаки или поросенка игра завершается, в зависимости от того, кто выжил имеем либо победу, либо поражение
    private void OnUnitDeath(Unit unit)
    {
        Time.timeScale = 0;
        
        battleEndText.text = unit is Dog ? "Congratulations! You win!" : "Game over! You lose!";

        panel.gameObject.SetActive(true);
    }

    //Уничтожение камня
    private void OnStoneDestroy(Unit unit)
    {
        Destroy(unit.gameObject);
    }

    //При получении урона собакой, включается режим ярости и все ускоряется
    private void DogOnGetDamage(Unit unit)
    {
        Time.timeScale = 2f;
    }

    //Создаем бомбу и подписываемся на ее событие уничтожения
    private void OnBombPlant(Vector3 position)
    {
        var bomb = Instantiate(bombPrefab, position, Quaternion.identity);

        bomb.GetComponent<Bomb>().OnBombExplode.Subscribe(x => OnBombExplode(x)).AddTo(bomb.gameObject);
    }

    //Уничтожаем бомбу после взрыва и вызываем эффект взрыва
    private async void OnBombExplode(Bomb bomb)
    {
        var explosion = Instantiate(bombExplosionPrefab, bomb.gameObject.transform.position, Quaternion.identity);

        Destroy(bomb.gameObject);

        await Task.Delay(TimeSpan.FromSeconds(0.5));

        Destroy(explosion);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}