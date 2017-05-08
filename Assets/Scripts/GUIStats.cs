using UnityEngine;
using System.Collections;

public class GUIStats : MonoBehaviour
{
    static public int guiScale = 1; //TODO
    static public Texture2D StaminaTexture, StaminaTextureFill;
    static public Texture2D HealthTexture, HealthTextureFill;
    public Texture2D musclesOnTexture, musclesOffTexture;

    private float staminaWidth, staminaHeight, staminaPosX, staminaPosY, healthPosY, musclePosX, musclePosY, healthWidth, muscleTexWidth, muscleTexHeight;
    private static float stamina, staminaFull, staminaGUI, health, healthFull, healthGUI;
    private bool setStamina;
    internal static bool enableGUIStats;
    public static bool enableMuscles;
    private float blendOut = 1;

    //Texture2D pixelsStaminaTextureFill;
    void Start()
    {
        StaminaTexture = GameManager.Get().StaminaTextureBackground;
        StaminaTextureFill = GameManager.Get().StaminaTextureFill;
        staminaWidth = StaminaTexture.width / 2 * guiScale;
        staminaHeight = StaminaTexture.height / 2 * guiScale;
        muscleTexWidth = musclesOnTexture.width / 2 * guiScale;
        muscleTexHeight = musclesOnTexture.height / 2 * guiScale;
        staminaPosX = StaminaTexture.width / 2 + StaminaTextureFill.width / 100 * guiScale;
        staminaPosY = StaminaTexture.height/ 10 * guiScale;
        musclePosX = muscleTexWidth / 10 * guiScale;
        musclePosY = muscleTexHeight / 2 * guiScale;

        HealthTexture = GameManager.Get().HealthTextureBackground;
        HealthTextureFill = GameManager.Get().HealthTextureFill;
        healthWidth = staminaWidth;
        healthPosY = StaminaTexture.height * 0.64f * guiScale;

    }

    void OnGUI()
    {
        if (enableGUIStats)
        {
            staminaFull = GameManager.Get().maxStamina;
            stamina = GameManager.Get().stamina;
            staminaGUI = stamina * staminaWidth / staminaFull;

            healthFull = GameManager.Get().maxHealth;
            health = GameManager.Get().health;
            healthGUI = health * healthWidth / healthFull;
            if(healthGUI < 0)
            {
                healthGUI = 0;
            }

            //Stamina GUI
            GUI.DrawTexture(new Rect(Screen.width - staminaPosX, staminaPosY, staminaGUI, staminaHeight), StaminaTextureFill);
            GUI.DrawTexture(new Rect(Screen.width - staminaPosX, staminaPosY, staminaWidth, staminaHeight), StaminaTexture);
            //Health GUI
            GUI.DrawTexture(new Rect(Screen.width - staminaPosX, healthPosY, healthGUI, staminaHeight), HealthTextureFill);
            GUI.DrawTexture(new Rect(Screen.width - staminaPosX, healthPosY, staminaWidth, staminaHeight), HealthTexture);
            //Muscle On.Off

            //Display framerate
            GUI.Label(new Rect(0, 0, 100, 100), (1.0f / Time.smoothDeltaTime).ToString());
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            blendOut = 2;
        }
        if (enableMuscles && blendOut > 0)
        {
            GUI.color = new Vector4(1,1,1, blendOut);
            blendOut -= 0.01f;
            GUI.DrawTexture(new Rect(musclePosX, musclePosY, muscleTexWidth, muscleTexHeight), musclesOnTexture);
        }
        else if (blendOut > 0)
        {
            GUI.color = new Vector4(1, 1, 1, blendOut);
            GUI.DrawTexture(new Rect(musclePosX, musclePosY, muscleTexWidth, muscleTexHeight), musclesOffTexture);
            blendOut -= 0.01f;
        }
    }
}
