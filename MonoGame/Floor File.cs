using System;
    public class FloorFile {
        private string file;

        public FloorFile(string file)
        {
            SetFile(file);
        }

        public string GetFile()
        {
            return file;
        }

        public void SetFile(string file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            this.file = file;
        }
    }