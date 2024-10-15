using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media.Immutable;
using Avalonia.Metadata;

namespace Sightful.Avalonia.ZoomViewer;

public sealed class ZoomContentPresenter : Control
{
	public static readonly StyledProperty<Control?> ChildProperty = Decorator.ChildProperty.AddOwner<ZoomContentPresenter>();

	public static readonly StyledProperty<double> ZoomProperty =
		AvaloniaProperty.Register<ZoomViewer, double>(nameof(Zoom), 1, defaultBindingMode: BindingMode.TwoWay);

	static ZoomContentPresenter()
	{
		ClipToBoundsProperty.OverrideDefaultValue<ZoomContentPresenter>(true);
		AffectsMeasure<ZoomContentPresenter>(ZoomProperty);
		AffectsArrange<ZoomContentPresenter>(ZoomProperty);
	}

	[Content]
	public Control? Child
	{
		get => GetValue(ChildProperty);
		set => SetValue(ChildProperty, value);
	}

	public double Zoom
	{
		get => GetValue(ZoomProperty);
		set => SetValue(ZoomProperty, value);
	}

	public ZoomContentPresenter()
	{
		_container = new Container
		{
			RenderTransformOrigin = RelativePoint.TopLeft
		};
		((ISetLogicalParent)_container).SetParent(this);
		VisualChildren.Add(_container);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == ChildProperty)
		{
			var (oldChild, newChild) = change.GetOldAndNewValue<Control?>();

			if (oldChild is not null)
			{
				((ISetLogicalParent)oldChild).SetParent(null);
				LogicalChildren.Remove(oldChild);
			}

			_container.Child = newChild;

			if (newChild is not null)
			{
				((ISetLogicalParent)newChild).SetParent(this);
				LogicalChildren.Add(newChild);
			}

			InvalidateMeasure();
		}
		else if (change.Property == ZoomProperty)
		{
			_container.RenderTransform = new ImmutableTransform(Matrix.CreateScale(Zoom, Zoom));
		}
	}

	protected override Size MeasureOverride(Size availableSize)
	{
		_container.Measure(Size.Infinity);
		return _container.DesiredSize * Zoom;
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		_container.Arrange(new Rect(_container.DesiredSize));
		return _container.DesiredSize * Zoom;
	}

	private readonly Container _container;

	private sealed class Container : Control
	{
		public Control? Child
		{
			get => _child;
			set
			{
				if (_child == value)
					return;
				if (_child is not null)
					VisualChildren.Remove(_child);
				_child = value;
				if (_child is not null)
					VisualChildren.Add(_child);
				InvalidateMeasure();
			}
		}

		private Control? _child;
	}
}