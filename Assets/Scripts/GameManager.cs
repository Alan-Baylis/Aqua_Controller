using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Texture2D StaminaTextureBackground, StaminaTextureFill;
    public Texture2D HealthTextureBackground, HealthTextureFill;
    public float stamina, maxStamina, health, maxHealth;

    public bool enableGUI;


    // singleton
    private static GameManager m_Instance = null;
    public static GameManager Get()
    {

        if (m_Instance == null)
            m_Instance = (GameManager)FindObjectOfType(typeof(GameManager));
        return m_Instance;
    }

    // class 
    public GUIStats guiStatsObject;

    void Awake()
    {
        Application.targetFrameRate = 300;

        //Dissable Mouse                
        Cursor.visible = false;
        Screen.lockCursor = true;
    }
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Screen.lockCursor = false;
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
        if (Input.GetKey("escape"))
            Application.Quit();

        if (!enableGUI && Input.GetKeyUp(KeyCode.G))
        {
            enableGUI = true;
        }
        else if(enableGUI && Input.GetKeyUp(KeyCode.G))
        {
            enableGUI = false;
        }
        GUIStats.enableGUIStats = enableGUI;



    }
}