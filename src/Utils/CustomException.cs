namespace Harkh_app_production.src.Utils
{
    public class CustomException : Exception
    {

        public int StatusCode { get; set; }
        public CustomException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }


        // not found
        public static CustomException NotFound(string message = "Item not found")
        {
            return new CustomException(404, message);
        }

        // bad request
        public static CustomException BadRequest(string message = "Bad request")
        {
            return new CustomException(400, message);
        }


        // auth
        public static CustomException UnAuthorized(string message = "Unauthorized. Please log in")
        {
            return new CustomException(401, message);
        }

        public static CustomException Forbidden(string message = "Forbidden. The user does not have access rights to the content")
        {
            return new CustomException(403, message);
        }


        // internal
        public static CustomException InternalError(string message = "Internal server error")
        {
            return new CustomException(500, message);
        }
    }
}