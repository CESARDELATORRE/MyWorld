using System;
using System.Linq;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

using MyWorld.Client.UI.Interfaces;


namespace MyWorld.Client.UI.Controls
{
    //Base class for the Accordion control
    public class ItemsView : Grid
    {
        protected ScrollView ScrollView;
        protected readonly ICommand SelectedCommand;
        protected readonly StackLayout ItemsStackLayout;

        public ItemsView()
        {
            ScrollView = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal
            };

            ItemsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0),
                Spacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            ScrollView.Content = ItemsStackLayout;
            Children.Add(ScrollView);

            SelectedCommand = new Command<object>(item =>
            {
                var selectable = item as ISelectable;
                if (selectable == null) return;

                SetSelected(selectable);
                SelectedItem = selectable.IsSelected ? selectable : null;
            });

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Orientation")
                {
                    ItemsStackLayout.Orientation = ScrollView.Orientation == ScrollOrientation.Horizontal ? StackOrientation.Horizontal : StackOrientation.Vertical;
                }

            };
        }

        protected virtual void SetSelected(ISelectable selectable)
        {
            selectable.IsSelected = true;
        }

        public bool ScrollToStartOnSelected { get; set; }

        public event EventHandler SelectedItemChanged;

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<ItemsView, IEnumerable>(p => p.ItemsSource, default(IEnumerable<object>), BindingMode.TwoWay, null, ItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create<ItemsView, object>(p => p.SelectedItem, default(object), BindingMode.TwoWay, null, OnSelectedItemChanged);

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create<ItemsView, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void ItemsSourceChanged(BindableObject bindable, IEnumerable oldValue, IEnumerable newValue)
        {
            var itemsLayout = (ItemsView)bindable;
            itemsLayout.SetItems();
        }

        protected virtual void SetItems()
        {
            ItemsStackLayout.Children.Clear();

            if (ItemsSource == null)
                return;

            foreach (var item in ItemsSource)
                ItemsStackLayout.Children.Add(GetItemView(item));

            SelectedItem = ItemsSource.OfType<ISelectable>().FirstOrDefault(x => x.IsSelected);
        }

        protected virtual View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();
            var view = content as View;
            if (view == null) return null;

            view.BindingContext = item;

            var gesture = new TapGestureRecognizer
            {
                Command = SelectedCommand,
                CommandParameter = item
            };

            AddGesture(view, gesture);

            return view;
        }

        protected void AddGesture(View view, TapGestureRecognizer gesture)
        {
            view.GestureRecognizers.Add(gesture);

            var layout = view as Layout<View>;

            if (layout == null)
                return;

            foreach (var child in layout.Children)
                AddGesture(child, gesture);
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsView = (ItemsView)bindable;
            if (newValue == oldValue)
                return;

            var selectable = newValue as ISelectable;
            itemsView.SetSelectedItem(selectable ?? oldValue as ISelectable);
        }

        protected virtual void SetSelectedItem(ISelectable selectedItem)
        {
            var items = ItemsSource;

            foreach (var item in items.OfType<ISelectable>())
                item.IsSelected = selectedItem != null && item == selectedItem && selectedItem.IsSelected;

            var handler = SelectedItemChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

    }
}
