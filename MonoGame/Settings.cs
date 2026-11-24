using System;
    // Classe abstraite : attributs communs a CameraSettings et GuardSettings
    public abstract class Settings {
        private int VisionRange;          // PI
        private ViewAngle VisionAngle;
        private int SuspicionTime;        // PI

        protected Settings(int VisionRange, ViewAngle VisionAngle, int SuspicionTime)
        {
            SetVisionRange(VisionRange);
            SetVisionAngle(VisionAngle);
            SetSuspicionTime(SuspicionTime);
        }

        public int GetVisionRange()
        {
            return VisionRange;
        }

        public void SetVisionRange(int VisionRange)
        {
            // Verification que le nombre est positif
            if (VisionRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(VisionRange), "VisionRange doit etre un entier positif.");
            this.VisionRange = VisionRange;
        }

        public ViewAngle GetVisionAngle()
        {
            return new ViewAngle(VisionAngle);
        }

        public void SetVisionAngle(ViewAngle VisionAngle)
        {
            if (VisionAngle == null)
                throw new ArgumentNullException(nameof(VisionAngle));
            this.VisionAngle = VisionAngle;
        }

        public int GetSuspicionTime()
        {
            return SuspicionTime;
        }

        public void SetSuspicionTime(int SuspicionTime)
        {   // Verification que le nombre est positif
            if (SuspicionTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(SuspicionTime), "SuspicionTime doit etre un entier positif.");
            this.SuspicionTime = SuspicionTime;
        }
    }