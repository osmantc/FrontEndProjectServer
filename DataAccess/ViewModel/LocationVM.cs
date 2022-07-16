using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndProjectServer.DataAccess.ViewModel
{
    public class MarkerVM
    {
        public string location;
        public Tooltip tooltip;

    }
    public class Tooltip
    {
        public bool isShown = false;
        public string text = string.Empty;
    }

}