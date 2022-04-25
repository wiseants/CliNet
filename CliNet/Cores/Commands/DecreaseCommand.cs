using AustinHarris.JsonRpc;
using CliNet.Interfaces;
using CommandLine;
using Common.Network;
using System;
using System.Net;
using System.Reactive;
using System.Text;

namespace CliNetCore.Cores.Commands
{
    [Verb("decrease", HelpText = "Decrease number")]
    public class DecreaseCommand : IAction
    {
        public bool IsValid => true;

        [Option('t', "target", Required = true, HelpText = "target number.")]
        public int TargetNumber
        {
            get;
            set;
        }

        public int Action()
        {
            JsonRpcClient client = new JsonRpcClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8055), Encoding.UTF8);

            IObservable<JsonResponse<int>> result = client.Invoke<int>("Decr", new object[] { TargetNumber });
            result.Subscribe(new AnonymousObserver<JsonResponse<int>>(
                x =>
                {
                    Console.WriteLine(string.Format("{0}", x.Result));
                },
                (ex) =>
                {
                    Console.WriteLine(string.Format("Exception occurred. Message({0})", ex.Message));
                }, () => { })).Dispose();

            return 1;
        }
    }
}
