using System.Linq;
using Xamarin.Forms;
using MyWorld.Client.UI.Interfaces;

namespace MyWorld.Client.UI.Controls
{
    public class Accordion : ItemsView
    {
        private View _detailView;
        private ScrolledEventArgs _scrolledEventArgs;
        private double _lastScrollPosition;
        private readonly ContentView _overlay;

        public Accordion()
        {
            //(CDLTLL) Wrapping the item detail in a ScrollView.  
            //This allows to naturally scroll the item detail into view within its own scroll view.
            ScrollView.Scrolled += AccordionViewScrolled;

            _overlay = new ContentView
            {
                IsVisible = false
            };

            //(CDLTLL-TOCHECK) TapGestureRecognizer is obsolte
            AddGesture(_overlay, new TapGestureRecognizer(view =>
            {
                _overlay.IsVisible = false;
                SelectedCommand.Execute(SelectedItem);
            }));

            Children.Add(_overlay);
        }

        private void AccordionViewScrolled(object sender, ScrolledEventArgs e)
        {
            _scrolledEventArgs = e;
        }

        protected override void SetSelected(ISelectable selectable)
        {
            selectable.IsSelected = !selectable.IsSelected;
        }

        public static readonly BindableProperty ItemDetailTemplateProperty =
            BindableProperty.Create<Accordion, DataTemplate>(p => p.ItemDetailTemplate, default(DataTemplate)
            , BindingMode.TwoWay, null, ItemDetailTemplateChanged);

        public DataTemplate ItemDetailTemplate
        {
            get { return (DataTemplate)GetValue(ItemDetailTemplateProperty); }
            set { SetValue(ItemDetailTemplateProperty, value); }
        }

        private static void ItemDetailTemplateChanged(BindableObject bindable, DataTemplate oldvalue, DataTemplate newvalue)
        {
            var accordionView = bindable as Accordion;
            if (accordionView == null) return;
            accordionView.CreateDetailView();
        }

        private void CreateDetailView()
        {
            var itemDetail = ItemDetailTemplate.CreateContent() as View;

            _detailView = new ScrollView
            {
                Content = itemDetail
            };
        }

        protected override void SetSelectedItem(ISelectable selectedItem)
        {
            base.SetSelectedItem(selectedItem);

            var element = ItemsStackLayout.Children.FirstOrDefault(x => x.BindingContext == selectedItem);
            if (element == null) return;

            var index = ItemsStackLayout.Children.IndexOf(element);
            var scrollPosition = _scrolledEventArgs != null ? _scrolledEventArgs.ScrollX : 0;

            if (selectedItem.IsSelected)
            {
                var scrollDistance = element.X - scrollPosition; // the distance to scroll
                _lastScrollPosition = scrollPosition;

                if (Device.OS != TargetPlatform.WinPhone)
                {
                    if (ItemsStackLayout.Children.Contains(_detailView))
                        ItemsStackLayout.Children.Remove(_detailView);
                }
                else
                    CreateDetailView();

                _detailView.BindingContext = selectedItem;

                if (scrollDistance < Width / 2)
                    ItemsStackLayout.Children.Insert(index + 1, _detailView);
                else
                    ItemsStackLayout.Children.Insert(index, _detailView);

                var width = Width - element.Width; // width to expand to

                _overlay.IsVisible = true;

                _detailView.Animate("expand",
                    percent =>
                    {
                        var change = width * percent;
                        _detailView.WidthRequest = change;

                        var position = scrollPosition + (scrollDistance * percent);
                        ScrollView.ScrollToAsync(position, 0, false);

                    }, 0, 400, Easing.Linear, (d, b) =>   //(CDLTLL) Set the animation time to 400ms and specify Linear Easing
                    {
                        _overlay.IsVisible = true;
                    });
            }
            else
            {
                var width = _detailView.WidthRequest; // width to collapse
                var scrollDistance = scrollPosition - _lastScrollPosition; // the distance to scroll

                _detailView.Animate("collapse",
                    percentage =>
                    {
                        var change = width * percentage;
                        _detailView.WidthRequest = width - change;

                        var position = scrollPosition - (scrollDistance * percentage);
                        ScrollView.ScrollToAsync(position, 0, false);

                    }, 0, 400, Easing.Linear, (d, b) =>     //(CDLTLL) Set the animation time to 400ms and specify Linear Easing
                    {
                        ItemsStackLayout.Children.Remove(_detailView);
                        _detailView.Parent = null;
                        _overlay.IsVisible = false;
                    });
            }
        }
    }
}
