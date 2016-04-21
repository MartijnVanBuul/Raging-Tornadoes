/*
   Copyright 2015 Manus VR

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using UnityEngine;
using ManusMachina;
using System.Timers;
using System.Collections.Generic;

public class HandController : MonoBehaviour
{
    public Transform rootTranform;
    public Transform handSkin;
    public Transform handBones;
    public GLOVE_HAND hand;

    private Glove glove;
    private GameObject mHandCopy = null;

    public bool lockedInPlace;
    public bool handsInFixedTime;

    private Timer timer;

    void Start()
    {
        Manus.ManusInit();
        glove = new Glove(hand);
        timer = new Timer();
        timer.AutoReset = false;
        timer.Elapsed += Timer_Elapsed;
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        Manus.ManusSetVibration(hand, 0.0f);
    }

    void onApplicationQuit()
    {
        Manus.ManusExit();
    }

    /// <summary>
    /// Makes the motor in the Manus Glove vibrate for a certain duration
    /// </summary>
    /// <param name="aPower">The power of the motor from 0 to 1</param>
    /// <param name="aDuration">The duration in seconds</param>
    public int SetVibrationPeriod(float aPower, float aDuration)
    {
        if (timer == null || timer.Enabled)
            return Manus.ERROR;

        timer.Interval = aDuration * 1000.0;
        timer.Start();
        return Manus.ManusSetVibration(hand, aPower);
    }

    /// <summary>
    /// Makes the motor in the Manus Glove vibrate at a constant power.
    /// If the motor is currently set with SetVibrationPeriod, that timer is stopped.
    /// Turn the motor off by setting the power to 0.
    /// </summary>
    /// <param name="aPower">The power of the motor from 0 to 1</param>
    public int SetConstantVibration(float aPower)
    {
        return Manus.ManusSetVibration(hand, aPower);
    }

    /// <summary>
    /// Get the glove data
    /// </summary>
    /// <returns>The glove object containing the information</returns>
    public Glove getGlove()
    {
        return glove;
    }

    public void LockHandInPlace()
    {
        if(mHandCopy == null && !lockedInPlace)
        {
            mHandCopy = Instantiate(gameObject);
            mHandCopy.GetComponent<HandController>().lockedInPlace = true;
            mHandCopy.GetComponent<HandController>().handBones.GetComponent<SkinnedMeshRenderer>().enabled = false;
            mHandCopy.GetComponent<HandController>().handSkin.GetComponent<SkinnedMeshRenderer>().material.SetFloat("Alpha", 1.0f);
            mHandCopy.GetComponent<HandController>().handSkin.GetComponent<SkinnedMeshRenderer>().material.SetFloat("Inv Rim Power", 1.0f);
            mHandCopy.transform.rotation = transform.rotation;
            mHandCopy.transform.position = transform.position;
            mHandCopy.transform.parent = transform.parent;
            mHandCopy.transform.localScale = transform.localScale;
            mHandCopy.GetComponent<GloveTracker>().enabled = false;
            mHandCopy.GetComponent<HandSimulator>().enabled = false;
            Collider[] copyColliders = mHandCopy.GetComponentsInChildren<Collider>();
            for (int i = 0; i < copyColliders.Length; i++)
            {
                copyColliders[i].enabled = false;
            }

            handSkin.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
    }

    public void UnlockHandInPlace()
    {
        if (mHandCopy != null)
        {
            Destroy(mHandCopy);
            handSkin.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
    }
}