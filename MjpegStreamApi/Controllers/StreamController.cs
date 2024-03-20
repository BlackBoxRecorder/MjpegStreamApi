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
            Response.Headers.Connection = "close"; // 关闭连接，因为multipart/x-mixed-replace不保持连接

            await Response.BodyWriter.FlushAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                var imageBytes = _cameraService.GetImageBytes();

                // 写入boundary
                await Response.WriteAsync(
                    $"{BOUNDARY}\r\nContent-Type: image/jpeg\r\n\r\n",
                    cancellationToken: cancellationToken
                );
                // 写入图片数据
                await Response.BodyWriter.WriteAsync(imageBytes, cancellationToken);
                // 刷新输出，确保图片被发送
                await Response.BodyWriter.FlushAsync(cancellationToken);

                // 等待一段时间后捕获下一帧
                await Task.Delay(30, cancellationToken);
            }
        }
    }
}
