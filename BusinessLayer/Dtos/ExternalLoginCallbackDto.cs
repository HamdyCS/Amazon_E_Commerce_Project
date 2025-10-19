namespace BusinessLayer.Dtos
{
    public class ExternalLoginCallbackDto
    {
        public string returnUrl { get; set; }
        public TokenDto tokenDto { get; set; }
    }
}
