using Avalonia;
using Avalonia.Controls;

namespace Sightful.Avalonia.ZoomViewer;

public class ZoomViewer : ContentControl
{
	public static readonly StyledProperty<double> ZoomProperty = ZoomContentPresenter.ZoomProperty.AddOwner<ZoomViewer>();

	public double Zoom
	{
		get => GetValue(ZoomProperty);
		set => SetValue(ZoomProperty, value);
	}
}