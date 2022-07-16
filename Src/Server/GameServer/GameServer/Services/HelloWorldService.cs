using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Network;
using Common;
using SkillBridge.Message;

namespace GameServer.Services
{
    class HelloWorldService : Singleton<HelloWorldService>
    {
        public void Init()
        {

        }

        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFristTestRequest);
        }

        public void OnFristTestRequest(NetConnection<NetSession> sender, FirstTestRequest request)
        {
            Log.InfoFormat("FirstTestRequest: HelloWorld{0}", request.HelloWorld);
        }
    }
}
