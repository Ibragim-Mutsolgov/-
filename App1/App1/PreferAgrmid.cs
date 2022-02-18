using System;
using Xamarin.Essentials;

namespace App1
{
    class PreferAgrmid
    {
        public void set(String name)
        {
            Preferences.Set("RandomText", name);
        }
        public bool get()
        {
            if (Preferences.ContainsKey("RandomText"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public String getName()
        {
            var s = Preferences.Get("RandomText", string.Empty);

            String f = s;

            return f;
        }
        public void clear()
        {
            Preferences.Clear();
        }
    }
}
