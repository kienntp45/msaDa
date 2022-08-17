using NetMQ;
using NetMQ.Sockets;

namespace MicroServices.Samples.Shared.Common.Utils;

public class NetMQPull : IDisposable
{
    private readonly PullSocket _pullSocket;

    public NetMQPull(PullSocket pullSocket)
    {
        _pullSocket = pullSocket;
    }

    public void Dispose()
    {
        _pullSocket.Dispose();
    }

    public void ReceiveMessage(Action<string> callback, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var rs = _pullSocket.TryReceiveFrameString(out string message);
            if (rs)
            {
                callback(message);
            }
        }
    }
}