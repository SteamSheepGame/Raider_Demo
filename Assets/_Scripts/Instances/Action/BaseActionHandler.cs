using System;

namespace Demo.Core
{
    public abstract class BaseActionHandler: IActionHandler
    {
        protected string GetParam(ActionBinding binding, string key)
        {
            foreach (var kv in binding.Params)
            {
                if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
                    return kv.Value;
            }
            
            throw new Exception($"Parameter '{key}' missing for action {binding.Id}");
        }

        public abstract void Execute(ActionBinding binding);
    }
}