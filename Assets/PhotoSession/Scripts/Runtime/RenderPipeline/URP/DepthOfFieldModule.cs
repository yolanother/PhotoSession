using System;
using UnityEngine;
using UnityEngine.Rendering;

#if USING_URP
using UnityEngine.Rendering.Universal;
#endif

namespace Rowlan.PhotoSession.Urp
{
    public class DepthOfFieldModule : IPhotoSessionModule
    {

#if USING_URP
        private PhotoSession photoSession;
        private Volume volume;
        private DepthOfField depthOfField;
        private DepthOfFieldSettings originalSettings;

        private bool featureActive = false;

        private DepthOfFieldMode defaultDepthOfFieldMode = DepthOfFieldMode.Bokeh;

        private bool hasTarget;
        private float hitDistance;
#endif

        public void Start(PhotoSession photoSession)
        {
#if USING_URP
            this.photoSession = photoSession;

			Urp.DepthOfFieldSettings dofSettings = photoSession.settings.urpDepthOfFieldSettings;

            // check if user enabled the feature
            if (!dofSettings.featureEnabled)
                return;

            // get the dof profile
            featureActive = dofSettings.volume.profile.TryGet(out depthOfField);
            if (!featureActive)
            {
                Debug.Log("DepthOfField enabled, but volume undefined");
                return;
            }

            this.volume = dofSettings.volume;

#endif
        }

        public void OnEnable()
        {
#if USING_URP
            // backup settings
            originalSettings = new DepthOfFieldSettings(volume, depthOfField);

            // activate the DoF feature if all criteria for that are met
            if (featureActive)
            {
                volume.gameObject.SetActive(true);
                depthOfField.active = true;
            }
#endif
        }

        public void OnDisable()
        {
#if USING_URP

            // restore settings (including active state)
            originalSettings.Restore(volume, depthOfField);

#endif
        }

        public void OnDrawGizmos()
        {
#if USING_URP
#endif
        }


        public void Update()
        {
#if USING_URP
            if (!featureActive)
                return;

            hasTarget = photoSession.autoFocusData.IsTargetInRange();
            hitDistance = photoSession.autoFocusData.minDistance;

            // Debug.Log(string.Format("target in range: {0}, hit distance: {1}, max ray length: {2}", hasTarget, hitDistance, photoSession.autoFocusData.maxRayLength));

            UpdateFocus(hasTarget, hitDistance);
#endif
        }

        #region Private Methods and Classes

#if USING_URP

        private void UpdateFocus(bool hasTarget, float hitDistance)
        {
            UpdateFocusMode(hasTarget);
            UpdateFocusSettings(hitDistance);
        }

        private void UpdateFocusMode(bool hasTarget)
        {
            if (hasTarget)
            {
                depthOfField.mode.overrideState = true;
                depthOfField.mode.value = defaultDepthOfFieldMode;
            }
            else
            {
                depthOfField.mode.overrideState = true;
                depthOfField.mode.value = DepthOfFieldMode.Off;
            }

        }

        private void UpdateFocusSettings(float targetDistance)
        {
			Urp.DepthOfFieldSettings dofSettings = photoSession.settings.urpDepthOfFieldSettings;

            switch (depthOfField.mode.value)
            {
                case DepthOfFieldMode.Off:
                    // nothing to do
                    break;

                case DepthOfFieldMode.Gaussian:
                    throw new System.NotImplementedException("Gaussian DoF mode not implemented");

                case DepthOfFieldMode.Bokeh:
                    
                    depthOfField.focusDistance.overrideState = true;
                    depthOfField.focusDistance.value = targetDistance + dofSettings.focusDistanceOffset;

                    depthOfField.focalLength.overrideState = true;
                    depthOfField.focalLength.value = dofSettings.focalLength;

                    depthOfField.aperture.overrideState = true;
                    depthOfField.aperture.value = dofSettings.aperture;

                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unsupported enum " + depthOfField.mode.value);
            }
        }

        private class DepthOfFieldSettings
        {
            private bool volumeActive;
            private bool depthOfFieldActive;

            private DepthOfFieldModeParameter focusMode;

            private FloatParameter focusDistance;
            private FloatParameter focalLength;
            private FloatParameter aperture;

            public DepthOfFieldSettings(Volume volume, DepthOfField depthOfField)
            {

                if (!depthOfField)
                    return;

                volumeActive = volume.gameObject.activeInHierarchy;

                depthOfFieldActive = depthOfField.active; // important: don't use IsActive(), it's something different

                focusMode = new DepthOfFieldModeParameter(depthOfField.mode.value, depthOfField.mode.overrideState);

                focusDistance = new FloatParameter(depthOfField.focusDistance.value, depthOfField.focusDistance.overrideState);
                focalLength = new FloatParameter(depthOfField.focalLength.value, depthOfField.focalLength.overrideState);
                aperture = new FloatParameter(depthOfField.aperture.value, depthOfField.aperture.overrideState);


            }

            public void Restore(Volume volume, DepthOfField depthOfField)
            {

                if (!depthOfField)
                    return;

                volume.gameObject.SetActive( volumeActive);

                depthOfField.active = depthOfFieldActive;

                depthOfField.mode.overrideState = focusMode.overrideState;
                depthOfField.mode.value = focusMode.value;

                depthOfField.focusDistance.overrideState = focusDistance.overrideState;
                depthOfField.focusDistance.value = focusDistance.value;

                depthOfField.focalLength.value = focalLength.value;
                depthOfField.focalLength.value = focalLength.value;

                depthOfField.aperture.overrideState = aperture.overrideState;
                depthOfField.aperture.value = aperture.value;

            }
        }
#endif
        #endregion Private Methods and Classes

    }
}