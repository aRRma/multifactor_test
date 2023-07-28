using multifactor_test.Models;
using System.Collections.Concurrent;

namespace multifactor_test.Services
{
    public class AccessService
    {
        private const int DEFAULT_EXPIRE_SEC = 20;
        private readonly ConcurrentDictionary<string, AccessContext> accessRegistry = new();

        public void AddInRegistry(string resource, ActionType actionType)
        {
            if (accessRegistry.TryGetValue(resource, out var context))
            {
                context.ActionType = actionType;
                context.Resource = resource;
            }
            else
            {
                accessRegistry.TryAdd(resource, new AccessContext()
                {
                    ActionType = actionType,
                    Resource = resource
                });
            }
        }

        public async Task<bool> CheckAccessInRegistryAsync(string resource)
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(DEFAULT_EXPIRE_SEC));

            while (true)
            {
                accessRegistry.TryGetValue(resource, out var context);

                if (context is not null)
                {
                    var result = context.ActionType == ActionType.Grant ? true : false;
                    accessRegistry.TryRemove(new(resource, context));
                    return result;
                }

                await Task.Delay(1, cts.Token);
            }
        }
    }
}