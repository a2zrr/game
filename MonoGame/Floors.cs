using System;
using System.Collections.Generic;
  // Composition : Floors contient 1..* FloorFile
    public class Floors {
        private List<FloorFile> floorFiles;

        public Floors(List<FloorFile> floorFiles)
        {
            SetFloorFiles(floorFiles);
        }

        public List<FloorFile> GetFloorFiles()
        {
            return floorFiles;
        }

        public void SetFloorFiles(List<FloorFile> floorFiles)
        {
            if (floorFiles == null)
                throw new ArgumentNullException(nameof(floorFiles));
            if (floorFiles.Count == 0)
                throw new ArgumentException("Floors doit contenir au moins un FloorFile (1..*).", nameof(floorFiles));
            this.floorFiles = floorFiles;
        }
    }