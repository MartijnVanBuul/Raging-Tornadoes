//========= Copyright 2014, Valve Corporation, All rights reserved. ===========
//
// Purpose: For controlling in-game objects with tracked devices.
//
//=============================================================================

using UnityEngine;
using Valve.VR;

public class GloveTracker : MonoBehaviour
{
    public ETrackedControllerRole myRole;

    private uint index;
    public Transform origin; // if not set, relative to parent
    public bool isValid = false;
    // Manus Custom vars
    public Quaternion gOffsetRotation = new Quaternion(0, 0, 0, 1);
    public Vector3 gOffset = new Vector3(0, 0, 0);

    public KeyCode mRecalibrateButton = KeyCode.None;

    void Start()
    {
        GetGloveIndex();
    }

    void GetGloveIndex()
    {
        var system = OpenVR.System;

        if (system != null)
        {
            index = system.GetTrackedDeviceIndexForControllerRole(myRole);
        }
        else
        {
            index = 0;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(mRecalibrateButton))
        {
            // switch the roles around
            myRole = myRole == ETrackedControllerRole.LeftHand ? ETrackedControllerRole.RightHand : ETrackedControllerRole.LeftHand;

            // get the new glove index
            GetGloveIndex();
        }
    }

    private void OnNewPoses(params object[] args)
    {
        if (myRole == ETrackedControllerRole.Invalid)
            return;

        var i = (int)index;

        isValid = false;
        var poses = (Valve.VR.TrackedDevicePose_t[])args[0];

        if (poses.Length <= i)
            return;

        try
        {
            if (!poses[i].bDeviceIsConnected)
                return;
        }
        catch (System.IndexOutOfRangeException)
        {
            // retry to get the glove index
            GetGloveIndex();
            return;
        }

        if (!poses[i].bPoseIsValid)
            return;

        isValid = true;

        var pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

        if (origin != null)
        {
            pose = new SteamVR_Utils.RigidTransform(origin) * pose;
            pose.pos.x *= origin.localScale.x;
            pose.pos.y *= origin.localScale.y;
            pose.pos.z *= origin.localScale.z;
            transform.position = pose.pos + (pose.rot * gOffsetRotation) * gOffset;
            //transform.rotation = pose.rot;
        }
        else
        {
            transform.localPosition = pose.pos;
            transform.localRotation = pose.rot;
        }
    }

    void OnEnable()
    {
        SteamVR_Utils.Event.Listen("new_poses", OnNewPoses);
    }

    void OnDisable()
    {
        SteamVR_Utils.Event.Remove("new_poses", OnNewPoses);
    }
}
