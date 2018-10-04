using Microsoft.IdentityModel.Tokens;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓에서 JWT 옵션을 설정하는 클래스 입니다.
    /// </summary>
    public class WebSocketIoJwtOption
    {
        /// <summary>
        /// 토큰 유효성을 검사하기 위한 매개변수 입니다.
        /// </summary>
        public TokenValidationParameters TokenValidationParameters;
    }
}
