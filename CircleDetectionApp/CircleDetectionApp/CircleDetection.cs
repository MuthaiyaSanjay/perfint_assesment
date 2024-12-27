using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;

namespace CircleDetection
{
    class CircleDetection
    {
        static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message}");
        }
        static string GetImagePath(string imageName)
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "img", imageName);
            return imagePath;
        }

        static Mat LoadImage(string imageName)
        {
            string imagePath = GetImagePath(imageName);
            Log($"Loading image from path: {imagePath}");

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                Log("Error: Image path is invalid or the file does not exist.");
                return null;
            }

            try
            {
                Mat image = CvInvoke.Imread(imagePath, ImreadModes.Color);
                if (image.IsEmpty)
                {
                    Log("Error: Could not load the image.");
                    return null;
                }
                Log("Image loaded successfully.");
                return image;
            }
            catch (Exception ex)
            {
                Log($"Error while loading the image: {ex.Message}");
                return null;
            }
        }

        static void DetectCircles(Mat image)
        {
            Log("Detecting circles using Hough Circle Transform...");

            Mat grayImage = new Mat();
            CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);

            Mat blurredImage = new Mat();
            CvInvoke.GaussianBlur(grayImage, blurredImage, new Size(9, 9), 2, 2);

            CircleF[] circles = CvInvoke.HoughCircles(
                blurredImage,           // Input image
                HoughModes.Gradient,    // Circle detection method
                dp: 1.0,                // Resolution of the accumulator
                minDist: 20,            // Minimum distance between circle centers
                param1: 50,             // High threshold for the Canny edge detector
                param2: 30,             // Threshold for center detection
                minRadius: 10,          // Minimum circle radius
                maxRadius: 100          // Maximum circle radius
            );
            int circleCount = 0;
            foreach (var circle in circles)
            {
                Point center = new Point((int)circle.Center.X, (int)circle.Center.Y);
                int radius = (int)circle.Radius;
                CvInvoke.Circle(image, center, radius, new MCvScalar(0, 255, 0), 2);
                circleCount++;
                Log($"Detected a circle at ({center.X}, {center.Y}) with radius {radius}.");
            }
            string countMessage = $"Circles detected: {circleCount}";
            CvInvoke.PutText(image, countMessage, new Point(10, 30), FontFace.HersheySimplex, 1, new MCvScalar(0, 0, 255), 2);
            Log($"Total Circles Detected: {circleCount}");
        }

        static void SaveImage(Mat image, string outputPath)
        {
            try
            {
                CvInvoke.Imwrite(outputPath, image);
                Log($"Image saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Log($"Error while saving the image: {ex.Message}");
            }
        }

        static void Main(string[] args)
        {
            string inputImageName = "circle.png";
            Mat image = LoadImage(inputImageName);
            if (image == null) return;
            DetectCircles(image);
            CvInvoke.Imshow("Detected Circles", image);
            CvInvoke.WaitKey(0);
            string outputImagePath = "output_detected_circles.png";
            SaveImage(image, outputImagePath);
        }
    }
}
