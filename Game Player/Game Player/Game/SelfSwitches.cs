using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Game
{
    public class SelfSwitches
    {
        Dictionary<object, bool> data = new Dictionary<object, bool>();

        public bool this[object key]
        {
            get 
            {
                bool outVal = false;
                if (data.ContainsKey(key))
                    data.TryGetValue(key, out outVal);

                return outVal;
            }
            set
            {
                if (data.ContainsKey(key))
                    data.Remove(key);

                data.Add(key, value);
            }

        }
    }
}
