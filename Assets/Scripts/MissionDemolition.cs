// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    #region GlobalVareables
    #region DefaultVareables
    public bool isDebug = false;
    private string debugScriptName = "MissionDemolition";
    #endregion

    #region Static
    public static MissionDemolition S = null;
    #endregion

    #region Public
    public GameObject[] castles;
    public GUIText gtLevel = null;
    public GUIText gtScore = null;
    public Vector3 castlePos = Vector3.zero;

    [Header("For Debug View Only")]
    public int level = 0;
    public int levelMax = 0;
    public int shotsTaken = 0;
    public GameObject castle = null;
    public GameMode mode = GameMode.idle;
    public string showing = "Slingshot";
    #endregion

    #region Private

    #endregion
    #endregion

    #region CustomFunction
    #region Static
    public static void SwitchView(string view)
    {
        S.showing = view;
        switch(S.showing)
        {
            case "Slingshot":
                FollowCam.S.poi = null;
                break;
            case "Castle":
                FollowCam.S.poi = S.castle;
                break;
            case "Both":
                FollowCam.S.poi = GameObject.Find("ViewBoth");
                break;
        }
    }

    public static void ShotFired()
    {
        S.shotsTaken++;
    }
    #endregion

    #region Public

    #endregion

    #region Private
    private void StartLevel()
    {
        if (castle != null) Destroy(castle);

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos) Destroy(pTemp);

        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsTaken = 0;

        SwitchView("Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        ShowGT();

        mode = GameMode.playing;
    }

    private void ShowGT()
    {
        gtLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        gtScore.text = "Shots Taken: " + shotsTaken;
    }

    private void NextLevel()
    {
        level++;
        if (level == levelMax) level = 0;
        StartLevel();
    }
    #endregion

    #region Debug
    private void PrintDebugMsg(string msg)
    {
        if (isDebug) Debug.Log(debugScriptName + "(" + this.gameObject.name + "): " + msg);
    }
    private void PrintWarningDebugMsg(string msg)
    {
        Debug.LogWarning(debugScriptName + "(" + this.gameObject.name + "): " + msg);
    }
    private void PrintErrorDebugMsg(string msg)
    {
        Debug.LogError(debugScriptName + "(" + this.gameObject.name + "): " + msg);
    }
    #endregion

    #region Getters

    #endregion

    #region Setters

    #endregion
    #endregion

    #region UnityFunctions
    void OnGUI()
    {
        Rect buttonRect = new Rect((Screen.width / 2) - 50, 10, 100, 24);

        switch(showing)
        {
            case "Slingshot":
                if (GUI.Button(buttonRect, "Show Castle")) SwitchView("Castle");
                break;
            case "Castle":
                if (GUI.Button(buttonRect, "Show Both")) SwitchView("Both");
                break;
            case "Both":
                if (GUI.Button(buttonRect, "Show Slingshot")) SwitchView("Slingshot");
                break;
        }
    }
    #endregion

    #region Start_Update
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        PrintDebugMsg("Loaded.");
    }
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {

    }
    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        ShowGT();

        if(mode == GameMode.playing && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Both");
            Invoke("NextLevel", 2f);
        }
    }
    // LateUpdate is called every frame after all other update functions, if the Behaviour is enabled.
    void LateUpdate()
    {

    }
    #endregion
}