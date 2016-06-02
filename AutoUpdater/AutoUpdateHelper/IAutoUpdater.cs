using System;
using System.Collections.Generic;
using System.Text;

namespace KellAutoUpdater
{
    public interface IAutoUpdater
    {
        void Update();

        void RollBack();
    }
}
