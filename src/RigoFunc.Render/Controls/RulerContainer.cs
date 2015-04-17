
namespace RigoFunc.Render.Controls {
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class RulerContainer : ContentControl {
        private Grid _rootGrid;
        private Grid _gridLinesGrid;
        private Grid _innerGrid;
        private Canvas _rulerCanvas;

        const double RULER_WIDTH = 12;

        static RulerContainer() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RulerContainer), new FrameworkPropertyMetadata(typeof(RulerContainer)));

            ShowRulerProperty =
                DependencyProperty.Register("ShowRuler", typeof(bool), typeof(RulerContainer), new PropertyMetadata(true, OnShowRulerChanged));

            ShowGridLinesProperty =
                DependencyProperty.Register("ShowGridLines", typeof(bool), typeof(RulerContainer), new PropertyMetadata(true, OnShowGridLinesChanged));
        }

        public static DependencyProperty ShowRulerProperty { private set; get; }

        public static DependencyProperty ShowGridLinesProperty { private set; get; }

        public bool ShowRuler {
            set { SetValue(ShowRulerProperty, value); }
            get { return (bool)GetValue(ShowRulerProperty); }
        }

        public bool ShowGridLines {
            set { SetValue(ShowGridLinesProperty, value); }
            get { return (bool)GetValue(ShowGridLinesProperty); }
        }

        public override void OnApplyTemplate() {
            if (_rootGrid != null) {
                _rootGrid.SizeChanged -= OnRootGridSizeChanged;
            }

            _rootGrid = GetTemplateChild("RootPart") as Grid;
            if (_rootGrid != null) {
                _rootGrid.SizeChanged += OnRootGridSizeChanged;
            }

            if (_gridLinesGrid != null) {
                _gridLinesGrid.Children.Clear();
            }

            _gridLinesGrid = GetTemplateChild("GridLinesPart") as Grid;

            if (_rulerCanvas != null) {
                _rulerCanvas.Children.Clear();
            }

            _rulerCanvas = GetTemplateChild("RulerPart") as Canvas;

            _innerGrid = GetTemplateChild("InnerGridPart") as Grid;

            base.OnApplyTemplate();
        }

        void RedrawGridLines() {
            _gridLinesGrid.Children.Clear();

            if (!this.ShowGridLines)
                return;

            // Vertical grid lines every 1/4"
            for (double x = 24; x < _gridLinesGrid.ActualWidth; x += 24) {
                var line = new Line {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = _gridLinesGrid.ActualHeight,
                    Stroke = this.Foreground,
                    StrokeThickness = x % 96 == 0 ? 1 : 0.5,
                    StrokeDashArray = x % 96 == 0 ? new DoubleCollection { 6, 6 } : new DoubleCollection { 4, 4 },
                    StrokeDashOffset = x % 96 == 0 ? 3 : 2
                };
                _gridLinesGrid.Children.Add(line);
            }

            // Horizontal grid lines every 1/4"
            for (double y = 24; y < _gridLinesGrid.ActualHeight; y += 24) {
                var line = new Line {
                    X1 = 0,
                    Y1 = y,
                    X2 = _gridLinesGrid.ActualWidth,
                    Y2 = y,
                    Stroke = this.Foreground,
                    StrokeThickness = y % 96 == 0 ? 1 : 0.5,
                    StrokeDashArray = y % 96 == 0 ? new DoubleCollection { 6, 6 } : new DoubleCollection { 4, 4 },
                    StrokeDashOffset = y % 96 == 0 ? 3 : 2
                };
                _gridLinesGrid.Children.Add(line);
            }
        }

        void RedrawRuler() {
            _rulerCanvas.Children.Clear();

            if (!this.ShowRuler) {
                _innerGrid.Margin = new Thickness();
                return;
            }

            _innerGrid.Margin = new Thickness(RULER_WIDTH, RULER_WIDTH, 0, 0);

            // Ruler across the top
            for (double x = 0; x < _gridLinesGrid.ActualWidth - RULER_WIDTH; x += 12) {
                // Numbers every inch
                if (x > 0 && x % 96 == 0) {
                    var symbol = new TextBlock {
                        Text = (x / 96).ToString("F0"),
                        FontSize = RULER_WIDTH - 2
                    };

                    symbol.Measure(new Size());
                    Canvas.SetLeft(symbol, RULER_WIDTH + x - symbol.ActualWidth / 2);
                    Canvas.SetTop(symbol, 0);
                    _rulerCanvas.Children.Add(symbol);
                }
                // Tick marks every 1/8"
                else {
                    var line = new Line {
                        X1 = RULER_WIDTH + x,
                        Y1 = x % 48 == 0 ? 2 : 4,
                        X2 = RULER_WIDTH + x,
                        Y2 = x % 48 == 0 ? RULER_WIDTH - 2 : RULER_WIDTH - 4,
                        Stroke = this.Foreground,
                        StrokeThickness = 1
                    };
                    _rulerCanvas.Children.Add(line);
                }
            }

            // Heavy line underneath the tick marks
            var topLine = new Line {
                X1 = RULER_WIDTH - 1,
                Y1 = RULER_WIDTH - 1,
                X2 = _rulerCanvas.ActualWidth,
                Y2 = RULER_WIDTH - 1,
                Stroke = this.Foreground,
                StrokeThickness = 2
            };
            _rulerCanvas.Children.Add(topLine);

            // Ruler down the left side
            for (double y = 0; y < _gridLinesGrid.ActualHeight - RULER_WIDTH; y += 12) {
                // Numbers every inch
                if (y > 0 && y % 96 == 0) {
                    var symbol = new TextBlock {
                        Text = (y / 96).ToString("F0"),
                        FontSize = RULER_WIDTH - 2,
                    };

                    symbol.Measure(new Size());
                    Canvas.SetLeft(symbol, 2);
                    Canvas.SetTop(symbol, RULER_WIDTH + y - symbol.ActualHeight / 2);
                    _rulerCanvas.Children.Add(symbol);
                }
                // Tick marks every 1/8"
                else {
                    var line = new Line {
                        X1 = y % 48 == 0 ? 2 : 4,
                        Y1 = RULER_WIDTH + y,
                        X2 = y % 48 == 0 ? RULER_WIDTH - 2 : RULER_WIDTH - 4,
                        Y2 = RULER_WIDTH + y,
                        Stroke = this.Foreground,
                        StrokeThickness = 1
                    };
                    _rulerCanvas.Children.Add(line);
                }
            }

            var leftLine = new Line {
                X1 = RULER_WIDTH - 1,
                Y1 = RULER_WIDTH - 1,
                X2 = RULER_WIDTH - 1,
                Y2 = _rulerCanvas.ActualHeight,
                Stroke = this.Foreground,
                StrokeThickness = 2
            };
            _rulerCanvas.Children.Add(leftLine);
        }

        static void OnShowRulerChanged(DependencyObject obj,
                                       DependencyPropertyChangedEventArgs args) {
            (obj as RulerContainer).RedrawRuler();
        }

        static void OnShowGridLinesChanged(DependencyObject obj,
                                           DependencyPropertyChangedEventArgs args) {
            (obj as RulerContainer).RedrawGridLines();
        }

        void OnRootGridSizeChanged(object sender, SizeChangedEventArgs e) {
            RedrawRuler();
            RedrawGridLines();
        }
    }
}
