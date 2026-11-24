using System;
  //value compris entre 0 et 180
    public class ViewAngle {
        private int value;

        public ViewAngle(int value)
        {
            SetValue(value);
        }

        public int GetValue()
        {
            return value;
        }

        public void SetValue(int value)
        {
            if (value < 0 || value > 180)
                throw new ArgumentOutOfRangeException(nameof(value), "value doit etre compris entre 0 et 180.");
            this.value = value;
        }
    }