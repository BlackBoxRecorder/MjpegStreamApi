using Microsoft.AspNetCore.Mvc;
using MjpegStreamApi.Services;

namespace MjpegStreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StreamController(ICameraService cameraService) : ControllerBase
    {
        private const string BOUNDARY = "--BoundaryString";

        private readonly string CONTENT_TYPE = $"multipart/x-mixed-replace; boundary={BOUNDARY}";

        private readonly ICameraService _cameraService = cameraService;

        [HttpGet("video")]
        public async Task VideoStream(CancellationToken cancellationToken)
        {
            Response.ContentType = CONTENT_TYPE;
            Response.Headers.Connection = "close"; // �ر����ӣ���Ϊmultipart/x-mixed-replace����������

            await Response.BodyWriter.FlushAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                var imageBytes = _cameraService.GetImageBytes();

                // д��boundary
                await Response.WriteAsync(
                    $"{BOUNDARY}\r\nContent-Type: image/jpeg\r\n\r\n",
                    cancellationToken: cancellationToken
                );
                // д��ͼƬ����
                await Response.BodyWriter.WriteAsync(imageBytes, cancellationToken);
                // ˢ�������ȷ��ͼƬ������
                await Response.BodyWriter.FlushAsync(cancellationToken);

                // �ȴ�һ��ʱ��󲶻���һ֡
                await Task.Delay(30, cancellationToken);
            }
        }
    }
}
