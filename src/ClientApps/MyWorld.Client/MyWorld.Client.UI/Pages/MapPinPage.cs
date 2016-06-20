using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MyWorld.Client.UI
{
	public class MapPinPage : ContentPage
	{
		Map map;

		public MapPinPage()
		{
			map = new Map { 
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			map.MoveToRegion (MapSpan.FromCenterAndRadius (
				new Position (36.9628066,-122.0194722), Distance.FromMiles (3))); // Santa Cruz golf course

			var position = new Position(36.9628066,-122.0194722); // Latitude, Longitude
			var pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = "Santa Cruz",
				Address = "custom detail info"
			};
			map.Pins.Add(pin);


			// create buttons
			var morePins = new Button { Text = "Add more pins" };
			morePins.Clicked += (sender, e) => {
				map.Pins.Add(new Pin {
					Position = new Position(36.9641949,-122.0177232),
					Label = "Boardwalk"
				});
				map.Pins.Add(new Pin {
					Position = new Position(36.9571571,-122.0173544),
					Label = "Wharf"
				});
				map.MoveToRegion (MapSpan.FromCenterAndRadius (
					new Position (36.9628066,-122.0194722), Distance.FromMiles (1.5)));

			};

            var reLocateToRedmond = new Button { Text = "Redmond move" };
            reLocateToRedmond.Clicked += (sender, e) => {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(47.661407, -122.131213), Distance.FromMiles(3)));
            };

            var reLocate = new Button { Text = "Re-center" };
			reLocate.Clicked += (sender, e) => {
				map.MoveToRegion (MapSpan.FromCenterAndRadius (
					new Position (47.661407, -122.131213), Distance.FromMiles (3)));
			};

            
            
            //DELETE PINS button
            var deletePins = new Button { Text = "Delete pins" };
            deletePins.Clicked += (sender, e) => {

                map.Pins.Clear();

                //for (int i = map.Pins.Count - 1; i >= 0; i--)
                //{
                //    map.Pins.RemoveAt(i);
                //}
            };

            //Add buttons to page
            var buttons = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Children = {
					morePins, deletePins, reLocateToRedmond, reLocate
                }
			};

            // put the page together
            Content = new StackLayout { 
				Spacing = 0,
				Children = {
					map,
					buttons
				}};
		}
	}
}

