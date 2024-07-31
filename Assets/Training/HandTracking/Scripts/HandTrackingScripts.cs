using System;
using UnityEngine;

namespace Training.HandTracking.Scripts
{
    public class HandTrackingScripts : MonoBehaviour
    {
        [SerializeField] private Camera _sceneCamera;
        [SerializeField] private OVRHand _leftHand;
        [SerializeField] private OVRHand _rightHand;
        [SerializeField] private OVRSkeleton _skeleton;
        
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private float _step;
        private bool _isIndexFingerPinching;

        [SerializeField] private LineRenderer _line;
        private Transform _p0;
        private Transform _p1;
        private Transform _p2;

        private Transform _handIndexTipTransform;

        private void Start()
        {
            transform.position = _sceneCamera.transform.position + _sceneCamera.transform.forward * 1f;
        }

        private void PinchCube()
        {
            _targetPosition = _leftHand.transform.position - _leftHand.transform.forward * 0.4f;
            _targetRotation = Quaternion.LookRotation(transform.position - _leftHand.transform.position);
            
            transform.position = Vector3.Lerp(transform.position, _targetPosition, _step);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _step);
        }

        private void DrawCurve(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            _line.positionCount = 200;
            Vector3 B = Vector3.zero;
            float t = 0f;

            for (int i = 0; i < _line.positionCount; i++)
            {
                t += 0.005f;
                B = (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
                _line.SetPosition(i, B);
            }
        }

        private void Update()
        {
            _step = 5f * Time.deltaTime;

            if (_leftHand.IsTracked)
            {
                _isIndexFingerPinching = _leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
                if (_isIndexFingerPinching)
                {
                    _line.enabled = true;
                    PinchCube();

                    foreach (var b in _skeleton.Bones)
                    {
                        if (b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                        {
                            _handIndexTipTransform = b.Transform;
                            break;
                        }
                    }

                    _p0 = transform;
                    _p2 = _handIndexTipTransform;
                    _p1 = _sceneCamera.transform;
                    _p1.position += _sceneCamera.transform.forward * 0.8f;
                    
                    DrawCurve(_p0.position, _p1.position, _p2.position);
                }
                else
                {
                    _line.enabled = false;
                }

                /*if (_leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
                {
                    _p0 = _leftHand.Bones[(int) OVRSkeleton.BoneId.Hand_IndexTip].Transform;
                    _p1 = _leftHand.Bones[(int) OVRSkeleton.BoneId.Hand_MiddleTip].Transform;
                    _p2 = _leftHand.Bones[(int) OVRSkeleton.BoneId.Hand_RingTip].Transform;
                    DrawCurve(_p0.position, _p1.position, _p2.position);
                }*/
            }

            
        }
    }
}