using System.Collections.Generic;

public class ADictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    public TKey findKey(TValue value)
    {
        TValue tempValue;
        foreach(TKey key in this.Keys)
        {
            if(this.TryGetValue(key, out tempValue))
            {
                if(tempValue.Equals(value))
                {
                    return key;
                }
            }
        }
        return default(TKey);
    }
}
