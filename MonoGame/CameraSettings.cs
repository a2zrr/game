 // Aucun attribut propre : tout est herite de Settings
    public class CameraSettings : Settings {
        public CameraSettings(int VisionRange, ViewAngle VisionAngle, int SuspicionTime)
            : base(VisionRange, VisionAngle, SuspicionTime)
        {
        }
    }