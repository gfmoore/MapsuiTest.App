using Mapsui.UI.Maui;

namespace MapsuiTest;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

    var mapControl = new Mapsui.UI.Maui.MapControl();
    mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
    //Content = mapControl;
    mapViewElement.Map = mapControl.Map;
  }

}

