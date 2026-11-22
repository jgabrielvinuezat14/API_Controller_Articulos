namespace ControlInventario.Web.Api.Security
{
    public static class ApiKey
    {
        public static Guid Key
        {
            get;
            set;
        } = Guid.Empty;

        public static string Secret
        {
            get;
            set;
        } = string.Empty;
    }
}