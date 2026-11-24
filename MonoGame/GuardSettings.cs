using System;
    public class GuardSettings : Settings {
        private int Speed;          // PI
        private int TouchDistance;  // PI

        public GuardSettings(int Speed, int VisionRange, ViewAngle VisionAngle, int SuspicionTime, int TouchDistance)
            : base(VisionRange, VisionAngle, SuspicionTime)
        {
            SetSpeed(Speed);
            SetTouchDistance(TouchDistance);
        }

        public int GetSpeed()
        {
            return Speed;
        }

        public void SetSpeed(int Speed)
        {
            if (Speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(Speed), "Speed doit etre un entier positif.");
            this.Speed = Speed;
        }

        public int GetTouchDistance()
        {
            return TouchDistance;
        }

        public void SetTouchDistance(int TouchDistance)
        {
            if (TouchDistance <= 0)
                throw new ArgumentOutOfRangeException(nameof(TouchDistance), "TouchDistance doit etre un entier positif.");
            this.TouchDistance = TouchDistance;
        }
    }