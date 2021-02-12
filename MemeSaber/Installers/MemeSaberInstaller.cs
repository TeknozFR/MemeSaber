using MemeSaber.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace MemeSaber.Installers
{
    class MemeSaberInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MemeViewController>().AsSingle();
        }
    }
}
