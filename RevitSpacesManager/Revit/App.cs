using Autodesk.Revit.UI;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RevitSpacesManager.Revit
{
    public class App : IExternalApplication
    {
        private RibbonPanel _panel;
        private readonly string _tabName = "SynSys";

        public Result OnStartup(UIControlledApplication revit)
        {
            CreateRibbonTab(revit);
            CreateRibbonPanel(revit);
            CreateButton();

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication revit)
        {
            return Result.Succeeded;
        }


        private void CreateRibbonTab(UIControlledApplication revit)
        {
            try
            {
                revit.CreateRibbonTab(_tabName);
            }
            catch { }
        }

        private void CreateRibbonPanel(UIControlledApplication revit)
        {
            string panelName = "Model";

            foreach (RibbonPanel panel in revit.GetRibbonPanels(_tabName))
            {
                if (panel.Name == panelName)
                {
                    _panel = panel;
                    return;
                }
            }

            _panel = revit.CreateRibbonPanel(_tabName, panelName);
        }

        private void CreateButton()
        {
            string buttonText = "Spaces Manager";
            string buttonToolTip = "Создание пространств и помещений в модели MEP на основе комнат из присоединённых моделей.\n" +
                                  $"v{typeof(App).Assembly.GetName().Version}";

            var buttonData = new PushButtonData(
                typeof(Command).Namespace,
                buttonText,
                typeof(Command).Assembly.Location,
                typeof(Command).FullName
            );

            var pushButton = _panel.AddItem(buttonData) as PushButton;
            pushButton.ToolTip = buttonToolTip;
            pushButton.LargeImage = GetImageSourceByBitMapFromResource(Properties.Resources.LargeImage);
            pushButton.Image = GetImageSourceByBitMapFromResource(Properties.Resources.Image);
        }

        private ImageSource GetImageSourceByBitMapFromResource(Bitmap source)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                source.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
        }
    }
}
