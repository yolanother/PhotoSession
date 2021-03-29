using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Rowlan.PhotoSession.ImageResolution;

namespace Rowlan.PhotoSession
{
    [System.Serializable]
    public class PhotoSessionSettings
    {
        /// <summary>
        /// The mode the photo session is currently in.
        /// </summary>
        public enum PhotoMode
        {
            Game,
            Photo
        }

        /// <summary>
        /// The type of photo that will be stored mono and VR modes
        /// </summary>
        public enum PhotoType
        {
            Flat,
            Mono360
        }

        // Image

        public ImageResolutionType resolution = ImageResolutionType.Game;

        public PhotoType photoType = PhotoType.Flat;

        public AspectRatio aspectRatio = AspectRatio.AR_16_9;

        public Output.Format outputFormat = Output.Format.JPG;

        public bool fieldOfViewOverride = false;

        [Range(0.1f, 180f)]
        public float fieldOfView = 60f;

        // Input

        // Input key which toggles the photo mode
        public KeyCode toggleKey = KeyCode.F12;

        // Camera Settings

        // The camera that will be used for the photo session. Usually the main camera
        public Camera photoCamera;

        // Normal camera movement speed
        public float movementSpeed = 4;

        // Camera movement speed when shift is pressed
        public float movementSpeedFast = 20;

        // Mouse look sensitivity
        public float freeLookSensitivity = 2f;

        // Normal mouse wheel zoom amount
        public float zoomSensitivity = 5;

        // Mouse wheel zoom amount when shift is pressed
        public float zoomSensitivityFast = 20;

        // Pause

        // the scaled time factor that's being used in photo mode
        [Range(0f,1f)]
        public float pauseTime = 0f;

        // Initialization

        // Keep the photo camera position and rotation the way it was in the previous session
        public bool reusePreviousCameraTransform = false;

        // Objects (e. g. Scripts) which should be disabled when photo mode is activated
        public Object[] disabledComponents;

        // User Interface

        // An optional canvas with a white stretched image. The alpha value of the image will be used to simulate a flash effect. The canvas can be hidden
        public Canvas canvas = null;

        #region HDRP

        public Hdrp.DepthOfFieldSettings hdrpDepthOfFieldSettings;

        #endregion HDRP

        #region URP

        public Urp.DepthOfFieldSettings urpDepthOfFieldSettings;

        #endregion URP

    }
}