namespace MetroFanfou.SDK.Http
{
    public interface IHttpRequestMethod
    {
        string Request(string uri, string postData);
    }
}