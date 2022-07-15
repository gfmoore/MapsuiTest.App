using Mapsui;
using Mapsui.Projections;
using Mapsui.UI.Maui;
using System.Diagnostics;

namespace MapsuiTest;

public partial class MainPage : ContentPage
{

  IGeolocation geolocation;
  public Location location;

  //public Mapsui.UI.Maui.MapControl mapControl;
  MapControl mapControl = new MapControl();

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
    mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());


    //link to xaml
    mapViewElement.Map = mapControl.Map;

    //navigate to my location
    var smc = SphericalMercator.FromLonLat(location.Longitude, location.Latitude);
    mapViewElement.Navigator.NavigateTo(new MPoint(smc.x, smc.y), mapControl.Map.Resolutions[16]);  //0 zoomed out-19 zoomed in

    //add handlers
    mapViewElement.MapClicked += OnMapClicked;
    mapViewElement.PinClicked += OnPinClicked;
    Compass.ReadingChanged += Compass_ReadingChanged;

    //add a pin
    AddPin(location.Latitude, location.Longitude, Colors.Blue);

  }

  private void OnMapClicked(object sender, MapClickedEventArgs e)
  {
    Debug.WriteLine("Map clicked");
  }

  private void OnPinClicked(object sender, PinClickedEventArgs e)
  {
    Debug.WriteLine("Pin clicked");
  }

  public void AddPin(double latitude, double longitude, Color c)
  {
    var myPin = new Pin(mapViewElement)
    {
      Position = new Position(latitude, longitude),
      Type = PinType.Pin,
      Label = "some text",
      Address = "more text",
      Scale = 0.7F,
      Color = c,
    };
    mapViewElement.Pins.Add(myPin);
  }

  private void Button_Clicked(object sender, EventArgs e)
  {
    Debug.WriteLine("Got here");
    var smc = SphericalMercator.FromLonLat(0, 0);

    mapViewElement.Navigator.NavigateTo(new MPoint(0, 0), mapControl.Map.Resolutions[4]);  //0 zoomed out-19 zoomed in
  }
}
