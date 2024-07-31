using System;
using gs.chef.utilities.log;
using UnityEngine;

namespace Training.Controllers.Scripts
{
    public class ControllerScript : MonoBehaviour
    {
        [SerializeField] private Camera _sceneCamera;
        
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private float _step;

        private void Start()
        {
            transform.position = _sceneCamera.transform.position + _sceneCamera.transform.forward * 3f;
        }

        private void CenterCube()
        {
            _targetPosition = _sceneCamera.transform.position + _sceneCamera.transform.forward * 3f;
            _targetRotation = Quaternion.LookRotation(transform.position - _sceneCamera.transform.position);
            
            transform.position = Vector3.Lerp(transform.position, _targetPosition, _step);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _step);
        }

        private void Update()
        {
            _step = 5f * Time.deltaTime;

            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                CenterCube();
                CLog.Log(LogState.Game, "Centering cube");
            }

            if (OVRInput.Get(OVRInput.RawButton.RThumbstickLeft))
            {
                transform.Rotate(0, 5f *_step, 0);
                CLog.Log(LogState.Game, "Rotating cube left");
            }
            
            if (OVRInput.Get(OVRInput.RawButton.RThumbstickRight))
            {
                transform.Rotate(0, -5f *_step, 0);
                CLog.Log(LogState.Game, "Rotating cube right");
            }

            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                OVRInput.SetControllerVibration(1,1, OVRInput.Controller.RTouch);
                CLog.Log(LogState.Game, "Vibrating right controller");
            }

            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0f)
            {
                transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
                CLog.Log(LogState.Game, "Moving cube with left controller");
            }
        }
    }
}