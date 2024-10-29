namespace BusinessLayer.Authentication
{
    public class JwtOptions
    {
        public string Issuar { get; set; }
        public string Audience { get; set; }
        public byte LifeTimeMin { get; set; }
        public string SigningKey { get; set; }
        public string EncryptionKey { get; set; }
        

    }


}
