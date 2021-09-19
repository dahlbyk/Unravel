namespace System.Web
{
    public interface IHttpContextAccessor
    {
        HttpContextBase HttpContext { get; }
    }
}
