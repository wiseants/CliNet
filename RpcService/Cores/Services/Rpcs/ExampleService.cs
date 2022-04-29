using AustinHarris.JsonRpc;
using NLog;

namespace RpsService.Cores.Services.Rpsc
{
    public class ExampleService : JsonRpcService
    {
        [JsonRpcMethod] // handles JsonRpc like : {'method':'incr','params':[5],'id':1}
        private int Incr(int i)
        {
            LogManager.GetCurrentClassLogger().Info("{0} + 1 = {1}", i, i + 1);

            return i + 1;
        }

        [JsonRpcMethod] // handles JsonRpc like : {'method':'decr','params':[5],'id':1}
        private int Decr(int i)
        {
            LogManager.GetCurrentClassLogger().Info("{0} - 1 = {1}", i, i - 1);

            return i - 1;
        }
    }
}
