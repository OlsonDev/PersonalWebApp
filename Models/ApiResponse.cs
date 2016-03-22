using System;

namespace PersonalWebApp.Models {
	public class ApiResponse {
		public bool Valid { get; protected set; }
		public string Message { get; protected set; }
		public object Result { get; protected set; }
	}

	public class InvalidApiResponse : ApiResponse {
		public InvalidApiResponse(Exception exception) {
			Valid = false;
			Message = exception.Message;
			Result = exception; // TODO: Only do this on for local/debug requests
		}
	}

	public class ValidApiResponse : ApiResponse {
		public ValidApiResponse(object result) {
			Valid = true;
			Result = result;
		}
	}
}