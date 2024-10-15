using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;

namespace Sightful.Avalonia.ZoomViewer;

internal static class Converters
{
	public static FuncValueConverter<bool, ScrollBarVisibility> BoolToScrollBarVisibility { get; } =
		new(b => b ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden);
}