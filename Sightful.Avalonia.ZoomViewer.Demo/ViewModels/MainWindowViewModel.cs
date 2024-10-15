using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Sightful.Avalonia.ZoomViewer.Demo.ViewModels;

public sealed partial class MainWindowViewModel : ViewModelBase
{
	public IEnumerable<Bitmap> Images { get; }

	public MainWindowViewModel()
	{
		var files = Directory.GetFiles("Images");
		Images = files.Select(file => new Bitmap(file)).ToList();
	}

	[ObservableProperty] private Bitmap? _selectedImage;
}