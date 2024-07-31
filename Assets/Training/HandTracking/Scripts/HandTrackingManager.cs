using System;
using System.Collections.Generic;
using System.Linq;
using gs.chef.utilities.log;
using UnityEngine;

namespace Training.HandTracking.Scripts
{
    public class HandTrackingManager : MonoBehaviour
    {
        [SerializeField] private Camera _sceneCamera;
        [SerializeField] private OVRHand _leftHand;
        [SerializeField] private OVRHand _rightHand;
        [SerializeField] private OVRSkeleton _leftHandskeleton;
        [SerializeField] private OVRSkeleton _rightHandskeleton;

        private void Start()
        {
            transform.position = _sceneCamera.transform.position + _sceneCamera.transform.forward * 1f;
        }

        private void Update()
        {
            //CLog.Log(LogState.Game, "######### Checking for pinching fingers");

            if (_leftHand.IsTracked)
            {
                var leftEnums = Enum.GetValues(typeof(OVRHand.HandFinger));
                
                List<(OVRHand.HandFinger finger, bool isPinch)> fingerPinchStates =
                    new List<(OVRHand.HandFinger finger, bool isPinch)>();

                foreach (var leftEnum in leftEnums)
                {
                    if (leftEnum.Equals(OVRHand.HandFinger.Thumb))
                        continue;

                    fingerPinchStates.Add(((OVRHand.HandFinger)leftEnum, _leftHand.GetFingerIsPinching((OVRHand.HandFinger)leftEnum)));
                }

                if (fingerPinchStates.Any())
                    fingerPinchStates.ToList().FindAll(s => s.isPinch).ForEach(s =>
                        CLog.Log(LogState.Game, $"Left -> {s.finger} is Pinching"));
            }

            if (_rightHand.IsTracked)
            {
                var rightEnums = Enum.GetValues(typeof(OVRHand.HandFinger));

                List<(OVRHand.HandFinger finger, bool isPinch)> fingerPinchStates =
                    new List<(OVRHand.HandFinger finger, bool isPinch)>();
                
                foreach (var rightEnum in rightEnums)
                {
                    if (rightEnum.Equals(OVRHand.HandFinger.Thumb))
                        continue;

                    fingerPinchStates.Add(((OVRHand.HandFinger)rightEnum, _rightHand.GetFingerIsPinching((OVRHand.HandFinger)rightEnum)));
                }

                if (fingerPinchStates.Any())
                    fingerPinchStates.ToList().FindAll(s => s.isPinch).ForEach(s =>
                        CLog.Log(LogState.Game, $"Right -> {s.finger} is Pinching"));
            }
        }
    }
}