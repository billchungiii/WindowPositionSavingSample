using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowPositionSavingSample.MultiScreens;

namespace WindowPositionSavingSample
{
    public static class WindowPositionExtension
    {
        private static List<ScreenInfo> Screens
        {
            get => MonitorWrapper.GetScreens();
        }

        private static WindowPositionModel CreateWindowPositionModel(this System.Windows.Window window)
        {
            IntPtr screenPtr = MonitorWrapper.GetScreenOfWindow(window);
            var screenInfo = Screens.FirstOrDefault(x => x.ScreenPtr == screenPtr);

            // Top 和 Left 是相對於螢幕零點的座標，所以要減去螢幕的左上角(零點)座標
            var model = new WindowPositionModel()
            {
                ScreenIndex = screenInfo.ScreenIndex,
                ScreenPtr = screenInfo.ScreenPtr,
                ScaleFactor = screenInfo.ScaleFactor,
                ScaledWidth = screenInfo.ScaledMonitorArea.Width,
                ScaledHeight = screenInfo.ScaledMonitorArea.Height,
                Top = window.Top - screenInfo.ScaledMonitorArea.Top,
                Left = window.Left - screenInfo.ScaledMonitorArea.Left
            };
            return model;

        }
        public static void SaveWindowPosition(this System.Windows.Window winodw)
        {
            var model = winodw.CreateWindowPositionModel();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            File.WriteAllText("position.json", json);
        }

        public static void LoadWindowPosition(this System.Windows.Window window)
        {
            if (File.Exists("position.json"))
            {
                var json = File.ReadAllText("position.json");
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<WindowPositionModel>(json);
                var screenIndex = model.ScreenIndex;

                if (screenIndex < Screens.Count)
                {
                    var screenInfo = Screens.First(x => x.ScreenIndex == screenIndex);
                    if (model.ScaledHeight == screenInfo.ScaledMonitorArea.Height)
                    {
                        window.Top = model.Top + screenInfo.ScaledMonitorArea.Top;
                    }
                    else
                    {
                        // 若現有螢幕的高度不等於儲存的螢幕高度，則依比例計算
                        window.Top = model.Top * (screenInfo.ScaledMonitorArea.Height / model.ScaledHeight) + screenInfo.ScaledMonitorArea.Top;
                    }

                    if (model.ScaledWidth == screenInfo.ScaledMonitorArea.Width)
                    {
                        window.Left = model.Left + screenInfo.ScaledMonitorArea.Left;
                    }
                    else
                    {
                        // 若現有螢幕的寬度不等於儲存的螢幕寬度，則依比例計算
                        window.Left = model.Left * (screenInfo.ScaledMonitorArea.Width / model.ScaledWidth) + screenInfo.ScaledMonitorArea.Left;
                    }


                }
                else
                {
                    // 如果螢幕數量變少，就會變成預設的螢幕
                    window.Top = 0;
                    window.Left = 0;
                }
            }
            else
            {
                // 如果沒有設定檔，就會變成預設的螢幕
                window.Top = 0;
                window.Left = 0;
            }
        }
    }
}
