using UnityEngine;
using System.Collections;

/// <summary> Measures the velocity of each hand bit. Used to smack physics-objects with. </summary>
public class HandVelocity : MonoBehaviour {

    public Vector3 mVelocity;
    protected Vector3 mLastPos;
    public float mSmoothSpeed;

    void Awake()
    {
        mLastPos = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 tNewVelocity = (transform.position - mLastPos) / Time.fixedDeltaTime;
        mVelocity = Vector3.Lerp(mVelocity, tNewVelocity, mSmoothSpeed * Time.fixedDeltaTime);
        mLastPos = transform.position;
    }
}
