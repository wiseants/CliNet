﻿using AustinHarris.JsonRpc;

namespace CliNet.Cores.Services
{
    public class ExampleService : JsonRpcService
    {
        [JsonRpcMethod] // handles JsonRpc like : {'method':'incr','params':[5],'id':1}
        private int Incr(int i)
        {
            return i + 1;
        }

        [JsonRpcMethod] // handles JsonRpc like : {'method':'decr','params':[5],'id':1}
        private int Decr(int i)
        {
            return i - 1;
        }
    }
}
