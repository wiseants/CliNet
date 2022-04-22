using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    /// <summary>
    /// HTTP 요청 보내기 인터페이스.
    /// </summary>
    public interface IRequestSender
    {
        void SetClient(HttpClient client);

        Task<HttpResponseMessage> SendAsync(string path, object param);
    }
}
