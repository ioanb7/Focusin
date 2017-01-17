using SnagFree.TrayApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Focusin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern void ShowWindow(IntPtr hWnd, int nCmdShow);

        private static GlobalKeyboardHook _globalKeyboardHook;

        //[STAThread]
        public MainWindow()
        {
            InitializeComponent();
            
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }


        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {

            if ((e.KeyboardData.VirtualCode < GlobalKeyboardHook.VkNumpad0 ||
                e.KeyboardData.VirtualCode > GlobalKeyboardHook.VkNumpad9) &&
                e.KeyboardData.VirtualCode != GlobalKeyboardHook.VkNumpadMultiply &&
                e.KeyboardData.VirtualCode != GlobalKeyboardHook.VkSubstract)
            {
                return;
            }

            // TODO: write your config here.d

            if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VkNumpad0)
            {
                PowerAllProcessesThatContains("mintty");
            }
            if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VkNumpad1)
            {
                PowerNthProcessThatContains("mintty", 0);
            }
            if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VkNumpad2)
            {
                PowerNthProcessThatContains("mintty", 1);
            }
            if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VkNumpad3)
            {
                PowerNthProcessThatContains("mintty", 2);
            }
            if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VkNumpad5)
            {
                PowerAllProcessesThatContains("notepad");
            }
            if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VkNumpadMultiply)
            {
                PowerUpProcess("UE4Editor");
            }
            if (e.KeyboardData.VirtualCode == GlobalKeyboardHook.VkSubstract)
            {
                //PowerAllProcessesThatContains("ApacheDirectoryStudio");
                PowerAllProcessesThatContains("hrome");
            }

            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
            {
                e.Handled = true;
            }
        }

        private void PowerAllProcessesThatContains(string name)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in processes)
            {
                if (p.ProcessName.Contains(name))
                {
                    SwitchToThisWindow(p.MainWindowHandle, true);
                }
            }
        }
        private void PowerNthProcessThatContains(string name, int n)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();

            int i = 0;
            foreach (System.Diagnostics.Process p in processes)
            {
                if (p.ProcessName.Contains(name))
                {
                    if (i == n)
                    {
                        SwitchToThisWindow(p.MainWindowHandle, true);
                        //ShowWindow(p.MainWindowHandle, 5);
                        return;
                    }
                    i++;
                }
            }
        }

        //Faster than PowerAllProcessesThatContains if you only have one process
        private void PowerUpProcess(string name)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(name);
            foreach (System.Diagnostics.Process p in processes)
            {
                if (p.ProcessName.Contains(name))
                {
                    SwitchToThisWindow(p.MainWindowHandle, true);
                    return;
                }
            }
        }
    }
}
