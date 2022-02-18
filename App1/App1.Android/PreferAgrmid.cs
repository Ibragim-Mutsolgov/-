using App1;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(PreferAgrmid))]
namespace App1
{
    class PreferAgrmid: Interface1
    {
        public void set(String name)
        {
            if (get() == false)
            {
                Preferences.Set("Random", name);
            }
        }
        public bool get()
        {
            if (Preferences.ContainsKey("Random"))
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
            var s = Preferences.Get("Random", string.Empty);

            String f = s;

            return f;
        }
        public void clear()
        {
            if (get())
            {
                Preferences.Remove("Random");
            }
            
        }

        public void delete()
        {
            clear();
        }

        public bool boo(bool b)
        {
            return get();
        }

        public void set2(string name)
        {
            set(name);
        }
    }
}
