using System;

namespace PersonalWebApp.Models {
	public class ApiResponse {
		public bool Valid { get; protected set; }
		public string Message { get; protected set; } = string.Empty;
		public object Result { get; protected set; } = new object();
	}

	public class InvalidApiResponse : ApiResponse {
		public InvalidApiResponse() {}

		public InvalidApiResponse(Exception exception) {
			Message = exception.Message;
			Result = exception; // TODO: Only do this on for local/debug requests
		}

		public InvalidApiResponse(object result) {
			Result = result;
		}
	}

	public class ValidApiResponse : ApiResponse {
		public ValidApiResponse(object result) {
			Valid = true;
			Result = result;
		}
	}
}