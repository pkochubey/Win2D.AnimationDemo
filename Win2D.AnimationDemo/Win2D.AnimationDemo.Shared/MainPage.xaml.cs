using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Win2D.AnimationDemo
{
    public enum Direction
    {
        Left,
        Right
    }

    public sealed partial class MainPage : Page
    {
        private CanvasBitmap sprite;
        private Direction direction;
        private int spriteCount = 8;
        private int spriteWidth = 108;
        private int spriteX = 0;
        private int spriteY = 0;

        public MainPage()
        {
            InitializeComponent();

            direction = Direction.Right;

            animateControl.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 33); // 30 fps, default 60 fps (16.6 ms)
            animateControl.Input.PointerPressed += OnPointerPressed;
        }

        private void OnPointerPressed(object sender, Windows.UI.Core.PointerEventArgs args)
        {
            if (direction != Direction.Left)
            {
                direction = Direction.Left;
                return;
            }

            direction = Direction.Right;
        }

        private void CanvasAnimatedControl_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var session = args.DrawingSession;

            session.DrawImage(sprite, 80, 80, new Rect(new Point(spriteX, spriteY), new Size(spriteWidth, 140)));

            if (direction == Direction.Right)
            {
                spriteY = 0;
                spriteX += spriteWidth;
                if (spriteX >= spriteWidth * 8)
                {
                    spriteX = 0;
                }
            } else if (direction == Direction.Left) {
                spriteY = 140;
                spriteX -= spriteWidth;
                if (spriteX <= 0)
                {
                    spriteX = spriteWidth * 7;
                }
            }
        }

        private void CanvasAnimatedControl_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            sprite = await CanvasBitmap.LoadAsync(sender, "Resource/scottpilgrim.png");
        }
    }
}
