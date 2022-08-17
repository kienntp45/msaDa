using NetMQ;
using NetMQ.Sockets;

namespace MicroServices.Samples.Shared.Common.Utils;

public class NetMQPush : IDisposable
{
    private readonly PushSocket _pushSocket;

    public NetMQPush(PushSocket pushSocket)
    {
        _pushSocket = pushSocket;
    }

    public void Dispose()
    {
        _pushSocket.Dispose();
    }
    public bool SendMessage(string message)
    {
            if (!string.IsNullOrEmpty(message))
            {
            return  _pushSocket.TrySendFrame(message);
            }
        return false;
    }
}