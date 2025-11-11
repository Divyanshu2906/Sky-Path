using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float LoadNextLevelDelay = 1f;
    [SerializeField] AudioClip crashed;
    [SerializeField] AudioClip landing;

    AudioSource audioSource;
    bool iscontrolable = true; //made a bool variable so that movement freezes after touching the launching pad 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }


    void OnCollisionEnter(Collision other) // used when colliding with other objects 
    {
        if (!iscontrolable)
        {
            return;
        }

        switch (other.gameObject.tag) // always remember this to use when using collision with other game objects
        {
            case "Friendly":
                Debug.Log("Level Starts");
                break;

            case "Finish":
                Debug.Log("Level Complete");
                nextlevelsequence();
                break;

            default:
                Debug.Log("The rocket is crashed");
                startcrashingsequence();
                break;
        }
    }

    void nextlevelsequence() {

        iscontrolable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(landing);        
        GetComponent<Movement>().enabled = false; // made this so that after crashing the movement freezes so the player cannot control the rocket after it has been crashed
        Invoke("loadnextlevel", LoadNextLevelDelay);
    }

    void startcrashingsequence()
    {
        iscontrolable = false;
        audioSource.Stop(); // made this because if we landed or hit anything the thrust stops 
        audioSource.PlayOneShot(crashed);
        GetComponent<Movement>().enabled = false; // made this so that after crashing the movement freezes so the player cannot control the rocket after it has been crashed
        Invoke("reloadLevel", LoadNextLevelDelay); // now reload level method will work normally but there will a delay of 2 seconds 
    }

    void reloadLevel() //new method to reload level as soon as the rocket hits and obstacle
    {
        int currentscene = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentscene);
    }

    void loadnextlevel() //new method to load next scene 
    {
        int currentscene = SceneManager.GetActiveScene().buildIndex; //pre made template for getting active scene
        int nextscene = currentscene + 1; // loading scene = current scene + 1 so every next level is loaded

        if (nextscene == SceneManager.sceneCountInBuildSettings)
        {
            nextscene = 0;
        }

        SceneManager.LoadScene(nextscene); 
    }              
}
