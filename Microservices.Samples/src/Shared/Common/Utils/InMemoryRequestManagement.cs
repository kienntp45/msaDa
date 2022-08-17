using System.Collections.Concurrent;

namespace MicroServices.Samples.Shared.Common.Utils;


public class InMemoryRequestManagement
{
    private ConcurrentDictionary<Guid, object> store;

    public InMemoryRequestManagement()
    {
        store = new ConcurrentDictionary<Guid, object>();
    }

    public Guid CreateRequest()
    {
        var requestId = Guid.NewGuid();
        var rs = store.TryAdd(requestId, null);
        if (rs)
            return requestId;
        return Guid.Empty;
    }

    public Guid SetRequest(Guid requestId)
    {
        var rs = store.TryAdd(requestId, null);
        if (rs)
            return requestId;
        return Guid.Empty;
    }

    public Guid SetResponse(Guid requestId, object response)
    {
        if (store.ContainsKey(requestId))
        {
            store.TryUpdate(requestId, response, null);
            return requestId;
        }
        return Guid.Empty;
    }

    public async Task<object> GetResponse(Guid requestId, int millisecondsTimeout = 10000)
    {
        // var start = DateTime.Now.Ticks;
        if (store.ContainsKey(requestId))
        {
            var taskDelay = Task.Delay(millisecondsTimeout);
            var taskWaitResponse = Task.FromResult(WaitResponse((requestId)));
            var task = await Task.WhenAny(taskDelay, taskWaitResponse);
            if (task == taskWaitResponse)
            {
                store.TryRemove(requestId, out object response);
                // var duration = DateTime.Now.Ticks - start;
                // Console.Write($"Consume {duration / 10000} ms");
                return await taskWaitResponse;
            }
        }
        return null;
    }

    private object WaitResponse(Guid requestId)
    {
        while (true)
        {
            store.TryGetValue(requestId, out object response);
            if (response != null)
            {
                return response;
            }
        }
    }
}