using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KNDefectMap
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// 清除控制項
        /// </summary>
        /// <param name="controls">控制項物件</param>
        /// <param name="dispose">是否dispose</param>
        public static void Clear(this Control.ControlCollection controls, bool dispose)
        {
            for (int ix = controls.Count - 1; ix >= 0; --ix)
            {
                if (dispose) controls[ix].Dispose();
                else controls.RemoveAt(ix);
            }
        }
    }
}
