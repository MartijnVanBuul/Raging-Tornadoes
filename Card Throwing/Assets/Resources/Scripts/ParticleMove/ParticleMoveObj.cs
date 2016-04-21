using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary> Script that keeps track of its own velocity, and moves particles with it. </summary>
public class ParticleMoveObj : MonoBehaviour {

    public static List<ParticleMoveObj> mParticleMoveObjs;

    public float mRadius;                           // Radius of object
    protected Vector3 mLastPos;                     // Last position
    public float mDeltaLerp;                        // Lerp of delta to smooth it out a bit
    protected Vector3 mDelta;                       // Object delta, otherwise known as Velocity
    public Vector3 mObjDelta
    {
        get
        {
            return mDelta;
        }
    }
    public float mForce;                            // Applied force from this object
    
    // Wind rushing effect
    public AudioSource mWindRushAudio;
    public float mSoundMinVelocity;         // Minimum velocity before sound is heard
    public float mWindSoundVolMult;         // Volume multiplier
    public float mWindSoundPitchMult;       // Pitch multiplier
    public float mWindSoundVolMax;          // Maximum volume
    public float mWindSoundPitchMax;        // Maximum pitch
    protected float mSoundVel;
    public float mSoundLerp;                // Volume and pitch lerp speeds

    void Awake()
    {
        // Keep track of all moving objects in a static list. Manager would be overkill for this small a feature.
        if (mParticleMoveObjs == null)
            mParticleMoveObjs = new List<ParticleMoveObj>();
        mParticleMoveObjs.Add(this);

        mLastPos = transform.position;
    }

    void OnDestroy()
    {
        mParticleMoveObjs.Remove(this);
    }

    void Update()
    {
        // Update velocity
        mDelta = Vector3.Lerp(mDelta, transform.position - mLastPos, mDeltaLerp * Time.deltaTime);
        mLastPos = transform.position;

        // Adjust volume and pitch based on velocity.
        if (mWindRushAudio != null)
        {
            mSoundVel = Mathf.Lerp(mSoundVel, mDelta.magnitude, mSoundLerp * Time.deltaTime);
            if (mSoundVel < mSoundMinVelocity && mWindRushAudio.isPlaying)
            {
                mWindRushAudio.Pause();
            }
            else if (mSoundVel > mSoundMinVelocity)
            {
                if (!mWindRushAudio.isPlaying) mWindRushAudio.Play();
                float tUseSound = mSoundVel - mSoundMinVelocity;
                mWindRushAudio.volume = Mathf.Min(tUseSound * mWindSoundVolMult, mWindSoundVolMax);
                mWindRushAudio.pitch = Mathf.Min(tUseSound * mWindSoundPitchMult, mWindSoundPitchMax);
            }
        }
    }
}
