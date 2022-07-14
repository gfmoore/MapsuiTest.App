using Mapsui;
using Mapsui.Projections;
using Mapsui.UI.Maui;
using System.Diagnostics;

namespace MapsuiTest;

public partial class MainPage : ContentPage
{

  IGeolocation geolocation;
  public Location location;

  public Mapsui.UI.Maui.MapControl mapControl;

  public MainPage(IGeolocation geolocation)
  {
    InitializeComponent();
    this.geolocation = geolocation;

    DoStuff();
  }

  public async void  DoStuff()  //I'm not exactly sure how best to structure this, but it seems to work??
  {
    location = new();
    await GetLocation();
    DrawMap();
  }
  public async Task GetLocation()
  {
    try
    {
      location = await geolocation.GetLocationAsync(new GeolocationRequest
      {
        DesiredAccuracy = GeolocationAccuracy.Best,
        Timeout = TimeSpan.FromSeconds(30)
      });
    }
    catch (Exception e)
    {
      Debug.WriteLine($" {e.Message}");
    }
  }

  public void DrawMap()
  {
    //setup mapsui
    mapControl = new();

    mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());

    //navigate to my location
    var smc = SphericalMercator.FromLonLat(location.Longitude, location.Latitude);
    mapControl.Map.Home = n => n.NavigateTo(new MPoint(smc.x, smc.y), mapControl.Map.Resolutions[16]);  //0 zoomed out-19 zoomed in
    
    //link to xaml
    mapViewElement.Map = mapControl.Map;
    
    //add a pin
    var myPin = new Pin(mapViewElement)
    {
      Position = new Position(location.Latitude, location.Longitude),
      Type = PinType.Pin,
      Label = "Zero point",
      Address = "Zero point",
      Scale = 0.7F,
      Color = Colors.Blue,
    };
    mapViewElement.Pins.Add(myPin);

  }

  private void Button_Clicked(object sender, EventArgs e)
  {
    Debug.WriteLine("Got here");
    var smc = SphericalMercator.FromLonLat(0, 0);
    mapControl.Map.Home = n => n.NavigateTo(new MPoint(0, 0), mapControl.Map.Resolutions[10]);  //0 zoomed out-19 zoomed in
  }
}

