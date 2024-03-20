## 摄像头视频通过 MJPEG 在网页上播放

一个 asp.net core demo

`Content-Type: multipart/x-mixed-replace;` 是一种特殊的HTTP内容类型，它用于服务器向客户端发送一个连续的数据流，其中每个部分都替换前一个部分。这种内容类型通常用于动态生成的内容，如实时更新的图片或其他多媒体数据。
当服务器使用 `multipart/x-mixed-replace` 响应时，它会发送一系列的HTTP消息部分，每个部分之间用分界字符串分隔。每个部分都可以包含自己的HTTP头，如 `Content-Type` 和 `Content-Length`，以及数据本身。客户端通常会在接收到新的部分后立即替换旧的部分。
这种机制的一个常见应用是网络摄像头的实时视频流。服务器可以不断发送新的图片帧作为HTTP响应的一部分，客户端（通常是浏览器）则不断地更新显示的图片。这种方式下，用户可以看到连续的视频流，而不需要等待整个视频下载完成。
以下是一个使用 `multipart/x-mixed-replace` 的HTTP响应的示例：

```
HTTP/1.1 200 OK
Content-Type: multipart/x-mixed-replace; boundary=myboundary
--myboundary
Content-Type: image/jpeg
Content-Length: 12345
[JPEG image data here]
--myboundary
Content-Type: image/jpeg
Content-Length: 12346
[JPEG image data here]
--myboundary--
```
在这个示例中，每个图片帧都作为一个独立的部分发送，并且每个部分都有自己的 `Content-Type` 和 `Content-Length` 头。分界字符串 `myboundary` 用于分隔不同的部分。
