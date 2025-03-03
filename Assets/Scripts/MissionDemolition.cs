using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum GameMode {                                                            // b
    idle,     
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // a private Singleton                    // c
 
    [Header("Inscribed")]
    public TextMeshProUGUI               uitLevel;  // The UIText_Level Text
    public TextMeshProUGUI               uitShots;  // The UIText_Shots Text
    public Vector3            castlePos; // The place to put castles
    public GameObject[]       castles;   // An array of the castles
 
    [Header("Dynamic")]
    public int                level;     // The current level
    public int                levelMax;  // The number of lev”   
    public int                shotsTaken;
    
    public GameObject castle;
    public GameMode mode = GameMode.idle;

    public string showing = "Show Slingshot";
    
    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        startLevel();
    }

    void startLevel(){
        //get rid of castle if the old one exists
        if (castle != null){
            Destroy( castle );
        }

        //Destroy old projectiles if they exist (the method is not tet written)
        Projectile.DESTROY_PROJECTILES(); //This will be underlined in red

        //Instantiate the new castle
        castle = Instantiate<GameObject>( castles[level] );
        castle.transform.position = castlePos;

        //Reset the goal
        Goal.goalMet = false;

        updateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW( FollowCam.eView.both );
    }

    void updateGUI(){
        //Show data in the GUITexts
        uitLevel.text = "Level: "+(level+1)+" of "+levelMax;
        uitShots.text = "Shots Taken: "+shotsTaken;
    }

    void Update()
    {
        updateGUI();

        //check for level end
        if( (mode == GameMode.playing) && Goal.goalMet ){
            //Change mode to stope checking for level end
            mode = GameMode.levelEnd;

            FollowCam.SWITCH_VIEW( FollowCam.eView.both ); 

            //Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel(){
        level++;
        if(level == levelMax){
            level = 0;
            shotsTaken = 0;
            SceneManager.LoadScene( "Game_Over" );   
        }
        startLevel();
    }

    public static void SHOT_FIRED(){
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE(){
        return S.castle;
    }
}
