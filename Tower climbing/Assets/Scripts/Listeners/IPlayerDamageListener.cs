using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Listeners {

    public interface IPlayerDamageListener : Listener {

        void OnPlayerDamage();
    }
}
