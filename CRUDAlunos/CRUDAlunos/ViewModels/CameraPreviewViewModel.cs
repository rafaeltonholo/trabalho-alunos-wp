using CRUDAlunos.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CRUDAlunos.ViewModels {
    public class CameraPreviewViewModel : ICameraPreviewViewModel {
        private Visibility _previewVisibility;
        private MediaCapture _previewImageSource;
        private Visibility _imageVisibility;
        private ImageSource _imageSource;
        private double _finalPhotoAreaBorderWidth;
        private double _finalPhotoAreaBorderHeight;
        private Thickness _finalPhotoAreaBorderMargin;
        private Visibility _finalPhotoAreaBorderVisibility;

        private DelegateCommand _saveCommand;

        private bool _isPreviewing;
        private MediaCapture _captureManager;
        private WriteableBitmap _bitmap;
        private CaptureElement _captureElement;

        public CaptureElement CaptureElement {
            get {
                return this._captureElement;
            }
            set {
                this._captureElement = value;
            }
        }

        public MediaCapture PreviewImageSource {
            get {
                return _previewImageSource;
            }
            set {
                _previewImageSource = value;
                this.CaptureElement.Source = this._previewImageSource;
            }
        }

        public Visibility PreviewVisibility {
            get { return this._previewVisibility; }
            set {
                this._previewVisibility = value;
                this.CaptureElement.Visibility = this._previewVisibility;
            }
        }

        public ImageSource ImageSource {
            get { return this._imageSource; }
            set {
                this._imageSource = value;
                RaisedPropertyChanged(() => ImageSource);
            }
        }

        public Visibility ImageSourceVisibility {
            get { return this._imageVisibility; }
            set {
                this._imageVisibility = value;
                RaisedPropertyChanged(() => ImageSourceVisibility);
            }
        }

        public double FinalPhotoAreaBorderWidth {
            get { return _finalPhotoAreaBorderWidth; }
            set {
                this._finalPhotoAreaBorderWidth = value;
                RaisedPropertyChanged(() => FinalPhotoAreaBorderWidth);
            }
        }

        public double FinalPhotoAreaBorderHeight {
            get { return _finalPhotoAreaBorderHeight; }
            set {
                _finalPhotoAreaBorderHeight = value;
                RaisedPropertyChanged(() => FinalPhotoAreaBorderHeight);
            }
        }

        public Thickness FinalPhotoAreaBorderMargin {
            get { return _finalPhotoAreaBorderMargin; }
            set {
                _finalPhotoAreaBorderMargin = value;
                RaisedPropertyChanged(() => FinalPhotoAreaBorderMargin);
            }
        }

        public Visibility FinalPhotoAreaBorderVisibility {
            get { return _finalPhotoAreaBorderVisibility; }
            set {
                this._finalPhotoAreaBorderVisibility = value;
                RaisedPropertyChanged(() => FinalPhotoAreaBorderVisibility);
            }
        }

        public DelegateCommand SaveCommand {
            get {
                return this._saveCommand ?? (this._saveCommand = new DelegateCommand(() => {
                    if (_isPreviewing) {
                        PreviewVisibility = Visibility.Collapsed;

                        //if (GetDisplayAspectRatio() == DisplayAspectRatio.FifteenByNine) {
                        CaptureFifteenByNineImage();
                        //}

                        //if (GetDisplayAspectRatio() == DisplayAspectRatio.SixteenByNine) {
                        //  CaptureSixteenByNineImage();
                        //}
                    }
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<PhotoTakedEventArgs> PhotoTaked;

        public CameraPreviewViewModel() {
            InitializePreview();
        }

        /// <summary>
        /// initializes the preview with our desired settings
        /// </summary>
        private async void InitializePreview() {
            _captureManager = new MediaCapture();

            var cameraID = await GetCameraID(Windows.Devices.Enumeration.Panel.Back);

            await _captureManager.InitializeAsync(new MediaCaptureInitializationSettings {
                StreamingCaptureMode = StreamingCaptureMode.Video,
                PhotoCaptureSource = PhotoCaptureSource.Photo,
                AudioDeviceId = string.Empty,
                VideoDeviceId = cameraID.Id,
            });

            await _captureManager.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.Photo, MaxResolution());

            _captureManager.SetPreviewRotation(VideoRotation.Clockwise90Degrees);

            StartPreview();
        }

        /// <summary>
        /// starts the preview and effectifely shows the finalPhotoAreaBorder for 15:9 devices
        /// </summary>
        private async void StartPreview() {
            SetFocus();

            PreviewVisibility = Visibility.Visible;
            PreviewImageSource = _captureManager;
            await _captureManager.StartPreviewAsync();

            _isPreviewing = true;

            //if (GetDisplayAspectRatio() == DisplayAspectRatio.FifteenByNine) {
                GetBounds();
            //}

        }

        /// <summary>
        /// stops preview and frees all system resources, and converts the app back to the default values.
        /// </summary>
        private async void CleanCapture() {
            if (_captureManager != null) {
                if (_isPreviewing == true) {
                    await _captureManager.StopPreviewAsync();
                    _isPreviewing = false;
                }
                _captureManager.Dispose();

                PreviewImageSource = null;
                PreviewVisibility = Visibility.Collapsed;
                ImageSource = null;
                ImageSourceVisibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// captures and saves the 15:9 image with rotation and cropping applied
        /// </summary>
        private async void CaptureFifteenByNineImage() {
            //declare string for filename
            string captureFileName = string.Empty;
            //declare image format
            ImageEncodingProperties format = ImageEncodingProperties.CreateJpeg();

            using (var imageStream = new InMemoryRandomAccessStream()) {
                //generate stream from MediaCapture
                await _captureManager.CapturePhotoToStreamAsync(format, imageStream);

                //create decoder and transform
                BitmapDecoder dec = await BitmapDecoder.CreateAsync(imageStream);
                BitmapTransform transform = new BitmapTransform();

                //roate the image
                transform.Rotation = BitmapRotation.Clockwise90Degrees;
                transform.Bounds = GetBounds();

                //get the conversion data that we need to save the cropped and rotated image
                BitmapPixelFormat pixelFormat = dec.BitmapPixelFormat;
                BitmapAlphaMode alpha = dec.BitmapAlphaMode;

                //read the PixelData
                PixelDataProvider pixelProvider = await dec.GetPixelDataAsync(
                    pixelFormat,
                    alpha,
                    transform,
                    ExifOrientationMode.RespectExifOrientation,
                    ColorManagementMode.ColorManageToSRgb
                    );
                byte[] pixels = pixelProvider.DetachPixelData();

                if (PhotoTaked != null) {
                    PhotoTaked(this, new PhotoTakedEventArgs(pixels));
                }

                Frame rootFrame = Window.Current.Content as Frame;

                if (rootFrame != null && rootFrame.CanGoBack) {
                    rootFrame.GoBack();
                }
            }

            CleanCapture();

        }




        #region [ Helpers ]
        /// <summary>
        /// get highest possible resolution
        /// </summary>
        /// <returns></returns>
        private VideoEncodingProperties MaxResolution() {
            VideoEncodingProperties resolutionMax = null;

            //get all photo properties
            var resolutions = _captureManager.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo);

            //generate new list to work with
            List<VideoEncodingProperties> vidProps = new List<VideoEncodingProperties>();

            //add only those properties that are 16:9 to our own list
            for (var i = 0; i < resolutions.Count; i++) {
                VideoEncodingProperties res = (VideoEncodingProperties)resolutions[i];

                if (MatchScreenFormat(new Size(res.Width, res.Height)) != CameraResolutionFormat.FourByThree) {
                    vidProps.Add(res);
                }
            }

            //order the list, and select the highest resolution that fits our limit
            if (vidProps.Count != 0) {
                vidProps = vidProps.OrderByDescending(r => r.Width).ToList();

                resolutionMax = vidProps.Where(r => r.Width < 2600).First();
            }

            return resolutionMax;
        }


        /// <summary>
        /// get device panel to interact with the correct camera
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        private static async Task<DeviceInformation> GetCameraID(Windows.Devices.Enumeration.Panel camera) {
            DeviceInformation deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == camera);

            return deviceID;
        }

        /// <summary>
        /// the camera resolution format (aspect ratio)
        /// </summary>
        public enum CameraResolutionFormat {
            Unknown = -1,

            FourByThree = 0,

            SixteenByNine = 1
        }

        /// <summary>
        /// Helper to detect the correct camera resolution
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        private CameraResolutionFormat MatchScreenFormat(Size resolution) {
            CameraResolutionFormat result = CameraResolutionFormat.Unknown;

            double relation = Math.Max(resolution.Width, resolution.Height) / Math.Min(resolution.Width, resolution.Height);
            if (Math.Abs(relation - (4.0 / 3.0)) < 0.01) {
                result = CameraResolutionFormat.FourByThree;
            } else if (Math.Abs(relation - (16.0 / 9.0)) < 0.01) {
                result = CameraResolutionFormat.SixteenByNine;
            }

            return result;
        }

        public void RaisedPropertyChanged<Type>(Expression<Func<Type>> expression) {
            var member = expression.Body as MemberExpression;
            var pInfo = member.Member as PropertyInfo;

            if (pInfo != null)
                PropertyChanged(this, new PropertyChangedEventArgs(pInfo.Name));
        }

        #region [ Display Helpers ]

        /// <summary>
        /// Helper to get the correct Bounds for 15:9 screens and to set finalPhotoAreaBorder values
        /// </summary>
        /// <returns></returns>
        private BitmapBounds GetBounds() {
            BitmapBounds bounds = new BitmapBounds();

            //image size is raw pixels, so we need also here raw pixels
            double logicalPixelWidth = Windows.UI.Xaml.Window.Current.Bounds.Width;
            double logicalPixelHeight = Windows.UI.Xaml.Window.Current.Bounds.Height;

            double rawPerViewPixels = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            double rawPixelHeight = logicalPixelHeight * rawPerViewPixels;
            double rawPixelWidth = logicalPixelWidth * rawPerViewPixels;

            //calculate scale factor of UniformToFill Height (remember, we rotated the preview)
            double scaleFactorVisualHeight = MaxResolution().Width / rawPixelHeight;

            //calculate the visual Width 
            //(because UniFormToFill scaled the previewElement Width down to match the previewElement Height)
            double visualWidth = MaxResolution().Height / scaleFactorVisualHeight;

            //calculate cropping area for 15:9
            uint scaledBoundsWidth = MaxResolution().Height;
            uint scaledBoundsHeight = (scaledBoundsWidth / 9) * 15;

            //we are starting at the top of the image
            bounds.Y = 0;
            //cropping the image width
            bounds.X = 0;
            bounds.Height = scaledBoundsHeight;
            bounds.Width = scaledBoundsWidth;

            //set finalPhotoAreaBorder values that shows the user the area that is captured
            FinalPhotoAreaBorderWidth = (scaledBoundsWidth / scaleFactorVisualHeight) / rawPerViewPixels;
            FinalPhotoAreaBorderHeight = (scaledBoundsHeight / scaleFactorVisualHeight) / rawPerViewPixels;
            FinalPhotoAreaBorderMargin = new Thickness(
                                            Math.Floor(((rawPixelWidth - visualWidth) / 2) / rawPerViewPixels),
                                            0,
                                            Math.Floor(((rawPixelWidth - visualWidth) / 2) / rawPerViewPixels),
                                            0);

            FinalPhotoAreaBorderVisibility = Visibility.Visible;

            return bounds;
        }

        #endregion [ Display Helpers ]

        #endregion [ Helpers ]
    }
}
