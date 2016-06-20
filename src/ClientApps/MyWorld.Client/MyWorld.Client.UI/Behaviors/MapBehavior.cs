using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

using MyWorld.Client.Core.Maps;

namespace MyWorld.Client.UI.Behaviors
{
    public class MapBehavior : BindableBehavior<Map>
    {
        //(CDLTLL-Check these warnings, deprecated APIs)
        // See --> https://forums.xamarin.com/discussion/63268/bindableproperty-pieces-obsoleting

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create<MapBehavior, IEnumerable<ILocationViewModel>>(
            p => p.ItemsSource, null, BindingMode.Default, null, ItemsSourceChanged);

        public static readonly BindableProperty VisibleRegionProperty = BindableProperty.Create<MapBehavior, MapSpan>(
            p => p.VisibleRegion, null, BindingMode.TwoWay, null, VisibleRegionChanged);

        protected override void OnAttachedTo(Map map)
        {
            base.OnAttachedTo(map);

            map.PropertyChanged += MapOnPropertyChanged;
        }

        protected override void OnDetachingFrom(Map map)
        {
            base.OnDetachingFrom(map);

            map.PropertyChanged -= MapOnPropertyChanged;
        }

        private void MapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var map = sender as Map;
            if (map != null && propertyChangedEventArgs.PropertyName == "VisibleRegion")
            {
                VisibleRegion = map.VisibleRegion;
            }
        }

        public IEnumerable<ILocationViewModel> ItemsSource
        {
            get { return (IEnumerable<ILocationViewModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public MapSpan VisibleRegion
        {
            get { return (MapSpan)GetValue(VisibleRegionProperty); }
            set { SetValue(VisibleRegionProperty, value); }
        }

        private static void ItemsSourceChanged(BindableObject bindable, IEnumerable oldValue, IEnumerable newValue)
        {
            var behavior = bindable as MapBehavior;
            if (behavior == null) return;
            behavior.AddPins();
        }

        private static void VisibleRegionChanged(BindableObject bindable, MapSpan oldValue, MapSpan newValue)
        {
            var behavior = bindable as MapBehavior;
            if (behavior == null) return;
            behavior.SetVisibleRegion(oldValue, newValue);
        }

        private void SetVisibleRegion(MapSpan oldValue, MapSpan newValue)
        {
            if (newValue == null)
            {
                return;
            }

            var map = AssociatedObject;
            var distance = double.MaxValue;
            if (oldValue != null)
            {
                distance = MapHelper.CalculateDistance(oldValue.Center.Latitude, oldValue.Center.Longitude, newValue.Center.Latitude, newValue.Center.Longitude, 'K');
            }

            if (distance > 1)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(newValue.Center, newValue.Radius));
            }
        }

        private void AddPins()
        {
            var map = AssociatedObject;

            //(CDLTLL) Clear Pins from Map
            map.Pins.Clear();
            //for (int i = map.Pins.Count - 1; i >= 0; i--)
            //{
            //    map.Pins[i].Clicked -= PinOnClicked;
            //    map.Pins.RemoveAt(i);
            //}

            var pins = ItemsSource.Select(x =>
            {
                var pin = new Pin
                {
                    Type = PinType.SearchResult,
                    Position = new Position(x.Latitude, x.Longitude),
                    Label = x.Title,
                    Address = x.Description,
                };

                pin.Clicked += PinOnClicked;
                return pin;
            }).ToArray();

            //(CDLTLL) Paint Pins on the Map
            foreach (var pin in pins)
                map.Pins.Add(pin);
        }
        /*
        private void UpdatePins(List<ILocationViewModel> pins)
        {
            if (Pins == null || Pins.Count == 0)
            {
                Pins = new ObservableCollection<ILocationViewModel>(pins);
            }
            else
            {
                var newPins = pins.ToDictionary(pin => pin.Key);

                for (int i = Pins.Count - 1; i >= 0; i--)
                {
                    var pin = Pins[i];
                    if (newPins.ContainsKey(pin.Key))
                    {
                        // Remove pins already available on map
                        newPins.Remove(pin.Key);
                    }
                    else
                    {
                        // Remove outside bounds of search
                        Pins.RemoveAt(i);
                    }
                }

                foreach (var key in newPins.Keys)
                {
                    Pins.Add(newPins[key]);
                }
            }
        }*/


        private void PinOnClicked(object sender, EventArgs eventArgs)
        {
            var pin = sender as Pin;
            if (pin == null) return;
            var viewModel = ItemsSource.FirstOrDefault(x => x.Title == pin.Label);
            if (viewModel == null || viewModel.Command == null) return;
            viewModel.Command.Execute(null);
        }

        private void PositionMap()
        {
            if (ItemsSource == null || !ItemsSource.Any()) return;

            var centerPosition = new Position(ItemsSource.Average(x => x.Latitude), ItemsSource.Average(x => x.Longitude));

            var minLongitude = ItemsSource.Min(x => x.Longitude);
            var minLatitude = ItemsSource.Min(x => x.Latitude);

            var maxLongitude = ItemsSource.Max(x => x.Longitude);
            var maxLatitude = ItemsSource.Max(x => x.Latitude);

            var distance = MapHelper.CalculateDistance(minLatitude, minLongitude,
                maxLatitude, maxLongitude, 'M') / 2;

            AssociatedObject.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance)));

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                AssociatedObject.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance)));
                return false;
            });
        }
    }
}
