# Ntreev.AspNetCore.WebSocketIo

웹용 REST API 를 웹소켓(WebSocket) 에서 재사용할 수 있는 라이브러리입니다.

일반적으로 웹으로 제공하는 REST API 를 사용하기 위해서 HTTP 통신이 필요합니다. 
웹소켓을 이용하여 기존에 만들어 놓은 REST API 를 사용하기 위해서는 웹소켓 통신으로 처리할 수 있는 로직을 중복으로 재사용하거나, 별도의 처리가 필요합니다.

이는 매우 비효율적 입니다. 기존의 REST API 를 만드는 개발 경험을 그대로 살려 웹소켓으로 개발할 수 없기 때문입니다.

Ntreev.AspNetCore.WebSocketIo 라이브러리는 이 문제를 해결하기 위해 만들어졌습니다. 이미 만들어진 REST API 는 웹소켓과 아주 쉽게 연동할 수 있습니다.

## 지원하는 기능

1. REST API 를 HTTP 통신과 웹소켓으로 호출할 수 있는 기능 제공
2. JWT 인증 기능 제공
3. 예외에 대한 처리 기능 제공
4. 채널(방) 기능 제공

## 제한 사항

1. HttpGetAttribute, HttpPostAttribute, HttpPutAttribute, HttpDeleteAttribute 특성 대신 RouteAttribute 을 사용해야 합니다.

## 라이선스

MIT License

Copyright (c) 2018 Ntreev Soft co., Ltd.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.