using PersonalWebApp.Models;

namespace PersonalWebApp.Services {
	public abstract class BaseService {
		private ValidationResult FailValidation(string message) {
			return new ValidationResult { Valid = false, Message = message };
		}
	}
}