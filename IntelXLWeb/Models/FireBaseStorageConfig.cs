﻿namespace IntelXLWeb.Models
{
    public class FireBaseStorageConfig
    {
        public string ApiKey { get; set; } = null!;
        public string Bucket { get; set; } = null!;     
        public string AuthEmail { get; set; } = null!;
        public string AuthPassword { get; set; } = null!;    
    }
}
