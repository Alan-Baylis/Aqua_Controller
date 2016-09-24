using System.Collections;
using UnityEngine;
using System;
//using UnityStandardAssets.Characters.ThirdPerson;

namespace UnityStandardAssets.Characters.ThirdPerson
{

    public class Emotions : MonoBehaviour
    {
        static ThirdPersonCharacter m_Character;

        static float eyeBlink, blinkStart, nextBlink, blinkTimer;
        bool blinking;
        static SkinnedMeshRenderer aquaRenderer;
        void Start()
        {

            nextBlink = 0;
            eyeBlink = 0;
            blinkStart = 0;
            m_Character = GetComponent<ThirdPersonCharacter>();
            try
            {
                aquaRenderer = GameObject.Find("Body").GetComponent<SkinnedMeshRenderer>();
            }
            catch (NullReferenceException ex)
            {
                //print("Cant find");
            }

        }

        public void EyeBlink()
        {

            blinkStart += 0.01f;
            if (blinkStart > nextBlink)
            {
                //Shutting eyes
                if (eyeBlink <= 85 && !blinking)
                {
                    eyeBlink += 15;

                    if (eyeBlink > 85) { blinking = true; }
                }

                //Opening eyes
                if (eyeBlink >= 15 && blinking)
                {
                    eyeBlink -= 15;

                    if (eyeBlink < 15)
                    {
                        blinking = false;
                        blinkStart = 0;
                        nextBlink = UnityEngine.Random.Range(1, 4);
                    }
                }
                aquaRenderer.SetBlendShapeWeight(4, eyeBlink);
            }
        }


        public void Surprised()
        {
            //print("Falling scared");
            if (m_Character.isFalling)
            {
                aquaRenderer.SetBlendShapeWeight(0, 100);
            }
            if (!m_Character.isFalling)
            {
                aquaRenderer.SetBlendShapeWeight(0, 0);
            }
        }


    }

}