﻿using System.IO;

namespace CanFrameBuilder.Model
{
    public class Settings
    {
        public string? OutputDirectory { get; set; }
        public bool AddNamespace { get; set; }

        public Settings()
        {
            OutputDirectory = Directory.GetCurrentDirectory();
            AddNamespace = true;
        }
    }
}
