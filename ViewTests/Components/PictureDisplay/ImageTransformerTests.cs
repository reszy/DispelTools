using View.Components.PictureDisplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NUnit.Framework;

namespace View.Components.PictureDisplay.Tests
{
    public class ImageTransformerTests
    {
        [TestCase(50, 50, 2)]
        [TestCase(150, 50, 3)]
        [Apartment(ApartmentState.STA)]
        public void ShouldZoomBeReversible(int x, int y, double firstZoom)
        {
            //given
            var image = new Image();
            var transformer = new ImageTransformer(image);


            //when zoom 2 times
            transformer.Zoom(x, y, firstZoom);

            //and zoom back from same place
            transformer.Zoom(x, y, 1);

            //then position should not be changed
            Assert.That(transformer.X, Is.Zero);
            Assert.That(transformer.Y, Is.Zero);
        }

        [TestCase]
        [Apartment(ApartmentState.STA)]
        public void ShouldZoomToCorrectPosition()
        {
            //given
            var image = new Image();
            var transformer = new ImageTransformer(image);
            transformer.Move(-50, -25);

            //when zoom 2 times
            transformer.Zoom(25, 20, 2);

            //then position should not be changed
            Assert.That(transformer.X, Is.EqualTo(-125));
            Assert.That(transformer.Y, Is.EqualTo(-70));
        }
    }
}