using OpenCvSharp;

namespace MjpegStreamApi.Services.Impl
{
    public class CameraService : ICameraService
    {
        private readonly VideoCapture cap = new();

        private readonly bool _isOpened = false;

        private byte[]? imgBytes = [];

        public CameraService()
        {
            //打开本机第一个摄像头
            _isOpened = cap.Open(0);

            Task.Run(GrabImageLoop);
        }

        private async Task GrabImageLoop()
        {
            var mat = new Mat();
            while (_isOpened)
            {
                bool readSuccess = cap.Read(mat);
                if (!readSuccess)
                {
                    break;
                }

                var buf = mat.ToMemoryStream(".jpeg").ToArray();

                imgBytes = buf.Clone() as byte[];

                await Task.Delay(1);
            }
        }

        public byte[]? GetImageBytes()
        {
            if (!_isOpened)
            {
                throw new InvalidDataException("摄像头打开失败");
            }

            return imgBytes;
        }
    }
}
