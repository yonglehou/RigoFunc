
namespace RigoFunc.Render.Chromes {
    using System.Windows;
    using System.Windows.Controls;

    public class ResizeChrome : Control {
        static ResizeChrome() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeChrome), new FrameworkPropertyMetadata(typeof(ResizeChrome)));
        }
    }
}
