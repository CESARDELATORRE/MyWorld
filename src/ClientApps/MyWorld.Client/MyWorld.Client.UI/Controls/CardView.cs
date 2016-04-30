using Xamarin.Forms;

namespace MyWorld.Client.UI
{
    public class CardView : Frame
    {
        public CardView()
        {
            Padding = 0;
            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.iOS)
            {
                HasShadow = true;
                OutlineColor = Color.Transparent;
                BackgroundColor = Color.Transparent;
            }
        }
    }
}

