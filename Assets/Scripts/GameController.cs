using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//this script cannot function properly without GameUI
//let Unity know about it with this attribute
[RequireComponent(typeof(GameUI))]
public class GameController : MonoBehaviour {

    //we can get this instance from other scripts very easily
    public static GameController Instance { get; private set; }

    [SerializeField]
    private int knifeCount;

    [Header("Knife Spawning")]
    [SerializeField]
    private Vector2 knifeSpawnPosition;
    [SerializeField]
    //this will be a prefab of the knife. You will create the prefab later.
    private GameObject knifeObject;

    //reference to the GameUI on GameController's game object
    public GameUI GameUI { get; private set; }

    private void Awake()
    {
        //simple kind of a singleton instance (we're only in 1 scene)
        Instance = this;

        GameUI = GetComponent<GameUI>();
    }

    private void Start()
    {
        //update the UI ASA game start
        GameUI.SetInitialDisplayedKnifeCount(knifeCount);
        //also spawn the first knife
        SpawnKnife();
    }

    //this method will be called from KnifeScript
    public void OnSuccessfulKnifeHit()
    {
        if (knifeCount > 0)
        {
            SpawnKnife();
        }
        else
        {
            StartGameOverSequence(true);
        }
    }

   
    private void SpawnKnife()
    {
        knifeCount--;
        Instantiate(knifeObject, knifeSpawnPosition, Quaternion.identity);
    }

    //the public method for starting game over
    public void StartGameOverSequence(bool win)
    {
        StartCoroutine("GameOverSequenceCoroutine", win);
    }

    //this is a coroutine because we want to wait for a while when the player wins
    private IEnumerator GameOverSequenceCoroutine(bool win)
    {
        if (win)
        {
            //the player realize it's game over and he won
            yield return new WaitForSecondsRealtime(0.3f);
            
            //restart the game after every time knife miss
            RestartGame();
        }
        else
        {
            GameUI.ShowRestartButton();
        }
    }

    public void RestartGame()
    {
        //restart the scene by reloading the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}