
namespace RigoFunc.Render.Chromes {
    using System.Windows;
    using System.Windows.Controls;

    public class ResizeRotateChrome : Control {
        static ResizeRotateChrome() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeRotateChrome), new FrameworkPropertyMetadata(typeof(ResizeRotateChrome)));
        }
    }
}
