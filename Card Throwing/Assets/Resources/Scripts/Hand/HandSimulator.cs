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

public class HandSimulator : MonoBehaviour {
    private const float timeFactor = 10.0f;
    public static float[] handYawOffset = { 0.0f, 0.0f };
    public float localYawOffset = 0.0f;

    public GLOVE_HAND hand;

    public KeyCode mRotateLeft;
    public KeyCode mRotateRight;

    private Glove glove;
    private Transform gameRoot;
    private Transform modelRoot;
    private GameObject modelObject;
    private AnimationClip animationClip;

    /// <summary>
    /// Finds a deep child in a transform
    /// </summary>
    /// <param name="aParent">Transform to be searched</param>
    /// <param name="aName">Name of the (grand)child to be found</param>
    /// <returns></returns>
    private static Transform FindDeepChild(Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = FindDeepChild(child, aName);
            if (result != null)
                return result;
        }
        return null;
    }

    /// <summary>
    /// Constructor which loads the HandModel
    /// </summary>
    void Start() {
        // Ensure the library initialized correctly.
        Manus.ManusInit();

        // Initialize the glove and the associated skeletal model.
        glove = new Glove(hand);
        if (hand == GLOVE_HAND.GLOVE_LEFT) {
            modelObject = Resources.Load<GameObject>("Manus_Handv2_Left");
            animationClip = Resources.Load<AnimationClip>("Manus_Handv2_Left");
        } else {
            modelObject = Resources.Load<GameObject>("Manus_Handv2_Right");
            animationClip = Resources.Load<AnimationClip>("Manus_Handv2_Right");
        }

        gameRoot = FindDeepChild(gameObject.transform, "Wrist");
        modelRoot = FindDeepChild(modelObject.transform, "Wrist");

        modelObject.SetActive(true);
    }

    /// <summary>
    /// Updates a skeletal from glove data
    /// </summary>
    void FixedUpdate() {
        Quaternion q = glove.Quaternion;
        float[] fingers = glove.Fingers;
        gameObject.transform.localRotation = Quaternion.AngleAxis(handYawOffset[(int)hand], Vector3.up) * q
            * Quaternion.AngleAxis(localYawOffset, Vector3.up);

        for (int i = 0; i < 5; i++) {
            animationClip.SampleAnimation(modelObject, fingers[i] * timeFactor);
            Transform child = gameRoot, model = modelRoot;
            for (int j = 0; j < 4; j++) {
                string finger = "Finger_" + i.ToString() + j.ToString();
                child = child.Find(finger);
                model = model.Find(finger);
                child.localRotation = model.localRotation;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(mRotateLeft))
        {
            handYawOffset[(int)hand] -= 45;
        }

        if (Input.GetKeyUp(mRotateRight))
        {
            handYawOffset[(int)hand] += 45;
        }
    }
}
