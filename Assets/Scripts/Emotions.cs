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
        static float breathStart, bellyBreath,mouthBreath, breathIntensity, breathStrength;
        bool inhale, exhale;
        bool blinking;
        static SkinnedMeshRenderer aquaRenderer, pantsRenderer;
        void Start()
        {

            nextBlink = 0;
            eyeBlink = 0;
            blinkStart = 0;
            m_Character = GetComponent<ThirdPersonCharacter>();
            try
            {
                aquaRenderer = GameObject.Find("Body").GetComponent<SkinnedMeshRenderer>();
                //pantsRenderer = GameObject.Find("Pants").GetComponent<SkinnedMeshRenderer>();
            }
            catch (NullReferenceException ex)
            {
                print("Can't find Mesh Renderer");
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
                        nextBlink = UnityEngine.Random.Range(1, 2);
                    }
                }
                aquaRenderer.SetBlendShapeWeight(7, eyeBlink);
            }
        }

        public void Breath(bool isRunning, bool isExhausted)
        {
            //breathing = true;
            if (!isExhausted) { 
                breathIntensity = 80;
                breathStrength = 1.2f;
            }

            if (isExhausted)
            {
                breathIntensity = 100;
                breathStrength = 4;
            }

            if (bellyBreath < breathIntensity && !inhale)
            {
                bellyBreath += breathStrength;
                if (isRunning || isExhausted)
                {
                    mouthBreath = bellyBreath;
                    //Call sound function
                }
            }
            else inhale = true;

            if (bellyBreath > 0 && inhale)
            {
                bellyBreath -= breathStrength;
                if ((isRunning || isExhausted))
                {
                    mouthBreath = bellyBreath;
                }
                else {
                    if(mouthBreath > 0) mouthBreath -= breathStrength;
                }
            }
            else inhale = false;

            try { 
                aquaRenderer.SetBlendShapeWeight(13, bellyBreath);
                //pantsRenderer.SetBlendShapeWeight(0, bellyBreath);
                aquaRenderer.SetBlendShapeWeight(12, mouthBreath);
            }
            catch (NullReferenceException ex)
            {
                print("Can't find breathing Blend Shapes ");
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