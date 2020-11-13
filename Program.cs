using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoBoard
{
    public static class Program
    {
        public static MemoBoard mb;
        public static Dictionary<String, Label> li = new Dictionary<String, Label>();
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary> 
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mb = new MemoBoard();
            try
            {
                Application.Run(mb);
            }catch (Exception E)
            { }
        }
    }
}
