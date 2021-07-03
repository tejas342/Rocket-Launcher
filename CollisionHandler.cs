using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success ;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles ;
    [SerializeField] ParticleSystem crashParticles ;

    bool isTansistioning = false ;
    bool collisionDisabled = false;
    AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnCollisionEnter(Collision other) 
    {
        if(isTansistioning || collisionDisabled)
        {
            return;
        }

        switch(other.gameObject.tag)
        { 
            case "Friendly" :
                Debug.Log("this thing is friendly");
                break;

            case "Finish":
                Debug.Log("Congrats You Finish");
                StartSuccessSequence();
                break;

            default :
                StartCrashSequence();
                break;
        }
    }

    void Update() 
    {
        //RespondToDebugKeys();    
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled ; //toogle between true and false
        }
    }

    void StartSuccessSequence()
    {   
        isTansistioning = true ;

        audioSource.Stop();
        audioSource.PlayOneShot(success);

        successParticles.Play();

        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel" , levelLoadDelay);
    }
    

    void StartCrashSequence()
    {
        isTansistioning = true ;

        audioSource.Stop();
        audioSource.PlayOneShot(crash);

        crashParticles.Play();

        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel",levelLoadDelay);
    }
    void LoadNextLevel()
    {
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneindex + 1 ;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {                   //this is for playing levels in loop
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        /*if (nextSceneIndex == 5)
        {
            Debug.Log("quit heres");
            Application.Quit();
            return;
                                            //this is for only playing all the levels at once
        }
        else
        {SceneManager.LoadScene(nextSceneIndex);}*/
    }
    void ReloadLevel()
    {
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneindex);
    }
}