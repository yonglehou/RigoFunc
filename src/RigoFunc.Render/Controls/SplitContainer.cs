
namespace RigoFunc.Render.Controls {
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public class SplitContainer : Control {
        private Grid grid1, grid2;
        private ColumnDefinition colDef1, colDef2;
        private RowDefinition rowDef1, rowDef2;
        private Thumb thumb;

        public UIElement Child1 {
            get { return (UIElement)GetValue(Child1Property); }
            set { SetValue(Child1Property, value); }
        }

        public static readonly DependencyProperty Child1Property =
            DependencyProperty.Register("Child1", typeof(UIElement), typeof(SplitContainer), new PropertyMetadata(null, OnChildChanged));

        public UIElement Child2 {
            get { return (UIElement)GetValue(Child2Property); }
            set { SetValue(Child2Property, value); }
        }

        public static readonly DependencyProperty Child2Property =
            DependencyProperty.Register("Child2", typeof(UIElement), typeof(SplitContainer), new PropertyMetadata(null, OnChildChanged));

        static SplitContainer() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitContainer), new FrameworkPropertyMetadata(typeof(SplitContainer)));

            OrientationProperty = DependencyProperty.Register("Orientation",
                typeof(Orientation), typeof(SplitContainer), new PropertyMetadata(Orientation.Horizontal, OnOrientationChanged));

            SwapChildrenProperty = DependencyProperty.Register("SwapChildren",
                typeof(bool), typeof(SplitContainer), new PropertyMetadata(false, OnSwapChildrenChanged));

            MinimumSizeProperty = DependencyProperty.Register("MinimumSize",
                typeof(double), typeof(SplitContainer), new PropertyMetadata(100.0, OnMinimumSizeChanged));
        }

        public override void OnApplyTemplate() {
            if (thumb != null) {
                thumb.DragStarted -= OnThumbDragStarted;
                thumb.DragDelta -= OnThumbDragDelta;
            }

            thumb = GetTemplateChild("PART_Thumb") as Thumb;

            if (thumb != null) {
                thumb.DragStarted += OnThumbDragStarted;
                thumb.DragDelta += OnThumbDragDelta;
            }

            if (grid1 != null) {
                grid1.Children.Clear();
            }
            grid1 = GetTemplateChild("PART_Grid1") as Grid;

            if (grid2 != null) {
                grid2.Children.Clear();
            }
            grid2 = GetTemplateChild("PART_Grid2") as Grid;

            colDef1 = GetTemplateChild("PART_ColDef1") as ColumnDefinition;
            colDef2 = GetTemplateChild("PART_ColDef2") as ColumnDefinition;

            rowDef1 = GetTemplateChild("PART_RowDef1") as RowDefinition;
            rowDef2 = GetTemplateChild("PART_RowDef2") as RowDefinition;

            base.OnApplyTemplate();
        }

        public static DependencyProperty OrientationProperty { get; set; }
        public static DependencyProperty SwapChildrenProperty { get; set; }
        public static DependencyProperty MinimumSizeProperty { get; set; }

        public Orientation Orientation {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public bool SwapChildren {
            get { return (bool)GetValue(SwapChildrenProperty); }
            set { SetValue(SwapChildrenProperty, value); }
        }

        public double MinimumSize {
            get { return (double)GetValue(MinimumSizeProperty); }
            set { SetValue(MinimumSizeProperty, value); }
        }

        static void OnChildChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            (obj as SplitContainer).OnChildChanged(e);
        }

        void OnChildChanged(DependencyPropertyChangedEventArgs e) {
            var targetGrid = (e.Property == Child1Property ^ this.SwapChildren) ? grid1 : grid2;

            if (targetGrid != null) {
                targetGrid.Children.Clear();

                if (e.NewValue != null) {
                    targetGrid.Children.Add(e.NewValue as UIElement);
                }
            }
        }

        static void OnOrientationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            (obj as SplitContainer).OnOrientationChanged((Orientation)e.OldValue, (Orientation)e.NewValue);
        }

        void OnOrientationChanged(Orientation oldOrientation, Orientation newOrientation) {
            // shouldn't be necessary, but...
            if (newOrientation == oldOrientation) {
                return;
            }

            if (newOrientation == Orientation.Horizontal) {
                colDef1.Width = rowDef1.Height;
                colDef2.Width = rowDef2.Height;

                colDef1.MinWidth = this.MinimumSize;
                colDef2.MinWidth = this.MinimumSize;

                rowDef1.Height = new GridLength(1, GridUnitType.Star);
                rowDef2.Height = new GridLength(0);

                rowDef1.MinHeight = 0;
                rowDef2.MinHeight = 0;

                thumb.Width = 6;
                thumb.Height = double.NaN;

                Grid.SetRow(thumb, 0);
                Grid.SetColumn(thumb, 1);

                Grid.SetRow(grid2, 0);
                Grid.SetColumn(grid2, 2);
            }
            else {
                rowDef1.Height = colDef1.Width;
                rowDef2.Height = colDef2.Width;

                rowDef1.MinHeight = this.MinimumSize;
                rowDef2.MinHeight = this.MinimumSize;

                colDef1.Width = new GridLength(2, GridUnitType.Star);
                colDef2.Width = new GridLength(0);

                colDef1.MinWidth = 0;
                colDef2.MinWidth = 0;

                thumb.Height = 6;
                thumb.Width = double.NaN;

                Grid.SetRow(thumb, 1);
                Grid.SetColumn(thumb, 0);

                Grid.SetRow(grid2, 2);
                Grid.SetColumn(grid2, 0);
            }
        }

        static void OnSwapChildrenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            (obj as SplitContainer).OnSwapChildrenChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        void OnSwapChildrenChanged(bool oldOrientation, bool newOrientation) {
            grid1.Children.Clear();
            grid2.Children.Clear();

            grid1.Children.Add(newOrientation ? this.Child2 : this.Child1);
            grid2.Children.Add(newOrientation ? this.Child1 : this.Child2);
        }

        static void OnMinimumSizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            (obj as SplitContainer).OnMinimumSizeChanged((double)e.OldValue, (double)e.NewValue);
        }

        void OnMinimumSizeChanged(double oldValue, double newValue) {
            if (this.Orientation == Orientation.Horizontal) {
                colDef1.MinWidth = newValue;
                colDef2.MinWidth = newValue;
            }
            else {
                rowDef1.MinHeight = newValue;
                rowDef2.MinHeight = newValue;
            }
        }

        private void OnThumbDragStarted(object sender, DragStartedEventArgs e) {
            if (this.Orientation == Orientation.Horizontal) {
                colDef1.Width = new GridLength(colDef1.ActualWidth, GridUnitType.Star);
                colDef2.Width = new GridLength(colDef2.ActualWidth, GridUnitType.Star);
            }
            else {
                rowDef1.Height = new GridLength(rowDef1.ActualHeight, GridUnitType.Star);
                rowDef2.Height = new GridLength(rowDef2.ActualHeight, GridUnitType.Star);
            }
        }

        private void OnThumbDragDelta(object sender, DragDeltaEventArgs e) {
            if (this.Orientation == Orientation.Horizontal) {
                double newWidth1 = Math.Max(0, colDef1.Width.Value + e.HorizontalChange);
                double newWidth2 = Math.Max(0, colDef2.Width.Value - e.HorizontalChange);

                colDef1.Width = new GridLength(newWidth1, GridUnitType.Star);
                colDef2.Width = new GridLength(newWidth2, GridUnitType.Star);
            }
            else {
                double newHeight1 = Math.Max(0, rowDef1.Height.Value + e.VerticalChange);
                double newHeight2 = Math.Max(0, rowDef2.Height.Value - e.VerticalChange);

                rowDef1.Height = new GridLength(newHeight1, GridUnitType.Star);
                rowDef2.Height = new GridLength(newHeight2, GridUnitType.Star);
            }
        }
    }
}
